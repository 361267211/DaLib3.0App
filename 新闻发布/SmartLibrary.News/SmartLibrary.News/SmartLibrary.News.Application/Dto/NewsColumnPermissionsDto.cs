using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application.Dto
{
    /// <summary>
    /// 名    称：NewsColumnPermissionsDto
    /// 作    者：张泽军
    /// 创建时间：2021/9/9 15:07:23
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NewsColumnPermissionsDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key, StringLength(64)]
        public string Id { get; set; }

        /// <summary>
        /// 栏目ID
        /// </summary>
        [Required]
        public string ColumnID { get; set; }

        ///// <summary>
        ///// 栏目名称
        ///// </summary>
        //[StringLength(50)]
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
        //[StringLength(20)]
        //public string PermissionName { get; set; }
    }
}
