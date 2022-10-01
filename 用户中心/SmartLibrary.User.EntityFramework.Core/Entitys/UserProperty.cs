using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.User.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 用户属性
    /// </summary>
    public class UserProperty : BaseEntity<Guid>
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Required]
        public Guid UserID { get; set; }
        /// <summary>
        /// 属性ID
        /// </summary>
        [Required]
        public Guid PropertyID { get; set; }
        /// <summary>
        /// 属性值
        /// </summary>
        [StringLength(500)]
        public string PropertyValue { get; set; }
        /// <summary>
        /// 数字类型
        /// </summary>
        [Column("NumValue", TypeName = "decimal(18,2)")]
        public decimal? NumValue { get; set; }
        /// <summary>
        /// 时间类型
        /// </summary>
        public DateTime? TimeValue { get; set; }
        /// <summary>
        /// 是非类型
        /// </summary>
        public bool? BoolValue { get; set; }

    }
}
