using SmartLibrary.AppCenter.EntityFramework.Core.Entitys.Permission;
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
    public class PermissionNode : SysMenuPermission
    {
        public List<PermissionNode> PermissionNodes { get; set; }
    }
}
