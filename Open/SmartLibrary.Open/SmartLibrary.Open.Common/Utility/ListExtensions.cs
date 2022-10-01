/*********************************************************
 * 名    称：ListExtensions
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/3 20:38:04
 * 描    述：List扩展方法
 *
 * 更新历史：
 *
 * *******************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Common.Utility
{
    public static class ListExtensions
    {
        public static string Join<T>(this IList<T> list, char joinChar)
        {
            return list.Join(joinChar.ToString());
        }

        public static string Join<T>(this IList<T> list, string joinString)
        {
            if (list == null || !list.Any())
                return String.Empty;

            StringBuilder result = new StringBuilder();

            int listCount = list.Count;
            int listCountMinusOne = listCount - 1;

            if (listCount > 1)
            {
                for (var i = 0; i < listCount; i++)
                {
                    if (i != listCountMinusOne)
                    {
                        result.Append(list[i]);
                        result.Append(joinString);
                    }
                    else
                        result.Append(list[i]);
                }
            }
            else
                result.Append(list[0]);

            return result.ToString();
        }
    }
}
