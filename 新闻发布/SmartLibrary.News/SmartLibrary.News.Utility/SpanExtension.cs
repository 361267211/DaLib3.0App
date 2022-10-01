/*********************************************************
 * 名    称：Span
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/5/8 15:20:49
 * 描    述：更高效的字符串替换帮助类。
 *
 * 更新历史：
 *
 * *******************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace SmartLibrary.News.Utility
{
    /// <summary>
    /// 字符串span快速替换的拓展方法
    /// </summary>
    public static class SpanExtension
    {
        /// <summary>
        /// 使用Span进行非空判断
        /// </summary>
        /// <param name="str">资源字符串</param>
        /// <returns></returns>
        public static bool AsSpanIsEmpty(this string str)
        {
            ReadOnlySpan<char> span = str.AsSpan();
            return span.IsEmpty;
        }

        /// <summary>
        /// 使用Span进行字符串替换
        /// </summary>
        /// <param name="str">资源字符串</param>
        /// <param name="splitStr">被替换的字符串</param>
        /// <param name="replaceStr">替换后的字符串</param>
        /// <returns></returns>
        public static string AsSpanReplace(this string str, string splitStr, string replaceStr)
        {
            var strSpan = str.AsSpan();
            var splitSapn = splitStr.AsSpan();
            int m = 0, n = 0;
            List<string> arr = new List<string>();

            while (true)
            {
                m = n;
                n = strSpan.IndexOf(splitSapn);
                if (n > -1)
                {
                    arr.Add(strSpan.Slice(0, n).ToString());
                    strSpan = strSpan.Slice(n + splitSapn.Length);
                }
                else
                {
                    break;
                }
            }
            arr.Add(strSpan.ToString());
            var strRes = string.Join(replaceStr, arr);
            arr.Clear();
            return strRes;
        }
    }
}
