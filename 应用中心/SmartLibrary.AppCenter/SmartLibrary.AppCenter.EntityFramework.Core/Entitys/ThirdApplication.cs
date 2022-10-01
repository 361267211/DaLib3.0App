using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 图书馆三方应用信息
    /// </summary>
    public class ThirdApplication : Entity<Guid>
    {
        /// <summary>
        /// 应用名称
        /// </summary>
        [Required, StringLength(200)]
        public string AppName { get; set; }

        /// <summary>
        /// 应用类型，多选，存储ID串，逗号分隔
        /// </summary>
        [Required, StringLength(200)]
        public string AppType { get; set; }

        /// <summary>
        /// 开发者
        /// </summary>
        [StringLength(100)]
        public string Developer { get; set; }

        /// <summary>
        /// 开发者联系方式
        /// </summary>
        [StringLength(100)]
        public string Contacts { get; set; }

        /// <summary>
        /// 支持终端，多选，存储123，逗号分隔
        /// </summary>
        [Required, StringLength(100)]
        public string Terminal { get; set; }

        /// <summary>
        /// 应用介绍
        /// </summary>
        [StringLength(500)]
        public string AppIntroduction { get; set; }

        /// <summary>
        /// 应用说明
        /// </summary>
        [Column(TypeName = "text")]
        public string AppExplain { get; set; }

        /// <summary>
        /// 应用图标
        /// </summary>
        [StringLength(100)]
        public string AppIcon { get; set; }

        /// <summary>
        /// 前台访问地址
        /// </summary>
        [StringLength(500)]
        public string FrontUrl { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [Required]
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// 状态 1=启用，0=停用
        /// </summary>
        public int Status { get; set; }
    }
}
