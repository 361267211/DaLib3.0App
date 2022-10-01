using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.User.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 分组规则
    /// </summary>
    public class PropertyGroupRule : BaseEntity<Guid>
    {
        /// <summary>
        /// 属性组ID
        /// </summary>
        public Guid GroupID { get; set; }
        /// <summary>
        /// 对比操作
        /// </summary>
        public int CompareType { get; set; }
        /// <summary>
        /// 属性ID
        /// </summary>
        public Guid PropertyId { get; set; }
        /// <summary>
        /// 属性编码
        /// </summary>
        [StringLength(100)]
        public string PropertyCode { get; set; }
        /// <summary>
        /// 属性值
        /// </summary>
        public string PropertyValue { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 连接方式
        /// </summary>
        public int UnionWay { get; set; }
    }
}
