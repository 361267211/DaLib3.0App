using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Model.Entities
{
    [SqlSugar.SugarTable("UserProperty")]
    public class UserProperty
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 读者ID
        /// </summary>
        public Guid UserID { get; set; }
        /// <summary>
        /// 属性ID
        /// </summary>
        public Guid PropertyID { get; set; }
        /// <summary>
        /// 属性值
        /// </summary>
        public string PropertyValue { get; set; }
        /// <summary>
        /// 数值
        /// </summary>
        public decimal? NumValue { get; set; }
        /// <summary>
        /// 时间值
        /// </summary>
        public DateTime? TimeValue { get; set; }
        /// <summary>
        /// 布尔值
        /// </summary>
        public bool? BoolValue { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>
        public bool DeleteFlag { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTimeOffset UpdateTime { get; set; }
        /// <summary>
        /// 租户标识
        /// </summary>
        public string TenantId { get; set; }
    }
}
