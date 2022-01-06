using Autofac;
using AutofacIoCSample.Interfaces;
using AutofacIoCSample.Services;
using System;
using System.Collections.Generic;
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

namespace AutofacIoCSample
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private ILifetimeScope _singleInstancescope;

        public MainWindow()
        {
            InitializeComponent();
        }

        //弹窗测试
        private void MessageBoxTest_Click(object sender, RoutedEventArgs e)
        {
            var builder = new ContainerBuilder();

            if(SystemRadioButton.IsChecked == true)
            {
                builder.RegisterType<SystemMessageBoxService>().As<IMessageBoxService>();//注册系统弹窗服务
            }
            else
            {
                builder.RegisterType<UserDefineMessageBoxService>().As<IMessageBoxService>();//注册自定义弹窗服务
            }

            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var service = scope.Resolve<IMessageBoxService>();
                service.Show("这是一条弹窗提示内容");
            }
        }

        //循环依赖测试
        private void CircularDependenciesTest_Click(object sender, RoutedEventArgs e)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ClassA>();
            builder.RegisterType<ClassB>();

            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var a = scope.Resolve<ClassA>();
                a.Show();
                a.ShowB();

                var b = scope.Resolve<ClassB>();
                b.Show();
                b.ShowA();
            }
        }

        //单例模式测试
        private void SingleInstanceTest_Click(object sender, RoutedEventArgs e)
        {
            //为了单例模式起作用，生存期不能释放，需要保存到类成员变量中
            if(_singleInstancescope == null)
            {
                var builder = new ContainerBuilder();
                builder.RegisterType<SingleInstanceClass>().SingleInstance();//通过SingleInstance()函数来注册该类型为单例模式
                var container = builder.Build();
                _singleInstancescope = container.BeginLifetimeScope();
            }

            var singleObj = _singleInstancescope.Resolve<SingleInstanceClass>();

            singleObj.Show();//每次显示的值递增，说明单例模式起了作用
        }

       
    }
}
