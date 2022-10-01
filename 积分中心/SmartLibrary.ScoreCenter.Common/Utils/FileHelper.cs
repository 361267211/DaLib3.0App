/*********************************************************
* 名    称：FileHelper.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：文件处理扩展
* 更新历史：
*
* *******************************************************/
using System;
using System.IO;
using System.Reflection;

namespace SmartLibrary.ScoreCenter.Common.Utils
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
            if (relativePath[0] == '/')
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
