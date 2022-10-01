/*********************************************************
* 名    称：StringExtensions.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：字符串扩展
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Common.Extensions
{
    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static class StringExtensions
    {
        public static bool IsNullOrWhiteSpace(this string val)
        {
            return string.IsNullOrWhiteSpace(val);
        }

        public static Guid? ToGuid(this string val)
        {
            Guid? guid = null;
            try
            {
                guid = new Guid(val);
            }
            catch
            {
                guid = null;
            }
            return guid;
        }
    }
}
