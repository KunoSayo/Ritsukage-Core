﻿using Newtonsoft.Json;
using System;
using System.IO;

namespace Ritsukage
{
    class Config
    {
        const string ConfigPath = "config.json";

        /// <summary>
        /// 是否启用QQ相关功能
        /// </summary>
        public bool QQ = false;

        /// <summary>
        /// 要连接的目标IP
        /// </summary>
        public string Host = "[::]";

        /// <summary>
        /// 要监听的目标端口
        /// </summary>
        public uint Port = 23150;

        /// <summary>
        /// token
        /// </summary>
        public string AccessToken = "";

        /// <summary>
        /// 心跳超时间隔
        /// </summary>
        public uint HeartBeatTimeOut = 30000;

        /// <summary>
        /// 数据库储存位置
        /// </summary>
        public string DatabasePath = "data.db";

        /// <summary>
        /// QQ相关功能超级权限者
        /// </summary>
        public long QQSuperUser = -1;

        /// <summary>
        /// 是否启用Discord相关功能
        /// </summary>
        public bool Discord = false;

        /// <summary>
        /// Discord Bot Token
        /// </summary>
        public string DiscordToken = "";

        /// <summary>
        /// 是否使用调试模式
        /// </summary>
        public bool IsDebug = false;

        public static Config LoadConfig()
        {
            Config cfg = null;
            try
            {
                if (File.Exists(ConfigPath))
                    cfg = JsonConvert.DeserializeObject<Config>(File.ReadAllText(ConfigPath));
            }
            catch
            {
            }
            if (cfg == null)
            {
                cfg = new Config();
                File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(cfg, Formatting.Indented));
            }
            return cfg;
        }
    }
}
