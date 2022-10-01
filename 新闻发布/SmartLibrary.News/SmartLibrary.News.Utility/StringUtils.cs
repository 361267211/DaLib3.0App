using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace SmartLibrary.News.Utility
{
    /// <summary>
    /// 名    称：StringUtils
    /// 作    者：张泽军
    /// 创建时间：2021/9/15 16:14:35
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class StringUtils
    {

        /// <summary>
        /// 将指定的字符串进行html编码,以使字符能正确显示在html页面上
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string HtmlEncode(string str)
        {
            if (string.IsNullOrEmpty(str)) return "";
            return HttpUtility.HtmlEncode(str);
        }

        /// <summary>
        /// 转换或去除HTML标记
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string HtmlDecode(string str)
        {
            if (string.IsNullOrEmpty(str)) return "";
            return HttpUtility.HtmlDecode(str);
        }

        /// <summary>
        /// url编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlEncode(string str)
        {
            return HttpUtility.UrlEncode(str).Replace("+", "%2b");
        }

        /// <summary>
        /// url解码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlDecode(string str)
        {
            return HttpUtility.UrlDecode(str);
        }

        /// <summary>
        /// 连接对象数组为字符串<br/>
        /// 调用对象的ToString()方法，然后按传入的分隔符和修饰符组合成一个字符串<br/>
        /// </summary>
        /// <param name="objs">对象数组</param>
        /// <param name="separate">分隔符</param>
        /// <param name="decorate">修饰符（用于加在对象前后，如：[修饰符]对象[修饰符]）</param>
        /// <returns></returns>
        public static string Join(int[] objs, string separate, string decorate)
        {
            if (objs == null || objs.Length == 0) return "";
            StringBuilder str = new StringBuilder(decorate + objs[0] + decorate);
            for (int i = 1; i < objs.Length; i++)
            {
                str.Append(separate + decorate + objs[i] + decorate);
            }
            return str.ToString();
        }

        public static string Join(long[] objs, string separate, string decorate)
        {
            if (objs == null || objs.Length == 0) return "";
            StringBuilder str = new StringBuilder(decorate + objs[0] + decorate);
            for (int i = 1; i < objs.Length; i++)
            {
                str.Append(separate + decorate + objs[i] + decorate);
            }
            return str.ToString();
        }

        /// <summary>
        /// 连接对象数组为字符串<br/>
        /// 调用对象的ToString()方法，然后按传入的分隔符和修饰符组合成一个字符串<br/>
        /// </summary>
        /// <param name="objs">对象数组</param>
        /// <param name="separate">分隔符</param>
        /// <param name="decorate">修饰符（用于加在对象前后，如：[修饰符]对象[修饰符]）</param>
        /// <returns></returns>
        public static string Join(object[] objs, string separate, string decorate)
        {
            if (objs == null || objs.Length == 0) return "";
            StringBuilder str = new StringBuilder(decorate + objs[0] + decorate);
            for (int i = 1; i < objs.Length; i++)
            {
                str.Append(separate + decorate + objs[i] + decorate);
            }
            return str.ToString();
        }

        /// <summary>
        /// 分割字符串为数字数组<br/>
        /// 调用该方法分割字符串时会先去除字符串前后的分隔符<br/>
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separate">分隔符</param>
        /// <returns></returns>
        public static string[] Split(string str, string separate)
        {
            if (string.IsNullOrEmpty(str)) return new string[0];
            string s = str.Trim(separate.ToCharArray());
            return s.Split(separate.ToCharArray());
        }

        /// <summary>
        /// 将字符串分割为整数数组
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separate"></param>
        /// <returns></returns>
        public static int[] SplitToInt(string str, string separate)
        {
            string[] strs = Split(str, separate);
            int[] values = new int[strs.Length];
            for (int i = 0; i < strs.Length; i++)
            {
                values[i] = int.Parse(strs[i]);
            }
            return values;
        }

        /// <summary>
        /// 获取字符串的半角长度(即一个全角字符按2长度计算)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetDBCLength(string str)
        {
            if (string.IsNullOrEmpty(str)) return 0;
            return Encoding.GetEncoding("gb2312").GetBytes(str).Length;
        }

        /// <summary>
        /// 从字符串的指定位置截取指定长度的子字符串,如果长度为负数则向前截取
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <param name="length">子字符串的长度</param>
        /// <returns>子字符串</returns>
        public static string CutString(string str, int startIndex, int length)
        {
            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length = length * -1;
                    if (startIndex - length < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        startIndex = startIndex - length;
                    }
                }
                if (startIndex > str.Length)
                {
                    return "";
                }
            }
            else
            {
                if (length < 0)
                {
                    return "";
                }
                else
                {
                    if (length + startIndex > 0)
                    {
                        length = length + startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            if (str.Length - startIndex < length)
            {
                length = str.Length - startIndex;
            }
            return str.Substring(startIndex, length);
        }

        /// <summary>
        /// 从字符串的指定位置开始截取到字符串结尾的了符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <returns>子字符串</returns>
        public static string CutString(string str, int startIndex)
        {
            return CutString(str, startIndex, str.Length);
        }

        /// <summary>
        /// 截取半角字符串(一个全角字符按2长度计算),当截取的位置只到全角字符的一半时不会取到该全角字符<br/>
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="startIndex">开始截取位置</param>
        /// <param name="length">要截取的字符串长度</param>
        /// <returns></returns>
        public static string CutDBCString(string str, int startIndex, int length)
        {
            return CutDBCString(str, startIndex, length, "");
        }

        /// <summary>
        /// 截取半角字符串(一个全角字符按2长度计算),当截取的位置只到全角字符的一半时不会取到该全角字符<br/>
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="startIndex">开始截取位置</param>
        /// <param name="length">要截取的字符串长度</param>
        /// <param name="fillStr">当字符串长度超过要截过的长度时追加到字符串末尾</param>
        /// <returns></returns>
        public static string CutDBCString(string str, int startIndex, int length, string fillStr)
        {
            if (string.IsNullOrEmpty(str)) return "";
            StringBuilder sb = new StringBuilder();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding e = Encoding.GetEncoding("gb2312");
            int n = 0;
            int endIndex = startIndex + length;
            foreach (char c in str)
            {
                if (n >= startIndex)
                {
                    n += e.GetByteCount(c.ToString());
                    if (n <= endIndex)
                    {
                        sb.Append(c);
                    }
                    else
                    {
                        //去掉fillStr的长度，然后再加上fillStr
                        int fillLen = GetDBCLength(fillStr);
                        int tempLen = 0;
                        while (sb.Length > 0 && tempLen < fillLen)
                        {
                            int index = sb.Length - 1;
                            string lastChar = sb[index].ToString();
                            tempLen += e.GetByteCount(lastChar);
                            sb.Remove(index, 1);
                        }

                        sb.Append(fillStr);
                        break;
                    }
                }
                else
                {
                    n += e.GetByteCount(c.ToString());
                }
            }
            return sb.ToString();
        }


        /// <summary>
        /// 将字符串转换成hashtable
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Hashtable ToHashTable(string str, string separator, string keyseparator)
        {
            Hashtable ht = new Hashtable();
            string tmpstr;
            ArrayList arr = StrToArrL(str, separator);
            for (int i = 0; i < arr.Count; i++)
            {
                string tmparr = arr[i].ToString();
                if (string.IsNullOrEmpty(tmparr)) { continue; }

                ArrayList arrkey = StrToArrL(tmparr, keyseparator);
                if (arrkey.Count < 2) { continue; }

                string tmpkey = arrkey[0].ToString();
                string tmpvalue = arrkey[1].ToString();
                if (!ht.ContainsKey(tmpkey))
                {
                    ht.Add(tmpkey, tmpvalue);
                }
                else
                {
                    tmpstr = ht[tmpkey].ToString();
                    tmpstr += "|" + tmpvalue;
                    ht[tmpkey] = tmpstr;
                }
            }
            return ht;
        }

        public static ArrayList StrToArrL(string str, string separator)
        {
            ArrayList result = new ArrayList();
            if (string.IsNullOrEmpty(str)) { return result; }

            separator = separator.Replace("$", "\\$");
            separator = separator.Replace("(", "\\(");
            separator = separator.Replace(")", "\\)");
            separator = separator.Replace("*", "\\*");
            separator = separator.Replace("+", "\\+");
            separator = separator.Replace(".", "\\.");
            separator = separator.Replace("[", "\\[");
            separator = separator.Replace("?", "\\?");
            separator = separator.Replace("^", "\\^");
            separator = separator.Replace("{", "\\{");
            separator = separator.Replace("}", "\\}");
            separator = separator.Replace("|", "\\|");
            result = new ArrayList(Regex.Split(str, separator, RegexOptions.IgnoreCase));

            return result;
        }

        /// <summary>
        /// Hashtable型转换为string型
        /// </summary>
        /// <param name="ht"></param>
        /// <param name="separator"></param>
        /// <param name="keyseparator"></param>
        /// <returns></returns>
        public static string HashTableToStr(Hashtable ht, string separator, string keyseparator)
        {
            StringBuilder sb = new StringBuilder();
            if (ht.Count <= 0) { return sb.ToString(); }

            foreach (DictionaryEntry de in ht)
            {
                string tmpkey = de.Key.ToString().Trim();
                string tmpvalue = de.Value.ToString().Trim();

                if (sb.Length <= 0)
                {
                    sb.Append(tmpkey + keyseparator + tmpvalue);
                }
                else
                {
                    sb.Append(separator + tmpkey + keyseparator + tmpvalue);
                }
            }
            return sb.ToString();
        }

        public static string BinaryToAscii(string SourceCode, string Password)
        {
            if (string.IsNullOrEmpty(SourceCode)) { return ""; }
            byte[] bin = Encoding.Default.GetBytes(SourceCode);
            int len = bin.Length;
            byte[] inKey = Encoding.Default.GetBytes(Password);

            byte[] result = BinaryToAscii(bin, inKey);
            return Encoding.Default.GetString(result);
        }

        public static byte[] BinaryToAscii(byte[] bin, byte[] inKey)
        {
            int number = 0;
            byte ch;
            byte ca;
            byte[] key = new byte[9];
            int j = 0;

            int keyLen = inKey.Length;
            if (keyLen < 1)
            {
                return null;
            }
            else if (keyLen < 8)
            {
                int i;
                for (i = 1; i <= keyLen; ++i)
                {
                    key[8 - i] = inKey[keyLen - i];
                }
                int k = 8 - i;
                while (k >= 0)
                {
                    key[k] = 0x30;  //0x30即字符0
                    --k;
                }
            }
            else
            {
                for (int i = 0; i < 8; ++i)
                {
                    key[i] = inKey[i];
                }
            }

            int inLen = bin.Length;
            if (inLen <= 0)
            {
                return null;
            }

            byte[] bout = new byte[inLen * 2];
            for (int i = 0; i < inLen; ++i)
            {
                j = i % 8;
                ch = (byte)(bin[i] ^ key[j]);
                ca = (byte)(ch >> 4);
                ca = (byte)(ca & 0x0f);
                number = ca;
                bout[2 * i] = (byte)(65 + number);

                ca = (byte)(ch & 0x0f);
                number = ca;
                bout[2 * i + 1] = (byte)(65 + number);
            }

            return bout;
        }

        /// <summary>
        /// 过滤HTML语法
        /// </summary>
        public static string FilterHTML(object input)
        {
            if (input == null)
            {
                return string.Empty;
            }
            else
            {
                string html = input.ToString();
                if (string.IsNullOrEmpty(html)) return string.Empty;
                Regex regex1 = new Regex(@"<[^>]*>", RegexOptions.IgnoreCase);
                html = regex1.Replace(html, string.Empty);
                return html.Replace("\"", "“");
            }
        }

        /// <summary>
        /// 检索结果飘红处理
        /// </summary>
        /// <param name="text">需要飘红处理的文字</param>
        /// <param name="keyword">飘红关键字</param>
        /// <returns>飘红处理过的字段</returns>
        public static string ShowTextWithHighlight(string text, string keyword)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(keyword)) return text;
            string[] tokens = keyword.TrimEnd(',').Split(",");
            var mergeTokens = tokens.OrderByDescending(y => y.Length);
            Regex r = new Regex(@"<span[^>]*?>.*?<\/span[^>]*>", RegexOptions.Multiline);
            foreach (string key in mergeTokens)
            {
                MatchCollection matchColl = r.Matches(text);
                for (int i = 0; i < matchColl.Count; i++)
                {
                    text = text.Replace(matchColl[i].ToString(), "{" + i + "}");
                }

                if (!Regex.IsMatch(text, "{\\d*" + key + "\\d*}"))
                {
                    text = Regex.Replace(text, key, "<span style = 'color:red'>" + key + "</span>",
                        RegexOptions.IgnoreCase);
                }

                for (int i = 0; i < matchColl.Count; i++)
                {
                    text = text.Replace("{" + i + "}", matchColl[i].ToString());
                }
            }

            return text;
        }
    }
}
