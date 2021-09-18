using Common.Logging;

namespace FDEV.Rules.Demo.Core.Logging
{
    /// <summary>
    /// Gets a Log instance of the calling type, as an extension method to the type.
    /// Use in class: this.LogByType().Info("Message");
    /// </summary>
    public static class InstantLog
    {
        /// <summary>
        /// IMPORTANT: Just type 'this.LogByType().[LogLevel]('Message') from wherever you are!
        /// Instantly and with no additional setup or configuration needed, you have a type strong logger at your disposal...
        /// And the implementation is almost as easy to change on this little clever (in my own words) extension,
        /// providing some almost "magical" access to a strongly typed logger instance! :-)
        /// </summary>
        /// <param name="logType"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>A logger instance of the type you are in, or the type you enter</returns>
        public static ILog LogByType<T>(this T logType) => ILogManager.Current.GetLogger<T>();

        /// <summary>
        /// Use to log in static classes. Since a static type can't be used as type arguments, a name for the log can be used
        /// with the same ease. So in a static class, use 'InstantLog.ByName(nameof([class])).[LogLevel].'
        /// </summary>
        /// <param name="logName"></param>
        /// <returns>A logger instance with the name given</returns>
        public static ILog LogByName(string logName) => ILogManager.Current.GetLogger(logName);
    }
}