using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 名    称：NavigationColumnPermissions
    /// 作    者：张泽军
    /// 创建时间：2021/9/30 15:37:33
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NavigationColumnPermissions: Entity<string>
    {
        /// <summary>
        /// 栏目ID
        /// </summary>
        [Required]
        public string ColumnID { get; set; }

        /// <summary>
        /// 管理员ID
        /// </summary>
        [StringLength(100), Required]
        public string ManagerID { get; set; }

        /// <summary>
        /// 管理员名称
        /// </summary>
        [StringLength(50), Required]
        public string Manager { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        [Required]
        public int Permission { get; set; }

        /// <summary>
        /// 逻辑删除标识
        /// </summary>
        public bool DeleteFlag { get; set; }
    }
}
