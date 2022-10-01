using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.User.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 属性组选项
    /// </summary>
    public class PropertyGroupItem : BaseEntity<Guid>
    {
        public Guid GroupID { get; set; }
        /// <summary>
        /// 名称
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
        /// 数据状态 0:未激活 1:正常
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 审批状态 0:审批中 1:正常
        /// </summary>
        public int ApproveStatus { get; set; }
        /// <summary>
        /// 内置选项
        /// </summary>
        public bool SysBuildIn { get; set; }

    }
}
