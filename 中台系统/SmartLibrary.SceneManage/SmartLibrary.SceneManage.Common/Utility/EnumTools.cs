/*********************************************************
 * 名    称：EnumExtension
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/3 11:50:19
 * 描    述：枚举扩展方法
 *
 * 更新历史：
 *
 * *******************************************************/
using SmartLibrary.SceneManage.Common.Attributes;
using SmartLibrary.SceneManage.Common.Dtos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.SceneManage.Common.Utility
{
    public static class EnumTools
    {

        /// <summary>
        /// 用于缓存枚举值的属性值
        /// </summary>
        private static readonly Dictionary<object, EnumAttribute> enumAttr = new Dictionary<object, EnumAttribute>();


        /// <summary>
        /// 获取枚举值的名称，该名称由EnumAttribute定义
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <returns>枚举值对应的名称</returns>
        public static string GetName(Enum value)
        {
            EnumAttribute ea = GetAttribute(value);
            return ea != null ? ea.Name : "";
        }

        /// <summary>
        /// 获取枚举值的名称，该名称由EnumAttribute定义
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <returns>枚举值对应的名称</returns>
        public static string GetDescription(Enum value)
        {
            EnumAttribute ea = GetAttribute(value);
            return ea != null ? ea.Description : "";
        }

        /// <summary>
        /// 获取枚举类型的 值-名称-分类 列表
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static List<Tuple<int, string, string>> GetValueName(Type enumType)
        {
            Type underlyingType = Enum.GetUnderlyingType(enumType);
            List<Tuple<int, string, string>> list = new List<Tuple<int, string, string>>();
            foreach (object o in Enum.GetValues(enumType))
            {
                Enum e = (Enum)o;
                int value = Convert.ToInt32(Convert.ChangeType(o, underlyingType));
                string classname = GetName(e);
                list.Add(Tuple.Create<int, string, string>(value, e.ToString(), classname));
            }
            return list;
        }

        /// <summary>
        /// 获取字典列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<SysDictModel<int>> EnumToList<T>() where T : Enum
        {
            var list = new List<SysDictModel<int>>();
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                var value = item;
                var field = item.GetType().GetField(item.ToString());
                var name = !(Attribute.GetCustomAttribute(field, typeof(EnumAttribute)) is EnumAttribute display) ? field.Name : display.Name;

                list.Add(new SysDictModel<int> { Key = name, Value = (int)value});
            }

            return list;
        }

        /// <summary>
        /// 从字符串转换为枚举类型
        /// </summary>
        /// <typeparam name="T">枚举类型（包括可空枚举）</typeparam>
        /// <param name="str">要转为枚举的字符串</param>
        /// <exception cref="Exception">转换失败</exception>
        /// <returns>转换结果</returns>
        public static T GetEnum<T>(string str)
        {
            return (T)Enum.Parse(typeof(T), str, true); //转换的时候忽略大小写

            //Type type = typeof(T);

            //Type nullableType = Nullable.GetUnderlyingType(type);
            //if (nullableType != null) type = nullableType;

            //Type underlyingType = Enum.GetUnderlyingType(type);
            //object o = Convert.ChangeType(str, underlyingType);

            //if (!Enum.IsDefined(type, o)) throw new Exception("枚举类型\"" + type.ToString() + "\"中没有定义\"" + (o == null ? "null" : o.ToString()) + "\"");

            ////处理可空枚举类型
            //if (nullableType != null)
            //{
            //    ConstructorInfo c = typeof(T).GetConstructor(new Type[] { nullableType });
            //    return (T)c.Invoke(new object[] { o });
            //}
            //return (T)o;
        }

        /// <summary>
        /// 从字符串转换为枚举类型
        /// </summary>
        /// <typeparam name="T">枚举类型（包括可空枚举）</typeparam>
        /// <param name="str">要转为枚举的字符串</param>
        /// <param name="defaultValue">转换失败时返回的默认值</param>
        /// <returns>转换结果</returns>
        public static T GetEnum<T>(string str, T defaultValue) where T : struct
        {
            try
            {
                return Enum.TryParse(str, true, out T result) ? result : defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 判断是否定义了FlagsAttribute属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool HasFlagsAttribute(Type type)
        {
            object[] attributes = type.GetCustomAttributes(typeof(FlagsAttribute), true);
            return attributes != null && attributes.Length > 0;
        }

        /// <summary>
        /// 判断是否包含指定的值
        /// </summary>
        /// <param name="multValue"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsMarked(System.Enum multValue, System.Enum value)
        {
            return (Convert.ToInt32(multValue) & Convert.ToInt32(value)) == Convert.ToInt32(value);
        }

        /// <summary>
        /// 将指定的值拆分为一个枚举值的数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public static T[] GetValues<T>(System.Enum values)
        {
            List<T> l = new List<T>();
            foreach (Enum v in Enum.GetValues(typeof(T)))
            {
                if (IsMarked(values, v))
                {
                    l.Add((T)((object)v));
                }
            }
            return l.ToArray();
        }

        /// <summary>
        /// 获取枚举值定义的属性
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static EnumAttribute GetAttribute(Enum value)
        {

            if (enumAttr.ContainsKey(value))
            {
                EnumAttribute ea = enumAttr[value];
                return ea;
            }
            else
            {
                FieldInfo field = value.GetType().GetField(value.ToString());
                if (field == null) return null;
                EnumAttribute ea = null;
                object[] attributes = field.GetCustomAttributes(typeof(EnumAttribute), true);
                if (attributes != null && attributes.Length > 0)
                {
                    ea = (EnumAttribute)attributes[0];
                }
                //enumAttr[value] = ea;
                return ea;
            }
        }


        /// <summary>
        /// 获取枚举类型的 值-名称 列表
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetTypeValueName(Type enumType)
        {
            Type underlyingType = Enum.GetUnderlyingType(enumType);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (object o in Enum.GetValues(enumType))
            {
                Enum e = (Enum)o;
                string value = Convert.ChangeType(o, underlyingType).ToString();
                dic.Add(value, GetName(e));
            }
            return dic;
        }
    }
}
