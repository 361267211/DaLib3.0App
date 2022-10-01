using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.EntityFramework.Core.Dto.Permission
{
    /// <summary>
    /// 
    /// </summary>
    public class SysUserRoleDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserID { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid RoleID { get; set; }

    }
}
