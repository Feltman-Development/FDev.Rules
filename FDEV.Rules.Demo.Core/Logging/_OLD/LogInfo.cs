using System;
using System.IO;
using System.Linq;
using FDEV.Rules.Demo.Core.Utility;

namespace FDEV.Rules.Demo.Core.Logging
{
    public class LogInfo
    {
        /// <summary>
        /// A LogInfo is llike a FileInfo, with  metadata ńeeded to control a library of logfiles 
        /// </summary>
        public static LogInfo CreateOnExisting(FileInfo fileInfo) => CreateOnExisting(fileInfo.FullName);

        /// <summary>
        /// Create a LogInfo on an existing file parsing the full file name and path
        /// </summary>
        public static LogInfo CreateOnExisting(string filePath) => new LogInfo(Type.GetType(GetName(filePath), false), GetPath(filePath), GetNumber(filePath), GetDate(filePath), GetExtension(filePath));

        /// <summary>
        /// Create a new LogInfo
        /// </summary>
        public static LogInfo CreateNew(Type type, string path, int fileNumber, string fileExtension = "log") => new LogInfo(type, path, fileNumber, fileExtension);

        /// <summary>
        /// Gets a LogInfo for the current log of the given type, that is: the latest logfile of the type
        /// </summary>
        public static LogInfo GetCurrentLog(Type type, string path)
        {
            var dir = new DirectoryInfo(path);
            var fileInfos = dir.GetFiles();
            var logInfos = fileInfos.Select(x => CreateOnExisting(x.FullName));
            var latestLogFileNumber = logInfos.Max(x => x.FileNumber);
            return logInfos.FirstOrDefault(x => x.FileNumber == latestLogFileNumber);
        }

        protected LogInfo(Type logType, string path, int fileNumber, string DateText = "", string fileExtension = "log")
        {
            CreatedAt = DateTime.UtcNow;

            LogType = logType;
            DirectoryName = path;
            FileName = LogType.Name;
            FileDateText = DateUtility.GetUIDate(CreatedAt);
            FileNumber = fileNumber;
            FileFileExtension = fileExtension;
        }

        public Type LogType { get; }
        public string Name { get; }
        public DateTime CreatedAt { get; }

        public string DirectoryName { get; }
        public string FileName { get; }
        public string FileDateText { get; }
        public int FileNumber { get; }
        public string FileFileExtension { get; }

        public string FullFileNameAndPath => Path.Combine(DirectoryName, DateUtility.GetUIDate(), "_", FileName, "_", FileNumber.ToString(), ".", FileFileExtension);

        #region Static Members

        public static string GetExtension(string fileName) => fileName.Substring(fileName.LastIndexOf(".", fileName.Length - fileName.LastIndexOf(".")));
        public static string GetPath(string fileName) => fileName.Substring(0, fileName.LastIndexOf("/"));
        public static string GetDate(string fileName) => fileName.Substring(fileName.LastIndexOf("/"), DateUtility.GetUIDate().Length);
        public static string GetName(string fileName) => fileName.Substring(fileName.IndexOf("_") + 1, fileName.LastIndexOf("_") - fileName.IndexOf("_"));
        public static int GetNumber(string fileName) => int.Parse(fileName.SubstringFromRight(0, 2));

        #endregion Static Members
    }
}