using System;
using Common.Logging;

namespace FDEV.Rules.Demo.Core.Logging.CONSOLIDATE_CONFIG
{
    public static class Log
    {
        private static  ILog log = null;// log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static Boolean showConsole = true;

        public static void Initialize(Type type, string LogFileName = "")
        {
            // setting GlobalContext must be done before GetLogger
            //if (LogFileName != "")
            //{
            //    GlobalContext.Properties["LogFileName"] = LogFileName;
            //}
            //else
            //{
            //    // Setting the Log4Net filename to the name of the project that has started the logging 
            //    GlobalContext.Properties["LogFileName"] = Assembly.GetEntryAssembly().GetName().Name + "_Log.txt";
            //}

            //log = log4net.LogManager.GetLogger(type);
        }



        public static void Info(string info)
        {
            log.Info(info);
            if (showConsole)
            {
                Console.WriteLine(info);
            }
        }

        public static void Info(string info, Exception ex)
        {
            log.Info(info, ex);
            if (showConsole)
            {
                Console.WriteLine(info);
            }
        }

        public static void Info<T>(string info, T dto, Exception ex = null)
        {
            if (ex == null)
            {
                Info(info);
            }
            else
            {
                Info(info, ex);
            }
        }

        public static void Error(string error)
        {
            log.Error(error);
            if (showConsole)
            {
                Console.WriteLine(error);
            }
        }

        public static void Error(string error, Exception ex)
        {
            log.Error(error, ex);
            if (showConsole)
            {
                Console.WriteLine(error);
            }
        }

        public static void Error<T>(string error, T dto, Exception ex = null)
        {
            if (ex == null)
            {
                Error(error);
            }
            else
            {
                Error(error, ex);
            }
        }

        public static void Debug(string debug)
        {
            log.Error(debug);
            if (showConsole)
            {
                Console.WriteLine(debug);
            }
        }

        public static void Debug(string debug, Exception ex)
        {
            log.Debug(debug, ex);
        }

        public static void Debug<T>(string debug, T dto, Exception ex = null)
        {
            if (ex == null)
            {
                Debug(debug);
            }
            else
            {
                Debug(debug, ex);
            }
        }
    }
}
