using System.Collections.Generic;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace CamDist
{
    public static class Utils
    {
        public static ILogger InitLogger()
        {
            var logfile = new FileTarget("logfile")
            {
                FileName = "log.txt",
                DeleteOldFileOnStartup = true,
                Layout = @"${level} ${longdate} ${message}"
            };

            var config = new LoggingConfiguration();
            config.AddTarget("file", logfile);
            var rule1 = new LoggingRule("*", LogLevel.Trace, logfile);
            config.LoggingRules.Add(rule1);

            LogManager.Configuration = config;

            return LogManager.GetLogger("CamDist");
        }

        public static string PrintDictionary<TKey, TValue>(IDictionary<TKey, TValue> dictionary)
        {
            return $"{string.Join(", ", dictionary)}";
        }
    }
}