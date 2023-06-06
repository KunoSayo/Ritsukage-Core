using NLog;
using RUCore.Common.Logging;
using System.Diagnostics;
using System.Text;

Console.Title = "Ritsukage Core";
DateTime LaunchTime = DateTime.Now;
Logger logger = CoreLogger.GetLogger("Main");

AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
{
    var now = DateTime.Now;
    var sb = new StringBuilder()
        .AppendLine("！！！程序发生未捕获异常，程序即将崩溃！！！")
        .AppendLine($"启动于 {LaunchTime:yyyy-MM-dd HH:mm:ss.ffff}")
        .AppendLine($"崩溃于 {now:yyyy-MM-dd HH:mm:ss.ffff}")
        .Append($"工作时长 {now - LaunchTime}");
    logger.Fatal(args.ExceptionObject as Exception, sb.ToString());
};

var currentProcess = Process.GetCurrentProcess();
_ = new Mutex(true, currentProcess.ProcessName, out var isFirst);
if (isFirst)
{
    logger.Info("程序启动");
    await Task.CompletedTask; // TODO: repalce for program logic
    logger.Info("程序结束");
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