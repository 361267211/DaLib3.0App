using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 名    称：新闻栏目管理权限
    /// 作    者：张泽军
    /// 创建时间：2021/9/7 15:33:39
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NewsColumnPermissions: Entity<string>
    {
        ///// <summary>
        ///// 主键
        ///// </summary>
        //[Key, StringLength(64)]
        //public string Id { get; set; }

        /// <summary>
        /// 栏目ID
        /// </summary>
        [Required]
        public string ColumnID { get; set; }

        ///// <summary>
        ///// 栏目名称
        ///// </summary>
        //[StringLength(50), Required]
        //public string ColumnName { get; set; }

        /// <summary>
        /// 管理员ID
        /// </summary>
        [StringLength(100),Required]
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

        ///// <summary>
        ///// 权限名称
        ///// </summary>
        //[StringLength(20), Required]
        //public string PermissionName { get; set; }

        /// <summary>
        /// 逻辑删除标识
        /// </summary>
        public bool DeleteFlag { get; set; }
    }
}
