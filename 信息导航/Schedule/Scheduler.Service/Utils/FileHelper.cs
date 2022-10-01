using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Service.Utils
{
    /// <summary>
    /// 文件处理类
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// 反射获取类信息
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="className">类名</param>
        /// <returns></returns>
        public static Type GetAbsolutePath(string assemblyName, string className)
        {
            Assembly assembly = Assembly.Load(new AssemblyName(assemblyName));
            Type type = assembly.GetType(className);
            return type;
        }
        /// <summary>
        /// 获取文件的绝对路径
        /// </summary>
        /// <param name="relativePath">相对路径地址</param>
        /// <returns>绝对路径地址</returns>
        public static string GetAbsolutePath(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
            {
                throw new ArgumentNullException("参数relativePath空异常！");
            }
            relativePath = relativePath.Replace("/", "\\");
            if (relativePath[0] == '\\')
            {
                relativePath = relativePath.Remove(0, 1);
            }

            return Path.Combine(AppContext.BaseDirectory, relativePath);
        }

        public static string ReadFileToText(string relativePath)
        {
            var path = GetAbsolutePath(relativePath);
            if (!File.Exists(path))
            {
                throw new Exception($"未找到文件:{path}");
            }
            var fileText = File.ReadAllText(path);
            return fileText;
        }

    }
}
