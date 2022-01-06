using AutofacIoCSample.Interfaces;
using AutofacIoCSample.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutofacIoCSample.Services
{
    public class UserDefineMessageBoxService : IMessageBoxService
    {
        public void Show(string msg)
        {
            var window = new UserDefineMessageBoxWindow();
            window.Owner = Application.Current.MainWindow;//设置主窗体为拥有者
            window.SetMsg(msg);

            window.ShowDialog();
        }
    }
}
