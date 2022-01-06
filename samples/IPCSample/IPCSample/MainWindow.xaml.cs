using MessagesLibrary;
using NamedPipeWrapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IPCSample
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// 当前系统的所有命名管道可有有各种方式来查看，有些需要在Win10操作系统上操作，如下：
    /// Powershell中可查看所有的命名管道列表：[System.IO.Directory]::GetFiles("\\.\\pipe\\") 或者 get-childitem \\.\pipe\
    /// cmd命令行中也可用来查看所有的命名管道列表： dir \\.\pipe\\ 
    /// 微软Sysinternals Suite工具包中的管道查看命令行工具：PipeList
    /// Chrome浏览器中输入地址来查看：file://./pipe// 或者直接输入 \\.\pipe\\
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _serverPipeId = System.Guid.NewGuid().ToString();

        private Dictionary<string, string> _clientsDict = new Dictionary<string, string>();

        private NamedPipeServer<ICommandMessage> _commandMessageServer;

        public MainWindow()
        {
            InitializeComponent();

            InitServer();
        }

        private void InitServer()
        {
            IdTextBox.Text = _serverPipeId;

            _commandMessageServer = new NamedPipeServer<ICommandMessage>(MessagePipeUtil.GetServerPipeNameById(_serverPipeId));

            _commandMessageServer.ClientDisconnected += _commandMessageServer_ClientDisconnected;
            _commandMessageServer.ClientMessage += _commandMessageServer_ClientMessage;
            _commandMessageServer.Start();
        }

        private void _commandMessageServer_ClientDisconnected(NamedPipeConnection<ICommandMessage, ICommandMessage> connection)
        {
            foreach(var item in _clientsDict)
            {
                if(item.Value == connection.Name)
                {
                    _clientsDict.Remove(item.Key);

                    LogString($"执行器{item.Key}断开连接");
                    ExecutorIdComboBox.Dispatcher.Invoke(()=> {
                        ExecutorIdComboBox.Items.Remove(item.Key);
                        ExecutorIdComboBox.SelectedIndex = ExecutorIdComboBox.Items.Count - 1;
                    });
                    
                    break;
                }
            }
        }

        private void LogString(string text)
        {
            LogTextBox.Dispatcher.Invoke(()=> {
                LogTextBox.Text += text;
                LogTextBox.Text += Environment.NewLine;
            });
        }

        private void _commandMessageServer_ClientMessage(NamedPipeConnection<ICommandMessage, ICommandMessage> connection, ICommandMessage message)
        {
            if(message is RegisterMessage)
            {
                var msg = message as RegisterMessage;

                _clientsDict[msg.id] = connection.Name;
                LogString($"ID为{msg.id}的执行器注册成功");

                ExecutorIdComboBox.Dispatcher.Invoke(()=> {
                    ExecutorIdComboBox.Items.Add(msg.id);
                    ExecutorIdComboBox.SelectedIndex = ExecutorIdComboBox.Items.Count - 1;
                });
            }
            else if(message is ExecutorInfoMessage)
            {
                var msg = message as ExecutorInfoMessage;

                LogString($"ID为{msg.id}的执行器获取信息成功，信息内容为：{msg.msg}");
            }
        }

        /// <summary>
        /// 启动执行器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartExecutor_Click(object sender, RoutedEventArgs e)
        {
            var arguments = $"{_serverPipeId}";//将服务器的id做为命令行参数传递给执行器，以便执行器通过该id找到服务器
            using (Process process = new Process
            {
                StartInfo =
                {
                    FileName = "IPCExecutor.exe",
                    Arguments = arguments
                }
            })
            {
                process.Start();
            }
        }

        /// <summary>
        /// 获取指定的执行器信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetExecutorInfo_Click(object sender, RoutedEventArgs e)
        {
            var currentExecutorId = ExecutorIdComboBox.Text;

            foreach(var conn in _commandMessageServer._connections)
            {
                if(conn.Name == _clientsDict[currentExecutorId])
                {
                    conn.PushMessage(new GetExecutorInfoMessage());
                    break;
                }
            }
            
        }

        /// <summary>
        /// 关闭指定的执行器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseExecutor_Click(object sender, RoutedEventArgs e)
        {
            var currentExecutorId = ExecutorIdComboBox.Text;

            foreach (var conn in _commandMessageServer._connections)
            {
                if (conn.Name == _clientsDict[currentExecutorId])
                {
                    conn.PushMessage(new CloseExecutorMessage());
                    break;
                }
            }
        }

        /// <summary>
        /// 清空日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearLogButton_Click(object sender, RoutedEventArgs e)
        {
            LogTextBox.Text = "";
        }
    }
}
