using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Model.Entities
{
    /// <summary>
    /// 分组规则
    /// </summary>
    [SqlSugar.SugarTable("PropertyGroupRule")]
    public class PropertyGroupRule
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
        /// <summary>
        /// 删除标记
        /// </summary>
        public bool DeleteFlag { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset CreateTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTimeOffset UpdateTime { get; set; }
    }
}
