using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 管理员角色
    /// </summary>
    public class UserRole : Entity<Guid>
    {
        /// <summary>
        /// 管理员唯一标识
        /// </summary>
        [Required, StringLength(50)]
        public string UserKey { get; set; }

        /// <summary>
        /// 分配角色ID串,英文逗号(,)分隔
        /// </summary>
        [Required, StringLength(1000)]
        public string ManagerRoleIds { get; set; }

        /// <summary>
        /// 是否是超级管理员
        /// </summary>
        [Required]
        public bool IsSuper { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [Required]
        public bool DeleteFlag { get; set; }
    }
}
