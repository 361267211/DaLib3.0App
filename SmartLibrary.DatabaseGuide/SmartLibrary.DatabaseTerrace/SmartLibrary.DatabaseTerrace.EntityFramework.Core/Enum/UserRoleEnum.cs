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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Assembly.EntityFramework.Core.Enum
{
    public enum UserRoleEnum
    {
        /// <summary>
        /// 管理员
        /// </summary>
        [Description("管理员")]
        Manager = 1,

        /// <summary>
        /// 操作者
        /// </summary>
        [Description("操作者")]
        Operator = 2,

        /// <summary>
        /// 浏览者
        /// </summary>
        [Description("浏览者")]
        Visitor = 3,

    }
}
