using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serilog.Events;

namespace FDEV.Rules.Demo.Core.Conversion
{
    public static class Logging
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
                // ReSharper disable once RedundantCaseLabel
                _ => LogEventLevel.Verbose,
            };
        }
   
    }
}
