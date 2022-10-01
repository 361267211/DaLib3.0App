using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.EntityFramework.Core.Entitys.Permission
{
    /// <summary>
    /// 用户角色关系表
    /// </summary>
    public class SysUserRole : Entity<Guid>
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid RoleID { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool DeleteFlag { get; set; }
    }
}
