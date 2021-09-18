using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace FDEV.Rules.Demo.Core.Logging.SeriLog
{
    public class CustomLogger : ILogger
    {
        protected CustomLogger()
        {
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


        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            //GetLogger(typeof(TState).Name).Log(logLevel, eventId, state, exception, formatter);
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public IDisposable BeginScope<TState>(TState state) => default;
        
        public void Dispose()
        {
        }
    }   

    public class CustomProvider : ILoggerProvider
    {
        protected CustomProvider()
        {
            LoadedLoggers = new Dictionary<string, ILogger>();
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

        public Serilog.Core.Logger CurrentLogger { get; set;}

        public static Dictionary<string, ILogger> LoadedLoggers { get; private set; }

        public ILogger GetLogger(string categoryName)
        {
            if (LoadedLoggers.ContainsKey(categoryName)) return LoadedLoggers[categoryName];

            var logger = CreateLogger(categoryName);
            LoadedLoggers.Add(categoryName, logger);

            return logger;
        }

        public ILogger CreateLogger(string categoryName)
        {
            var logger = LoadedLoggers[categoryName];
            if (logger == null) 
            {
                logger = LoadedLoggers[categoryName];
            
            }

            return null;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            GetLogger(typeof(TState).Name).Log(logLevel, eventId, state, exception, formatter);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return CurrentLogger.IsEnabled(Conversion.Logging.MicrosoftToSerilogLevel(logLevel));
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }
        
        public void Dispose()
        {
            LoadedLoggers.Clear();
            CurrentLogger = null;
        }
    }   
}