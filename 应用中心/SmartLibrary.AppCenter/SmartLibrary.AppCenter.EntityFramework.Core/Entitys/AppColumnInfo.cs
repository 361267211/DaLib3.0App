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
    /// 应用栏目信息
    /// </summary>
    public class AppColumnInfo : Entity<Guid>
    {
        /// <summary>
        /// 删除标识
        /// </summary>
        [Required]
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// 应用ID
        /// </summary>
        [Required, StringLength(100)]
        public string AppId { get; set; }

        /// <summary>
        /// 应用编码(固定)
        /// </summary>
        [Required, StringLength(100)]
        public string AppRouteCode { get; set; }

        /// <summary>
        /// 栏目名称
        /// </summary>
        [Required, StringLength(150)]
        public string ColumnName { get; set; }

        /// <summary>
        /// 栏目ID
        /// </summary>
        [Required, StringLength(100)]
        public string ColumnId { get; set; }

        /// <summary>
        /// 栏目创建时间
        /// </summary>
        [Required]
        public DateTimeOffset ColumnCreateTime { get; set; }

        /// <summary>
        /// 该应用栏目设置页面访问地址
        /// </summary>
        [Required, StringLength(200)]
        public string VisitUrl { get; set; }
    }
}
