using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MessagesLibrary
{
    [Serializable]
    public class RegisterMessage : ICommandMessage
    {
        public string cmd { get; set; } = "register";

        public string id { get; set; }
    }

    [Serializable]
    public class ExecutorInfoMessage : ICommandMessage
    {
        public string cmd { get; set; } = "executor_info";

        public string id { get; set; }

        public string msg { get; set; }
    }

}
