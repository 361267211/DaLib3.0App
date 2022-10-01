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
    public class SysRoleMenuDto
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public Guid MenuPermissionID { get; set; }

    }
}
