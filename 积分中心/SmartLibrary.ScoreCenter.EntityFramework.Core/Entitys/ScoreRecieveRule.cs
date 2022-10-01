using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys
{
    public class ScoreRecieveRule:BaseEntity<Guid>
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        public Guid ProcessID { get; set; }
        /// <summary>
        /// 任务类型
        /// </summary>
        public int RuleType { get; set; }
        /// <summary>
        /// 属性编码
        /// </summary>
        [Required]
        [StringLength(50)]
        public string PropertyCode { get; set; }
        /// <summary>
        /// 操作符
        /// </summary>
        public int OperateType { get; set; }
        /// <summary>
        /// 属性值
        /// </summary>
        public string PropertyValue { get; set; }
    }
}
