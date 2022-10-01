using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Tasks.Extension
{
    public static class StringExtensions
    {
        /// <summary>
        /// 判断字符串是否为空
        /// </summary>
        /// <param name="oriString"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string oriString)
        {
            return string.IsNullOrWhiteSpace(oriString);
        }
    }
}
