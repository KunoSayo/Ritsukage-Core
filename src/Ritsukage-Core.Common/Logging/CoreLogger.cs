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
        static readonly ISetupBuilder Builder;
        static CoreLogger()
        {
            var config = new ConfigurationBuilder().Build();
            Builder = LogManager.Setup().SetupExtensions(ext => ext.RegisterConfigSettings(config));
        }

        /// <summary>
        /// Gets the specified named logger.
        /// </summary>
        /// <param name="name">name of Logger</param>
        /// <returns></returns>
        public static Logger GetLogger(string name)
            => Builder.GetLogger(name);
    }
}
