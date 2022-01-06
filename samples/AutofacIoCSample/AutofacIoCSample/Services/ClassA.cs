using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutofacIoCSample.Services
{
    public class ClassA
    {
        private ClassB _b;

        public ClassA(ClassB b)
        {
            _b = b;
        }

        public void Show()
        {
            MessageBox.Show("ClassA");
        }

        public void ShowB()
        {
            _b.Show();
        }
    }
}
