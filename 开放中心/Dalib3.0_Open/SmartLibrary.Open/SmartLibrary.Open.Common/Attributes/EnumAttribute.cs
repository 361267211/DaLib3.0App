/*********************************************************
 * 名    称：EnumAttribute
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/3 13:49:26
 * 描    述：枚举扩展特性
 *
 * 更新历史：
 *
 * *******************************************************/
using System;

namespace SmartLibrary.Open.Common.Attributes
{
    /// <summary>
    /// 枚举的属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class EnumAttribute : Attribute
    {
        private string _name;
        private string _description;

        /// <summary>
        /// 枚举名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 枚举描述
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">枚举名称</param>
        public EnumAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">枚举名称</param>
        /// <param name="description">枚举描述</param>
        public EnumAttribute(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }
    }
}
