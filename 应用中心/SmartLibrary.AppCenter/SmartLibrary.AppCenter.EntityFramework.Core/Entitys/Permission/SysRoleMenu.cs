using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.EntityFramework.Core.Entitys.Permission
{
    /// <summary>
    /// 角色菜单权限
    /// </summary>
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
