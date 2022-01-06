using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagesLibrary
{
    public static class MessagePipeUtil
    {
        /// <summary>
        /// 根据唯一标识来构造管道名称
        /// </summary>
        /// <param name="id">唯一标识，区分多个服务器时用</param>
        /// <returns></returns>
        public static string GetServerPipeNameById(string id)
        {
            return "IPCNamedPipeServer#" + id;
        }
    }
}
