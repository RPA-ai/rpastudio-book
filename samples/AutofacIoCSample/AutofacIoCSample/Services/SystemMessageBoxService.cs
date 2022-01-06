using AutofacIoCSample.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutofacIoCSample.Services
{
    public class SystemMessageBoxService : IMessageBoxService
    {
        public void Show(string msg)
        {
            MessageBox.Show(msg);
        }
    }
}
