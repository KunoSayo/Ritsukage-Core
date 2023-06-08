using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;

namespace RUCore.Common.Logging
{
    /// <summary>
    /// NLog logger
    /// </summary>
    public static class CoreLogger
    {
        private static readonly ISetupBuilder Builder;

        static CoreLogger()
        {
            var config = new ConfigurationBuilder().Build();
            Builder = LogManager.Setup().SetupExtensions(ext => ext.RegisterConfigSettings(config));
        }

        /// <summary>
        /// Gets the specified named logger.
        /// </summary>
        /// <param name="name">name of Logger</param>
        /// <returns>Specified named logger</returns>
        public static Logger GetLogger(string name)
        {
            return Builder.GetLogger(name);
        }

        /// <summary>
        /// Flush any pending log messages (in case of asynchronous targets) with the default timeout of 15 seconds.
        /// </summary>
        public static void Flush()
        {
            LogManager.Flush();
        }
    }
}
