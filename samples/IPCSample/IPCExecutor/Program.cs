using MessagesLibrary;
using NamedPipeWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IPCExecutor
{
    class Program
    {
        private static string _clientId = System.Guid.NewGuid().ToString();

        private static NamedPipeClient<ICommandMessage> _client;

        private static string _serverId;

        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("参数数量不对，程序自动退出！");
                Environment.Exit(-1);
            }

            _serverId = args[0];

            _client = new NamedPipeClient<ICommandMessage>(MessagePipeUtil.GetServerPipeNameById(_serverId));
            _client.Disconnected += _client_Disconnected;

            _client.ServerMessage += _client_ServerMessage;
            _client.Start();
            _client.WaitForConnection();

            _client.PushMessage(new RegisterMessage() { id = _clientId });
            Console.WriteLine("执行器启动，ID号为：" + _clientId);

            Console.ReadKey();
        }

        private static void _client_Disconnected(NamedPipeConnection<ICommandMessage, ICommandMessage> connection)
        {
            Console.WriteLine("连接断开，稍后自动退出");
            Thread.Sleep(2000);//适当延时，便于观察
            Environment.Exit(0);
        }

        private static void _client_ServerMessage(NamedPipeConnection<ICommandMessage, ICommandMessage> connection, ICommandMessage message)
        {
            if (message is GetExecutorInfoMessage)
            {
                Console.WriteLine("收到获取执行器信息请求");
                connection.PushMessage(new ExecutorInfoMessage() { id = _clientId, msg = $"我是ID为{_clientId}的执行器，通过我来演示IPC交互的过程" });
            }else if(message is CloseExecutorMessage)
            {
                Console.WriteLine("收到关闭执行器请求，稍后自动退出");
                Thread.Sleep(2000);//适当延时，便于观察
                Environment.Exit(0);
            }
        }
    }
}
