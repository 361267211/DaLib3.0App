/*********************************************************
* 名    称：LogPropertyAttribute.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用于定义哪些字段需要进行值差异对比
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Common.Dtos
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class LogPropertyAttribute : Attribute
    {
        private int _sort = 0;
        private string _name = "";
        private string _code = "";

        public LogPropertyAttribute(int sort, string name, string code)
        {
            _sort = sort;
            _name = name;
            _code = code;

        }

        public int Sort
        {
            get { return _sort; }
        }

        public string Name
        {
            get { return _name; }
        }

        public string Code
        {
            get { return _code; }
        }
    }
}
