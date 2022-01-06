using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineParserSample
{
    class Options
    {
        [Option('a', "add", Required = true, HelpText = "Outputs the sum result of the sequence of integers.")]
        public IEnumerable<int> IntSeq { get; set; }

        [Option('m', "msg", HelpText = "Display result in a message box.")]
        public bool IsShowMsgBox { get; set; }

        [Option('l', "log", HelpText = "Output result to file.")]
        public string Log { get; set; }
    }
}
