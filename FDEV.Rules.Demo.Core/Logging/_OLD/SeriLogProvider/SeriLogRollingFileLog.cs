using System;
using System.Collections.Generic;
using System.IO;
using Common.Logging;
using Serilog;
using Serilog.Core;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace FDEV.Rules.Demo.Core.Logging.SeriLog
{
    public class SeriRollingFileLogger //: Logger//7, ILogger 
    {
        public static SeriRollingFileLogger InitialiseLogger => new SeriRollingFileLogger();

        public SeriRollingFileLogger()
        {
            //CurrentLoggers = new Dictionary<string, Logger>();
            ////this LoggerSettingsConfiguration settingConfiguration, IConfiguration configuration, string sectionName, DependencyContext dependencyContext = null
            //var logSettings = new LoggerSettingsConfiguration();
            //var configuration = new Configuration();
            //var sectionName = "MySeriLog";
            //var logConfig = ConfigurationLoggerConfigurationExtensions.Configuration(LoggerSettingsConfiguration.(), new Configuration(), new ConfigurationAssemblySource())
            //ReadFrom.Configuration(IConfiguration configuration, string sectionName, ConfigurationAssemblySource configurationAssemblySource) 
            //_manager = (ILogManager)new CommonSeriLogManager();
            // Structured logs can be captured as easily as specifying a formatter for console output:

            // Back in Program.Main():
            //.WriteTo.Console(new RenderedCompactJsonFormatter())
            //Or by adding a file sink with similar configuration:
            //.WriteTo.File(new RenderedCompactJsonFormatter(), "/logs/log.ndjson")
            //var logFile = Path.Combine(path, fileName);
            //Log.Logger = new LoggerConfiguration().WriteTo.File(logFile, rollingInterval: RollingInterval.Day).CreateLogger();

           // InitialiseRollingFileLogging(DevData.LogData.DefaultDevLogFolder, "TestCommonSeriLogManager.log", 10000);
        }

        protected static void InitialiseRollingFileLogging(string path, string fileName, int fileSize)
        {
           //var rollingFileLog = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File(Path.Combine(path, fileName), rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true, fileSizeLimitBytes: fileSize).CreateLogger();
           //CurrentLoggers.Add("rollingFileLog", rollingFileLog);
        }

        
    }
}