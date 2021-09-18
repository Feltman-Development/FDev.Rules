namespace FDEV.Rules.Demo.Core.Logging.CONSOLIDATE_CONFIG
{
    /// <summary>
    /// Helper class to easy and statically configure the Logger from code
    /// </summary>
    public class ConfigureLogger
    {
        /// <summary>
        /// Access the single instance of ConfigureLogger to configure the default (root) log repository
        /// </summary>
        internal static ConfigureLogger Configure => _logger ?? (_logger = new ConfigureLogger());

        private static ConfigureLogger _logger;

        private ConfigureLogger() {}

        public void RemoveAllAppenders()
        {
        }
        
        public void SetRootLevel(string rootLevelThreshold = "Debug")
        {
            //hierarchy.Root.Level = hierarchy.LevelMap[rootLevelThreshold];
            //hierarchy.Configured = true;
        }
        
        public void AddFileAppender(string name, string filePath, string layout, string levelName = "Debug")
        {
            //var patternLayout = new PatternLayout(layout);
            //patternLayout.ActivateOptions();

            //var hierarchy = (Hierarchy) log4net.LogManager.GetRepository();
            //var threshold = hierarchy.LevelMap[levelName];

            //var appender = new FileAppender() {Name = name, File = filePath, Layout = patternLayout, Threshold = threshold};
            //ActivateAppender(appender, hierarchy);
        }

        public void AddConsoleAppender(string name, string layout, string levelName = "Verbose")
        {
            //var patternLayout = new PatternLayout(layout);
            //patternLayout.ActivateOptions();

            //var hierarchy = (Hierarchy) log4net.LogManager.GetRepository();
            //var threshold = hierarchy.LevelMap[levelName];

            //var appender = new ConsoleAppender() {Layout = patternLayout, Threshold = threshold};
            //ActivateAppender(appender, hierarchy);
        }

        //private void ActivateAppender(AppenderSkeleton appender, Hierarchy hierarchy)
        //{
        //    appender.ActivateOptions();
        //    hierarchy.Root.AddAppender(appender);
        //    hierarchy.Configured = true;
        //}

    }
}