using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.User.EntityFramework.Core.Entitys
{
    /// 角色菜单权限
    public class SysRoleMenu : BaseEntity<Guid>
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid RoleID { get; set; }

        /// <summary>
        /// 菜单权限ID
        /// </summary>
        public Guid MenuPermissionID { get; set; }

    }
}
