using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log4netSample
{
    class Program
    {
        private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            _logger.Debug("Log4netSample程序启动");

            _logger.Debug("这是一条DEBUG消息");
            _logger.Info("这是一条INFO消息");
            _logger.Warn("这是一条WARN消息");
            _logger.Error("这是一条ERROR消息");
            _logger.Fatal("这是一条FATAL消息");

            try
            {
                int a = 10;
                int b = 0;
                int c = a / b;
            }
            catch (Exception err)
            {
                //注意：日志若要记录详细的错误信息需要将PDB文件放到程序目录
                _logger.Error("触发异常",err);
                _logger.Warn("忽略异常，程序将继续往下运行！");
            }

            _logger.Debug("Log4netSample程序结束");

            _logger.Info("请按任意键退出");

            System.Console.ReadKey();
        }
    }
}
