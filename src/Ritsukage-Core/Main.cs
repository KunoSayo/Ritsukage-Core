using CacheTower.Serializers.SystemTextJson;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using RUCore.Common.Logging;
using RUCore.Config;
using System.Diagnostics;
using System.Text;

Console.Title = "Ritsukage Core";
DateTime launchTime = DateTime.Now;
Logger mainLogger = CoreLogger.GetLogger("Main");

AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
{
    var now = DateTime.Now;
    var sb = new StringBuilder()
            .AppendLine("！！！程序发生未捕获异常，程序即将崩溃！！！")
            .AppendLine($"启动于 {launchTime:yyyy-MM-dd HH:mm:ss.ffff}")
            .AppendLine($"崩溃于 {now:yyyy-MM-dd HH:mm:ss.ffff}")
            .Append($"工作时长 {now - launchTime}");
    mainLogger.Fatal(args.ExceptionObject as Exception, sb.ToString());
};

var currentProcess = Process.GetCurrentProcess();
_ = new Mutex(true, currentProcess.ProcessName, out var isFirst);
if (isFirst)
{
    mainLogger.Info("程序启动");
    await BeginService().ConfigureAwait(false);
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

Task BeginService()
{
    var hb = Host.CreateDefaultBuilder()
                  //Configure logging
                 .ConfigureLogging(builder =>
                  {
                      //Disable default logging
                      builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.None);
                      //Use NLog as logging provider
                      builder.AddNLog();
                  })
                 .ConfigureServices(service =>
                  {
                      //Load config
                      Config? config = null;
                      if (!File.Exists("config.toml"))
                      {
                          config = new Config();
                          config.Cache.Add(Config.CacheLayerConfigTable.MemoryLayerConfigTable());
                          config.Cache.Add(
                              Config.CacheLayerConfigTable.FileLayerConfigTable("cache", TimeSpan.FromMinutes(30)));
                          config.Database = Config.DatabaseTargetConfigTable.SqliteDatabaseTargetConfigTable("data.db");
                          config.Save("config.toml");
                      }
                      else
                      {
                          config = Config.Load("config.toml");
                      }

                      //Configure database service
                      switch (config.Database?.Type ?? string.Empty)
                      {
                          case "sqlite":
                              service.AddDbContext<DbContext>(builder =>
                              {
                                  builder.UseSqlite(
                                      $"Data Source={config.Database?.Path ?? "data.db"}");
                              });
                              break;
                          case "sqlserver":
                              service.AddDbContext<DbContext>(builder =>
                              {
                                  builder.UseSqlServer(config.Database?.ConnectString);
                              });
                              break;
                          default:
                              service.AddDbContext<DbContext>(builder => { builder.UseSqlite("Data Source=data.db"); });
                              break;
                      }

                      //Configure cache service
                      if (config.Cache.Any())
                      {
                          service.AddCacheStack(builder =>
                          {
                              //Add cache layers which you want to use
                              foreach (var layer in config.Cache)
                              {
                                  switch (layer.Type)
                                  {
                                      case "memory":
                                          builder.AddMemoryCacheLayer();
                                          break;
                                      case "file":
                                          builder.AddFileCacheLayer(new(layer.Path ?? "cache",
                                                                        new SystemTextJsonCacheSerializer(new()),
                                                                        TimeSpan.FromSeconds(layer.ExpireTime ??
                                                                            TimeSpan.FromMinutes(30).TotalSeconds)));
                                          break;
                                  }
                              }

                              builder.WithCleanupFrequency(config.CleanupFrequency ?? TimeSpan.FromMinutes(5));
                          });
                      }
                      else
                      {
                          service.AddCacheStack(builder =>
                          {
                              builder.AddMemoryCacheLayer()
                                     .WithCleanupFrequency(config.CleanupFrequency ?? TimeSpan.FromMinutes(5));
                          });
                      }
                      //TODO: Add other services here
                  });
    return hb.RunConsoleAsync();
}
