using System;
using log4net;
using log4net.Config;

namespace GHI.Commons.Logging
{
    public class ConsoleLogger : ILogEvents
    {
        private ILog _log;

        public ConsoleLogger()
        {
            BasicConfigurator.Configure();
        }

        public void Log(string message, LogLevel level, Type type)
        {
            _log = LogManager.GetLogger(type);
            switch (level)
            {
                case LogLevel.Info:
                    _log.Info(message);
                    break;
                case LogLevel.Debug:
                    _log.Debug(message);
                    break;
                case LogLevel.Warn:
                    _log.Warn(message);
                    break;
                case LogLevel.Fatal:
                    _log.Fatal(message);
                    break;
                case LogLevel.Error:
                    _log.Error(message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("level");
            }
        }
    }
}
