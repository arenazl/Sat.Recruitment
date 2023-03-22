using System;
using NLog;

namespace Sat.Recruitment.Infrastructure.Logging
{
    public static class LogUtility
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public static void Log(LogLevel level, string message)
        {
            if (LogManager.Configuration != null)
            {
                if (LogManager.Configuration.LoggingRules.Count > 0)
                {                 
                    _logger.Log(level, message);
                }
            }
        }

        public static void Trace(string message)
        {
            Log(LogLevel.Trace, message);
        }

        public static void Debug(string message)
        {
            Log(LogLevel.Debug, message);
        }

        public static void Info(string message)
        {
            Log(LogLevel.Info, message);
        }

        public static void Warn(string message)
        {
            Log(LogLevel.Warn, message);
        }

        public static void Error(string message)
        {
            Log(LogLevel.Error, message);
        }

        public static void Fatal(string message)
        {
            Log(LogLevel.Fatal, message);
        }
    }

}




