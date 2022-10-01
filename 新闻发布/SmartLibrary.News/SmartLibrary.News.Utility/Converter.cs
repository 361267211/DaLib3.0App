using SmartLibrary.News.Utility.EnumTool;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace SmartLibrary.News.Utility
{
    /// <summary>
    /// 名    称：Converter
    /// 作    者：张泽军
    /// 创建时间：2021/9/10 13:07:21
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public static class Converter
    {
        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(object expression, bool defValue)
        {
            if (expression != null)
                return StrToBool(expression.ToString(), defValue);

            return defValue;
        }



        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(string expression, bool defValue)
        {
            if (!string.IsNullOrEmpty(expression))
            {
                if (string.Compare(expression, "true", true) == 0)
                    return true;
                else if (string.Compare(expression, "false", true) == 0)
                    return false;
                else if (string.Compare(expression, "1", true) == 0)
                    return true;
                else if (string.Compare(expression, "0", true) == 0)
                    return false;
            }
            return defValue;
        }


        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int ObjectToInt(object expression)
        {
            return ObjectToInt(expression, 0);
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int ObjectToInt(object expression, int defValue)
        {
            if (expression != null)
                return StrToInt(expression.ToString(), defValue);

            return defValue;
        }

        /// <summary>
        /// 将对象转换为Int32类型,转换失败返回0
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(string str)
        {
            return StrToInt(str, 0);
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(string str, int defValue)
        {
            if (string.IsNullOrEmpty(str) || str.Trim().Length >= 11 || !Regex.IsMatch(str.Trim(), @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
                return defValue;

            int rv;
            if (int.TryParse(str, out rv))
                return rv;

            return defValue;
        }


        /// <summary>
        /// 将对象转换为Int64类型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的long类型结果</returns>
        public static long ObjectToLong(object expression)
        {
            return ObjectToLong(expression, 0);
        }
        /// <summary>
        /// 将对象转换为Int64类型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的long类型结果</returns>
        public static long ObjectToLong(object expression, long defValue)
        {
            if (expression != null)
                return StrToLong(expression.ToString(), defValue);

            return defValue;
        }
        /// <summary>
        /// 将对象转换为Int64类型,转换失败返回0
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns>转换后的long类型结果</returns>
        public static long StrToLong(string str)
        {
            return StrToLong(str, 0);
        }
        /// <summary>
        /// 将对象转换为Int64类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的long类型结果</returns>
        public static long StrToLong(string str, long defValue)
        {
            if (string.IsNullOrEmpty(str) || str.Trim().Length >= 21 || !Regex.IsMatch(str.Trim(), @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
                return defValue;

            long rv;
            if (long.TryParse(str, out rv))
                return rv;

            return defValue;
        }

        public static decimal ToDecimal(string value)
        {
            return ToDecimal(value, 0);
        }

        public static decimal ToDecimal(string value, int defaultvalue)
        {
            if (string.IsNullOrEmpty(value) || !Regex.IsMatch(value.Trim(), @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
                return defaultvalue;

            decimal myint = 0M;
            if (!decimal.TryParse(value, out myint))
            {
                myint = defaultvalue;
            }
            return myint;
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float ObjectToFloat(object strValue, float defValue)
        {
            if (strValue == null)
                return defValue;

            return StrToFloat(strValue.ToString(), defValue);
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float ObjectToFloat(object strValue)
        {
            return ObjectToFloat(strValue.ToString(), 0);
        }
        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(object strValue, float defValue)
        {
            if (strValue == null)
                return defValue;

            return StrToFloat(strValue.ToString(), defValue);
        }
        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(object strValue)
        {
            if (strValue == null)
                return 0;

            return StrToFloat(strValue.ToString(), 0);
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(string strValue, float defValue)
        {
            if (strValue == null)
                return defValue;

            float intValue = defValue;

            if (strValue != null)
            {
                bool IsFloat = Regex.IsMatch(strValue, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsFloat)
                {
                    float.TryParse(strValue, out intValue);
                    string tmpValue = intValue.ToString("f2");
                    float.TryParse(tmpValue, out intValue);
                }
            }
            return intValue;
        }
        /// <summary>
        /// 格式化标准日期
        /// </summary>
        /// <param name="strDateTime">日期时间</param>
        /// <returns></returns>
        public static DateTime StrToDateTime(object strDateTime)
        {
            return Convert.ToDateTime(strDateTime);
        }
        /// <summary>
        /// 格式化标准日期
        /// </summary>
        /// <param name="strDateTime">日期时间</param>
        /// <returns></returns>
        public static DateTime StrToDateTime(string strDateTime)
        {
            return Convert.ToDateTime(strDateTime);
        }

        /// <summary>
        /// 格式化标准日期
        /// </summary>
        /// <param name="value">日期时间</param>
        /// <returns></returns>
        public static DateTime ObjToDateTime(object value, DateTime defaultvalue)
        {
            var time = new DateTime();
            if (value != null && !DateTime.TryParse(value.ToString(), out time))
            {
                time = defaultvalue;
            }
            return time;

        }


        /// <summary>
        /// 标准IP格式转十进制
        /// </summary>
        /// <param name="IPstr"></param>
        /// <returns></returns>
        public static decimal IPSTRtoDec(string IPstr)
        {
            decimal IP = 0;
            if (IPstr == string.Empty)
            {
                return 0;
            }
            string[] IPstrs = IPstr.Split('.');
            if (IPstrs.Length == 4)
            {
                try
                {
                    for (int i = 3; i >= 0; i--)
                    {
                        IP += Convert.ToDecimal(IPstrs[i]) * (decimal)Math.Pow(256, 3 - i);
                    }
                }
                catch
                {
                    IP = 0;
                }
                return IP;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 十进制IP转标准格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string IPDECtoStr(decimal ip)
        {
            long ip4 = Convert.ToInt64(ip) % 256;
            long ip3 = Convert.ToInt64(ip) / 256 % 256;
            long ip2 = Convert.ToInt64(ip) / 256 / 256 % 256;
            long ip1 = Convert.ToInt64(ip) / 256 / 256 / 256;
            return ip1.ToString() + "." + ip2.ToString() + "." + ip3.ToString() + "." + ip4.ToString();

        }

        /// <summary>
        /// 将指当前值转换为指定的类型<br/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToType<T>(object value)
        {
            if (value == null || value is string && string.IsNullOrEmpty(value as string))
            {
                return default;
            }

            Type t = typeof(T);
            //处理可空类型
            Type nullable = Nullable.GetUnderlyingType(t);
            if (nullable != null)
            {
                //如果传入的值是空引用或空字符串，返回默认值
                if (value == null || value is string && string.IsNullOrEmpty(value as string))
                {
                    return default;
                }
                t = nullable;
            }

            //转换
            object o = null;
            try
            {
                o = Convert.ChangeType(value, t.IsEnum ? Enum.GetUnderlyingType(t) : t);
            }
            catch (Exception)
            {
                try
                {
                    o = Enum.Parse(t, value.ToString(), true);
                }
                catch
                {
                    return default;
                }
            }

            if (t.IsEnum && !EnumUtils.HasFlagsAttribute(t) && !Enum.IsDefined(t, o)) throw new Exception("枚举类型\"" + t.ToString() + "\"中没有定义\"" + (o == null ? "null" : o.ToString()) + "\"");

            //可空类型
            if (nullable != null)
            {
                ConstructorInfo constructor = typeof(T).GetConstructor(new Type[] { nullable });
                return (T)constructor.Invoke(new object[] { o });
            }

            return (T)o;
        }

        /// <summary>
        /// 将指当前值转换为指定的类型,如果转换失败返回默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T ToType<T>(object value, T defaultValue)
        {
            try
            {
                return ToType<T>(value);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 日期转long
        /// </summary>
        /// <param name="dt">时间</param>
        /// <returns></returns>
        public static long ConvertDataTimeToLong(DateTime dt)
        {
            //var dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            var dtStart = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
            var toNow = dt.Subtract(dtStart);
            long timeStamp = toNow.Ticks;
            return long.Parse(s: timeStamp.ToString(CultureInfo.InvariantCulture).Substring(0, timeStamp.ToString(CultureInfo.InvariantCulture).Length - 4));

        }
    }
}
