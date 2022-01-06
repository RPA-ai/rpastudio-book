using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutofacIoCSample.Services
{
    public class ClassB
    {
        private Lazy<ClassA> _a;

        //用该构造函数解析会发生循环依赖
        //public ClassB(ClassA a)
        //{
        //    _a = a;
        //}

        //用该构造函数延迟解析不会触发循环依赖
        public ClassB(Lazy<ClassA> a)
        {
            _a = a;
        }

        public void ShowA()
        {
            _a.Value.Show();
        }

        public void Show()
        {
            MessageBox.Show("ClassB");
        }
    }
}
