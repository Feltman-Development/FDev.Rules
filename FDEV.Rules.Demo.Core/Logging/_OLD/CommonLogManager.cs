using System;
using Common.Logging;

namespace FDEV.Rules.Demo.Core.Logging
{
    /// <summary>
    /// Simple static access to log objects, using any underlying framework you want. This is the one of the resons for the simplicity of the manager,
    /// for it to be agnostic and to offer the same functionality even though it may not be implemented the same way across frameworks. 
    /// Thus allowing you to replace or change implementation with very little trouble.
    /// <seealso cref="InstantLog"/> for very easy usage of the log/>
    /// </summary>
    public class CommonLogManager : LogManager, ILogManager
    {

        protected CommonLogManager()
        {
            //
        }

        /// <summary>
        /// Get the current log manager (or type with manager functions), no matter the underlying logging framework
        /// </summary>
        public static ILogManager Manager => _manager ?? (_manager = (ILogManager)new LogManager());
        private static ILogManager _manager;

        /// <inheritdoc />
        protected int _maxUsedLogFileEnumeration;

        private static string _logFolder => DevData.LogData.DefaultDevLogFolder;

        private static LogInfo _currentLogFileInfo => LogInfo.GetCurrentLog(_manager.GetType(), _logFolder);
    }
}