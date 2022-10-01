using Furion.DatabaseAccessor;
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.AppCenter.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 应用改名记录
    /// </summary>
    public class AppReName : Entity<Guid>
    {
        /// <summary>
        /// 改名用户
        /// </summary>
        [StringLength(50), Required]
        public string UserKey { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        [StringLength(50), Required]
        public string AppId { get; set; }

        /// <summary>
        /// 应用新名称
        /// </summary>
        [StringLength(250), Required]
        public string AppNewName { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [Required]
        public bool DeleteFlag { get; set; }
    }
}
