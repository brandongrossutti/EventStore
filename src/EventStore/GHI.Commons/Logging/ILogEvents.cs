using System;

namespace GHI.Commons.Logging
{
    public interface ILogEvents
    {
        void Log(string message, LogLevel level, Type type);
    }
}