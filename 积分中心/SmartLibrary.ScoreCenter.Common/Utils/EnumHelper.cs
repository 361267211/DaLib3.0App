/*********************************************************
* 名    称：EnumHelper.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：枚举扩展
* 更新历史：
*
* *******************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace SmartLibrary.ScoreCenter.Common.Utils
{
    /// <summary>
    /// 枚举扩展
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// 获取枚举对象Key与名称的字典
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static Dictionary<string, int> GetEnumDictionaryItems(Type enumType)
        {
            // 获取类型的字段，初始化一个有限长度的字典
            FieldInfo[] enumFields = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            Dictionary<string, int> enumDic = new(enumFields.Length);

            // 遍历字段数组获取key和name
            foreach (FieldInfo enumField in enumFields)
            {
                int intValue = (int)enumField.GetValue(enumType);
                if (!enumDic.ContainsKey(enumField.Name))
                {
                    enumDic[enumField.Name] = intValue;
                }
            }
            return enumDic;
        }

        /// <summary>
        /// 获取枚举类型key与描述的字典（没有描述则获取name）
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Dictionary<string, int> GetEnumDescDictionaryItems(Type enumType)
        {
            // 获取类型的字段，初始化一个有限长度的字典
            FieldInfo[] enumFields = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            Dictionary<string, int> enumDic = new(enumFields.Length);

            // 遍历字段数组获取key和name
            foreach (FieldInfo enumField in enumFields)
            {
                int intValue = (int)enumField.GetValue(enumType);
                var desc = enumField.GetDescriptionValue<DescriptionAttribute>();
                var descName = desc != null && !string.IsNullOrWhiteSpace(desc.Description) ? desc.Description : enumField.Name;
                if (!enumDic.ContainsKey(descName))
                {
                    enumDic[descName] = intValue;
                }
            }
            return enumDic;
        }

    }
}
