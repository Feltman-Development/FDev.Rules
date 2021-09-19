using Microsoft.Extensions.Logging;
using Serilog.Events;

namespace FDEV.Rules.Demo.Core.Conversion
{
    public static class CommonLogging
    {
        public static LogEventLevel MicrosoftToSerilogLevel(LogLevel logLevel)
        {
            return logLevel switch
            {
                // as there is no match for 'None' in Serilog, pick the least logging possible
                LogLevel.None or LogLevel.Critical => LogEventLevel.Fatal,
                LogLevel.Error => LogEventLevel.Error,
                LogLevel.Warning => LogEventLevel.Warning,
                LogLevel.Information => LogEventLevel.Information,
                LogLevel.Debug => LogEventLevel.Debug,
                _ => LogEventLevel.Verbose,
            };
        }

        public static LogLevel SerilogToMicrosoftLevel(LogEventLevel logEventLevel)
        {
            return logEventLevel switch
            {
                LogEventLevel.Fatal => LogLevel.Critical,
                LogEventLevel.Error => LogLevel.Error,
                LogEventLevel.Warning => LogLevel.Warning,
                LogEventLevel.Information => LogLevel.Information,
                LogEventLevel.Debug => LogLevel.Debug,
                LogEventLevel.Verbose => LogLevel.Trace,
                _ => LogLevel.None,
            };
        }
    }
}