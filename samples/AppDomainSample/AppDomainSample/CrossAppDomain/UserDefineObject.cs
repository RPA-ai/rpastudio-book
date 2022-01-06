using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDomainSample.CrossAppDomain
{
    /// <summary>
    /// 自定义的类型，必须继承自MarshalByRefObject才能跨域交互
    /// </summary>
    public class UserDefineObject : MarshalByRefObject
    {
        public string Name { get; set; }

        public int Age { get; set; }
    }
}
