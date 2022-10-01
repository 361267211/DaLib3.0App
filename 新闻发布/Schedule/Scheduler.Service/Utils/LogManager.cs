using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Scheduler.Service.Utils
{
    public class LogManager
    {
        private static readonly Regex logFile = new Regex(@"^(?<name>.*)_(?<date>.*)\.txt$", RegexOptions.Compiled);
        private static readonly string LOG_PATH = null;
        private static Dictionary<string, object> locker = new Dictionary<string, object>();

        static LogManager()
        {
            LOG_PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\logs";
            if (!Directory.Exists(LOG_PATH)) Directory.CreateDirectory(LOG_PATH);
        }

        public static void Error(string message, string serviceName = "TaskManager")
        {
            string dir = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, string.Format("logs\\{0}\\{1}\\",
                  DateTime.Now.ToString("yyyyMM"), DateTime.Now.ToString("dd")));
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string file = string.Format("{0}_{1:yyyy-MM-dd}.txt", serviceName, DateTime.Today);

            string content = string.Format("{0:yyyy-MM-dd HH:mm:ss} {2}: {1}\r\n", DateTime.Now, message, "Error");

            string path = dir + "\\" + file;

            lock (GetLocker(serviceName))
            {
                File.AppendAllText(path, content, Encoding.UTF8);
            }
        }

        public static void Info(string message, string serviceName = "TaskManager")
        {
            string dir = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, string.Format("logs\\{0}\\{1}\\",
                 DateTime.Now.ToString("yyyyMM"), DateTime.Now.ToString("dd")));
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string file = string.Format("{0}_{1:yyyy-MM-dd}.txt", serviceName, DateTime.Today);

            string content = string.Format("{0:yyyy-MM-dd HH:mm:ss} {2}: {1}\r\n", DateTime.Now, message, "Info");

            string path = dir + "\\" + file;

            lock (GetLocker(serviceName))
            {
                File.AppendAllText(path, content, Encoding.UTF8);
            }
        }
        public static void Warning(string message, string serviceName = "TaskManager")
        {
            string dir = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, string.Format("logs\\{0}\\{1}\\",
                DateTime.Now.ToString("yyyyMM"), DateTime.Now.ToString("dd")));
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string file = string.Format("{0}_{1:yyyy-MM-dd}.txt", serviceName, DateTime.Today);

            string content = string.Format("{0:yyyy-MM-dd HH:mm:ss} {2}: {1}\r\n", DateTime.Now, message, "Warning");

            string path = dir + "\\" + file;

            lock (GetLocker(serviceName))
            {
                File.AppendAllText(path, content, Encoding.UTF8);
            }
        }
        public static void Debug(string message, string serviceName = "TaskManager")
        {
            string dir = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, string.Format("logs\\{0}\\{1}\\",
             DateTime.Now.ToString("yyyyMM"), DateTime.Now.ToString("dd")));
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string file = string.Format("{0}_{1:yyyy-MM-dd}.txt", serviceName, DateTime.Today);

            string content = string.Format("{0:yyyy-MM-dd HH:mm:ss} {2}: {1}\r\n", DateTime.Now, message, "Debug");

            string path = dir + "\\" + file;

            lock (GetLocker(serviceName))
            {
                File.AppendAllText(path, content, Encoding.UTF8);
            }
        }

        /// <summary>
        /// 写日志到指定文件
        /// </summary>
        /// <param name="name">文件名部分</param>
        /// <param name="message"></param>
        public static void Log(string name, string message)
        {
            string dir = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, string.Format("logs\\{0}\\{1}\\",
                  DateTime.Now.ToString("yyyyMM"), DateTime.Now.ToString("dd")));

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string file = string.Format("{0}_{1:yyyy-MM-dd}.txt", name, DateTime.Today);
            string path = dir + "\\" + file;

            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }

            string content = string.Format("{0:yyyy-MM-dd HH:mm:ss}: {1}\r\n", DateTime.Now, message);

            lock (GetLocker(file))
            {
                File.AppendAllText(path, content, Encoding.UTF8);
            }

        }



        public static string[] GetLogList(string serviceName)
        {
            List<string> logs = new List<string>();
            string[] files = Directory.GetFiles(LOG_PATH);
            foreach (string file in files)
            {
                Match match = logFile.Match(Path.GetFileName(file));
                if (match.Success)
                {
                    string name = match.Groups["name"].Value;
                    string date = match.Groups["date"].Value;
                    if (name == serviceName) logs.Add(date);
                }
            }
            return logs.ToArray();
        }



        private static object GetLocker(string name)
        {
            if (locker.ContainsKey(name))
            {
                return locker[name];
            }
            lock (locker)
            {
                if (locker.ContainsKey(name))
                {
                    return locker[name];
                }
                locker[name] = new object();
            }
            return locker[name];
        }
    }
}
