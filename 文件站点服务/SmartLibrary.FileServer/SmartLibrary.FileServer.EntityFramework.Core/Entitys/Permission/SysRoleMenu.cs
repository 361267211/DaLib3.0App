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

using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.FileServer.EntityFramework.Core.Entitys
{
    /// 角色菜单权限
    public class SysRoleMenu : Entity<Guid>
    {



        /// <summary>
        /// 角色ID
        /// </summary>

        public Guid RoleID { get; set; }

        /// <summary>
        /// 菜单权限ID
        /// </summary>

        public Guid MenuPermissionID { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>

        public bool DeleteFlag { get; set; }



    }
}
