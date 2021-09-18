using System;
using System.Collections.Generic;
using System.IO;
using Common.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Serilog.Configuration;
using Serilog.AspNetCore;
using Serilog.Extensions;
using Serilog.Core;
    

//Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>()}).UseSerilog();

namespace FDEV.Rules.Demo.Core.Logging
{
    /// <summary>
    /// Static access to log objects
    /// </summary>
    public class LogManager : Common.Logging.LogManager, ILogManager
    {
        public static ILogManager Instance => _logManager ??= new LogManager(); 
        private static ILogManager _logManager;

        static LogManager()
        {
            _logManager = new LogManager();
        }

        public static Dictionary<string, Logger> CurrentLoggers;

       // public static ILog GetLog = SeriLog.AspNetCore...GetLogger<ILog>();

        /// <inheritdoc />
        //public ILog GetLogger<T>() => Manager.GetLogger(typeof(T));

        /// <inheritdoc />
        //public ILog GetLogger(Type type) => Manager.GetLogger(type);

        /// <inheritdoc />
       // public ILog GetLogger(string name) => Manager.GetLogger(name);
    }
}