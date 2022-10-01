/*********************************************************
* 名    称：ReflectionHelper.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：反射工具
* 更新历史：
*
* *******************************************************/
using System;
using System.Reflection;

namespace SmartLibrary.User.Common.Utils
{
    /// <summary>
    /// 反射工具
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// 获取字段特性
        /// </summary>
        /// <param name="field"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetDescriptionValue<T>(this FieldInfo field) where T : Attribute
        {
            // 获取字段的指定特性，不包含继承中的特性
            object[] customAttributes = field.GetCustomAttributes(typeof(T), false);

            // 如果没有数据返回null
            return customAttributes.Length > 0 ? (T)customAttributes[0] : null;
        }

        /// <summary>
        /// 获取类型的特性
        /// </summary>
        /// <param name="field"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetDescriptionValue<T>(this Type field) where T : Attribute
        {
            // 获取字段的指定特性，不包含继承中的特性
            object[] customAttributes = field.GetCustomAttributes(typeof(T), false);

            // 如果没有数据返回null
            return customAttributes.Length > 0 ? (T)customAttributes[0] : null;
        }
    }
}
