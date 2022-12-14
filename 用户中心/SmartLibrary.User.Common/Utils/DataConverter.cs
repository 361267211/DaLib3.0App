/*********************************************************
* 名    称：DataConverter.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：数据转换工具
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Common.Utils
{
    /// <summary>
    /// 数据转换工具
    /// </summary>
    public class DataConverter
    {
        /// <summary>
        /// String转换为Decimal
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal? ToNullableDecimal(string str)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(str))
                {
                    return Convert.ToDecimal(str);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// String转换为Decimal
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal ToDecimal(string str)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(str))
                {
                    return Convert.ToDecimal(str);
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// String转换为Int
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal ToInt(string str)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(str))
                {
                    return Convert.ToInt32(str);
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// String类型转换为DateTime类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime? ToNumableDateTime(string str)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(str))
                {
                    return Convert.ToDateTime(str);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// String转换为DateTime类型
        /// </summary>
        /// <param name="str"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public static DateTime? ToNullableDateTime(string str, IFormatProvider formatProvider)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(str))
                {
                    return Convert.ToDateTime(str, formatProvider);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// String转换为Boolean类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool? ToNullableBoolean(string str)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(str))
                {
                    if (str.ToLower() != "0" && str.ToLower() != "false")
                    {
                        return Convert.ToBoolean("true");
                    }
                    else
                    {
                        return Convert.ToBoolean("false");
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 对象转字符串
        /// </summary>
        /// <param name="val"></param>
        /// <param name="fullType"></param>
        /// <returns></returns>
        public static string ObjectToString(object val, Type fullType)
        {
            var targetString = "";
            if (val == null || fullType == null)
            {
                return targetString;
            }
            if (fullType.FullName.Contains("DateTime"))
            {
                targetString = ((DateTime)val).ToString("g");
            }
            else
            {
                return val.ToString();
            }
            return targetString;
        }

        /// <summary>
        /// 字符串转换为对象
        /// </summary>
        /// <param name="str"></param>
        /// <param name="fullType"></param>
        /// <returns></returns>
        public static object StringToObject(string str, Type fullType)
        {
            var targetObj = default(object);
            if (string.IsNullOrWhiteSpace(str) || fullType == null)
            {
                return targetObj;
            }
            if (fullType.FullName.Contains("DateTime"))
            {
                targetObj = Convert.ToDateTime(str);
            }
            else if (fullType.FullName.Contains("Int"))
            {
                targetObj = Convert.ToInt32(str);
            }
            else if (fullType.FullName.Contains("Decimal"))
            {
                targetObj = Convert.ToDecimal(str);
            }
            else if (fullType.FullName.Contains("Boolean"))
            {
                targetObj = Convert.ToBoolean(str);
            }
            else
            {
                targetObj = str;
            }
            return targetObj;
        }
    }
}
