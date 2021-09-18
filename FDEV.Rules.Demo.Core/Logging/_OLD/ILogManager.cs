namespace FDEV.Rules.Demo.Core.Logging
{
    using System;
    using System.IO;
    using Common.Logging;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Configuration;
    using Microsoft.Extensions.Logging.Abstractions;
    using Serilog.Configuration;
    using Serilog.AspNetCore;
    using Serilog.Extensions;
    using Serilog.Core;

    /// <summary>
    /// Simple interface for a log manager, enabling you to use and choose implementation as you go.
    /// This interface and its base implementation, covers the basic usage of Microsofts own logging, and offers a simple 'log4net', NLog and 'SeriLog' LogManager usage.
    /// Functionality is cut down to the basics: get a logger by type (defined by a generic, injected type or type name) - and my little "magic" extension, that by extending Type itself,
    /// serves you a logger with half a line of code, anywhere anytime - and it's typestrong! See <see cref="InstantLog"/>. It really is that simple!
    /// </summary>
    public interface ILogManager : Common.Logging.ILogManager
    {
        static ILogManager Current { get; }
        
        //In base
        //ILog GetLogger<T>();
        //ILog GetLogger(Type type);
        //ILog GetLogger(string key);
        
        
    }
}