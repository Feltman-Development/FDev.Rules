using System;
using System.Runtime.CompilerServices;
using Common.Logging;
using FDEV.Rules.Demo.Core.Logging.CONSOLIDATE_CONFIG;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.AspNetCore;
using Serilog.Configuration;

namespace FDEV.Rules.Demo.Core.Logging
{
    /// <summary>
    /// Logger using the default(root) log repository. Has its own ConfigureLogger class (access thru "Logger.Configure...") to ease configuration by code.
    /// References to log framework (log4net for now) isn't needed in projects using this Logger class - it is totally stand alone!
    /// If your project uses only one logger, this is (probably) the simplest way to get logging :-)
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// Access the singleton instance of Logger to make a log entry. Caller information is included in all entries automatically.
        /// </summary>
        //public static Logger<TLogger> Entry<TLogger>() where TLogger : class => _logger ??= new Logger<TLogger>(new LoggerFactory());
        //private static Logger<TLogger> _logger;

        //private static ILoggerFactory => new Serilog.Core.loggerf
        //Private to use class as singleton
        private Logger() { }

        private void Initialize() => _log = Common.Logging.LogManager.GetLogger(typeof(Logger));
        private ILog _log;

        #region Entries

        public void Info(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (_log == null) Initialize();
            _log.Info($"{message} | Method: {memberName}() > file: {sourceFilePath} > line: {sourceLineNumber}");
        }

        public void Error(string message, Exception ex = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (_log == null) Initialize();
            _log.Error($"{message} | Method: {memberName}() > file: {sourceFilePath} > line: {sourceLineNumber}", ex);
        }

        public void Warning(string message, Exception ex = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (_log == null) Initialize();
            _log.Warn($"{message} | Method: {memberName}() > file: {sourceFilePath} at line: {sourceLineNumber}", ex);
        }

        public void Debug(string message, Exception ex = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (_log == null) Initialize();
            _log.Debug($"{message} | Method: {memberName}() > file: {sourceFilePath} > line: {sourceLineNumber}", ex);
        }

        public void Exception(string message, Exception ex = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (_log == null) Initialize();
            _log.Fatal($"{message} | Method: {memberName}() > file: {sourceFilePath} > line: {sourceLineNumber}", ex);
        }

        #endregion Entries

        #region Configuration

        //public static ConfigureLogger Configure => SeriLog..ConfigureLogger.Configure;

        /// <summary>
        /// Predefined layouts to easy configuration of the Logger instance
        /// </summary>
        public static class Layouts
        {
            public static string Simple => @"TIME: %date{dd-MM-yyyy HH:mm:ss} | LEVEL: %level | MESSAGE: %message | %exception %newline%newline";
            public static string SimpleSplunk => @"TIME: %date{dd-MM-yyyy HH:mm:ss} | LEVEL: %level | MESSAGE: %message | %exception %newline%newline";
        }

        #endregion Configuration
    }
}