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

namespace SmartLibrary.Assembly.Application.Attributes
{
    /// <summary>
    /// 标记某方法的返回值将被缓存
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class PermissionObjAttribute : Attribute
    {
        public PermissionObjAttribute(string objType)
        {
            ObjType = objType;
        }
        /// <summary>
        /// 缓存key，使用string Format可以缓存不同参数的方法
        /// </summary>
        public string ObjType { get; }

    }
}
