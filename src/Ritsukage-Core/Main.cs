using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using RUCore.Common.Logging;
using System.Diagnostics;
using System.Text;

Console.Title = "Ritsukage Core";
DateTime LaunchTime = DateTime.Now;
Logger mainLogger = CoreLogger.GetLogger("Main");

AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
{
    var now = DateTime.Now;
    var sb = new StringBuilder()
        .AppendLine("！！！程序发生未捕获异常，程序即将崩溃！！！")
        .AppendLine($"启动于 {LaunchTime:yyyy-MM-dd HH:mm:ss.ffff}")
        .AppendLine($"崩溃于 {now:yyyy-MM-dd HH:mm:ss.ffff}")
        .Append($"工作时长 {now - LaunchTime}");
    mainLogger.Fatal(args.ExceptionObject as Exception, sb.ToString());
};

var currentProcess = Process.GetCurrentProcess();
_ = new Mutex(true, currentProcess.ProcessName, out var isFirst);
if (isFirst)
{
    mainLogger.Info("程序启动");
    await BeginService();
    mainLogger.Info("程序结束");
    CoreLogger.Flush();
}
else
{
    Console.WriteLine("请勿重复启动程序实例，以免发生异常");
}
if (!Console.IsInputRedirected)
{
    Console.WriteLine("程序主逻辑已结束，按任意键结束程序");
    Console.ReadKey(true);
}
else
{
    Console.WriteLine("程序主逻辑已结束");
}

static async Task BeginService()
{
    var hb = Host.CreateDefaultBuilder()
        .ConfigureLogging(builder =>
        {
            //Remove default logging
            builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.None);
            builder.AddNLog();
        })
        .ConfigureServices((context, service) =>
        {
            //TODO: Add services here
        });
    await hb.RunConsoleAsync();
}