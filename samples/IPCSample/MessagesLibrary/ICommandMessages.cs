using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagesLibrary
{
    /// <summary>
    /// 服务端往客户端发送的命令消息
    /// </summary>
    public interface ICommandMessage
    {
        /// <summary>
        /// 命令标识
        /// </summary>
        string cmd { get; set; }
    }

}
