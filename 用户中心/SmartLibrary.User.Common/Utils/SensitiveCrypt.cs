/*********************************************************
* 名    称：SensitiveCrypt.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：数据脱敏
* 更新历史：
*
* *******************************************************/

namespace SmartLibrary.User.Common.Utils
{
    /// <summary>
    /// 脱敏操作
    /// </summary>
    public static class SensitiveCrypt
    {
        public static string EncodeName(string rawName)
        {
            if (string.IsNullOrWhiteSpace(rawName))
            {
                return "";
            }
            var splitLength = rawName.Length;
            if (splitLength == 1)
            {
                return rawName;
            }
            else if (splitLength == 2)
            {
                return $"{rawName.Substring(0, 1)}*";
            }
            else
            {
                return $"{rawName.Substring(0, 1).PadRight(splitLength - 1, '*')}{rawName.Substring(splitLength - 1, 1)}";
            }

        }

        public static string EncodePhone(string rawName)
        {
            if (string.IsNullOrWhiteSpace(rawName))
            {
                return "";
            }
            var splitLength = rawName.Length;
            if (splitLength <= 2)
            {
                return rawName;
            }
            if (splitLength <= 7)
            {
                return $"{rawName.Substring(0, 1).PadRight(splitLength - 1, '*')}{rawName.Substring(splitLength - 4, 1)}";
            }
            else
            {
                return $"{rawName.Substring(0, 3).PadRight(splitLength - 4, '*')}{rawName.Substring(splitLength - 4, 4)}";
            }
        }

        public static string EncodeIdCard(string rawName)
        {
            if (string.IsNullOrWhiteSpace(rawName))
            {
                return "";
            }
            var splitLength = rawName.Length;
            if (splitLength <= 2)
            {
                return rawName;
            }
            if (splitLength <= 8)
            {
                return $"{rawName.Substring(0, 1).PadRight(splitLength - 1, '*')}{rawName.Substring(splitLength - 4, 1)}";
            }

            else
            {
                return $"{rawName.Substring(0, 4).PadRight(splitLength - 4, '*')}{rawName.Substring(splitLength - 4, 4)}";
            }
        }

        public static string EncodeEmail(string rawName)
        {
            if (string.IsNullOrWhiteSpace(rawName))
            {
                return "";
            }
            var splitLength = rawName.Length;
            var lastLength = rawName.IndexOf("@");
            var addr = rawName.Substring(lastLength);
            var addrLength = addr.Length;
            if (splitLength <= addrLength + 3)
            {
                return $"{"****"}{addr}";
            }

            else
            {
                return $"{rawName.Substring(0, 3).PadRight(splitLength - addrLength, '*')}{addr}";
            }
        }

        public static string EncodeAddr(string rawName)
        {
            if (string.IsNullOrWhiteSpace(rawName))
            {
                return "";
            }
            var splitLength = rawName.Length;
            var showLength = splitLength / 3 * 2;
            return $"{rawName.Substring(0, showLength).PadRight(splitLength, '*')}";

        }
    }
}
