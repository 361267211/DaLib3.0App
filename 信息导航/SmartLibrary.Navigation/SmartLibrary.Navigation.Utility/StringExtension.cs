using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SmartLibrary.Navigation.Utility
{
    /// <summary>
    /// 字符串扩展方法
    /// </summary>
    public static class StringExtension
    {
        public static bool IsEmpty(this string value)
        {
            return ((value == null) || (value.Length == 0));
        }

        public static bool IsEmptyOrWhiteSpace(this string value)
        {
            return (value.IsEmpty() || value.All(t => char.IsWhiteSpace(t)));
        }

        public static bool IsNotEmptyOrWhiteSpace(this string value)
        {
            return (value.IsEmptyOrWhiteSpace() == false);
        }

        public static string UrlEncode(this string showStringToUrl)
        {
            if (showStringToUrl.IsEmptyOrWhiteSpace()) return showStringToUrl;
            return HttpUtility.UrlEncode(showStringToUrl);
        }

        public static string UrlDecode(this string showStringToUrl)
        {
            if (showStringToUrl.IsEmptyOrWhiteSpace()) return showStringToUrl;
            return HttpUtility.UrlDecode(showStringToUrl);
        }

        /// <summary>
        /// 字符串转datetime
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string source)
        {
            var isOK = DateTime.TryParse(source, out DateTime result);
            return isOK ? result : DateTime.Now;
        }

        /// <summary>
        /// 字符串转DateTimeOffset
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static DateTimeOffset ToDateTimeOffset(this string val)
        {
            var isOK = DateTimeOffset.TryParse(val, out var result);
            return isOK ? result : DateTimeOffset.MinValue;
        }

        /// <summary>
        /// 字符串转int
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int ToInt(this string source)
        {
            int.TryParse(source, out int result);
            return result;
        }
    }
}
