/*********************************************************
 * 名    称：IgnoreMappingAttribute
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：自定义特性 添加了此特性的属性不会被配置到siteglobalconfig上。
 *
 * 更新历史：
 *
 * *******************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Adapters.Util
{
   public static class FirstSpell
    {
        /// <summary>
        /// 获得字符串首字符字母（大写）；
        /// </summary>
        /// <param name="cnChar"></param>
        /// <returns></returns>
        public static string GetStringFirstSpell(string cnChar)
        {
            //除字母、数字、汉字以外的返回"*"
            var result = "*";

            if (string.IsNullOrEmpty(cnChar.Trim()))
                return result;

            cnChar = cnChar.Trim().Substring(0, 1);
            byte[] arrCn = Encoding.Default.GetBytes(cnChar);

            //首字为字符,占一个字节
            if (arrCn.Length <= 1)
            {
                //大写英文字母
                if ((short)arrCn[0] >= 65 && (short)arrCn[0] <= 90)
                    return cnChar;

                //小写英文字母
                if (arrCn[0] >= 97 && arrCn[0] <= 122)
                    return Encoding.Default.GetString(new byte[] { (byte)((short)arrCn[0] - 32) });

                //数字
                switch (cnChar)
                {
                    case "1":
                        result = "Y";
                        break;
                    case "2":
                        result = "E";
                        break;
                    case "3":
                    case "4":
                        result = "S";
                        break;
                    case "5":
                        result = "W";
                        break;
                    case "0":
                    case "6":
                        result = "L";
                        break;
                    case "7":
                        result = "Q";
                        break;
                    case "8":
                        result = "B";
                        break;
                    case "9":
                        result = "J";
                        break;
                }
                return result;
            }
            //首字为汉字,占两个字节
            else
            {
                int area = (short)arrCn[0];
                int pos = (short)arrCn[1];
                int code = (area << 8) + pos;
                int[] areacode = { 45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324, 49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481 };
                for (int i = 0; i < 26; i++)
                {
                    int max = 55290;
                    if (i != 25) max = areacode[i + 1];
                    if (areacode[i] <= code && code < max)
                    {
                        return Encoding.Default.GetString(new byte[] { (byte)(65 + i) });
                    }
                }
                return result;
            }
        }
    }
}
