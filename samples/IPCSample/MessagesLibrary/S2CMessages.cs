using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MessagesLibrary
{
    [Serializable]
    public class GetExecutorInfoMessage : ICommandMessage
    {
        public string cmd { get; set; } = "get_executor_info";
    }

    [Serializable]
    public class CloseExecutorMessage : ICommandMessage
    {
        public string cmd { get; set; } = "close_executor";
    }
}
