﻿using Newtonsoft.Json;
using Ritsukage.Discord;
using Ritsukage.Library.Data;
using Ritsukage.QQ;
using Sora.Tool;
using System;
using System.Threading;

namespace Ritsukage
{
    class Program
    {
        public static QQService QQServer { get; private set; }
        public static DiscordAPP DiscordServer { get; private set; }

        public static Config Config { get; private set; }

        public static bool Working = false;

#pragma warning disable IDE0060 // 删除未使用的参数
        static void Main(string[] args)
#pragma warning restore IDE0060 // 删除未使用的参数
        {
            Console.Title = "Ritsukage Core";
            ConsoleLog.Info("Main", "Loading...");
            Launch();
            while (Working)
            {
                Thread.Sleep(1000);
            }
            Shutdown();
            ConsoleLog.Info("Main", "程序主逻辑已结束，按任意键结束程序");
            Console.ReadKey();
        }

        static void Launch()
        {
            var cfg = Config = Config.LoadConfig();
#if DEBUG
            ConsoleLog.SetLogLevel(Fleck.LogLevel.Debug);
            ConsoleLog.Debug("Main", "当前正在使用Debug模式");
#else
            if (cfg.IsDebug)
            {
                ConsoleLog.SetLogLevel(Fleck.LogLevel.Debug);
                ConsoleLog.Debug("Main", "当前正在使用Debug模式");
            }
            else
                ConsoleLog.SetLogLevel(Fleck.LogLevel.Info);
#endif
            ConsoleLog.Debug("Main", "Config:\r\n" + JsonConvert.SerializeObject(cfg, Formatting.Indented));

            ConsoleLog.Info("Main", "初始化数据库中……");
            Database.Init(cfg.DatabasePath);
            ConsoleLog.Info("Main", "数据库已装载");

            if (cfg.Discord)
            {
                Working = true;
                ConsoleLog.Info("Main", "已启用Discord功能");
                new Thread(() =>
                {
                    try
                    {
                        DiscordServer = new(cfg.DiscordToken);
                        DiscordServer.Start();
                    }
                    catch (Exception ex)
                    {
                        ConsoleLog.Fatal("Main", "Discord功能启动失败");
                        ConsoleLog.ErrorLogBuilder(ex);
                        Working = false;
                    }
                })
                {
                    IsBackground = true
                }.Start();
            }

            if (cfg.QQ)
            {
                Working = true;
                ConsoleLog.Info("Main", "已启用QQ功能");
                new Thread(() =>
                {
                    try
                    {
                        QQServer = new(new()
                        {
                            Location = cfg.Host,
                            Port = cfg.Port,
                            AccessToken = cfg.AccessToken,
                            HeartBeatTimeOut = cfg.HeartBeatTimeOut
                        });
                        QQServer.Start();
                    }
                    catch (Exception ex)
                    {
                        ConsoleLog.Fatal("Main", "QQ功能启动失败");
                        ConsoleLog.ErrorLogBuilder(ex);
                        Working = false;
                    }
                })
                {
                    IsBackground = true
                }.Start();
            }
        }

        static void Shutdown()
        {
            try
            {
                QQServer?.Stop();
            }
            catch
            {
            }
            try
            {
                DiscordServer?.Stop();
            }
            catch
            {
            }
        }
    }
}