using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommandLineParserSample
{
    class Program
    {
        static void Main(string[] args)
        {
            //解析命令行参数
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptions);
        }

        private static void RunOptions(Options option)
        {
            var sum = option.IntSeq.Sum();//整数序列求和

            //如果需要弹窗显示则弹窗，否则输出到控制台
            if(option.IsShowMsgBox)
            {
                MessageBox.Show(sum.ToString());
            }
            else
            {
                Console.WriteLine(sum);
            }

            //需要写到日志文件时，写求和结果到日志文件
            if(!string.IsNullOrEmpty(option.Log))
            {
                System.IO.File.WriteAllText(option.Log,sum.ToString());
            }
        }

    }
}
