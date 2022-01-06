using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLogSample
{
    class Program
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            _logger.Debug("NLogSample程序启动");

            _logger.Trace("这是一条TRACE消息");
            _logger.Debug("这是一条DEBUG消息");
            _logger.Info("这是一条INFO消息");
            _logger.Warn("这是一条WARN消息");
            _logger.Error("这是一条ERROR消息");
            _logger.Fatal("这是一条FATAL消息");

            _logger.Log(LogLevel.Info, "另一种输出日志消息的方法演示");

            try
            {
                throw new Exception("这是一条异常消息");
            }
            catch (Exception err)
            {
                //注意：日志若要记录详细的错误信息需要将PDB文件放到程序目录
                _logger.Error(err,"触发异常");
                _logger.Warn("忽略异常，程序将继续往下运行！");
            }

            _logger.Debug("NLogSample程序结束");

            _logger.Info("请按任意键退出");

            LogManager.Shutdown();
            System.Console.ReadKey();
        }
    }
}
