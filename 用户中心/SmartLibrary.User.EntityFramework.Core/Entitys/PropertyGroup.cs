using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.User.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 属性组
    /// </summary>
    public class PropertyGroup : BaseEntity<Guid>
    {
        /// <summary>
        /// 分组名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        /// <summary>
        /// 类型 0:内置 1:扩展
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 系统内置，系统内置属性组的可选项固定
        /// </summary>
        public bool SysBuildIn { get; set; }
        /// <summary>
        /// 编码必填
        /// </summary>
        public bool RequiredCode { get; set; }
    }
}
