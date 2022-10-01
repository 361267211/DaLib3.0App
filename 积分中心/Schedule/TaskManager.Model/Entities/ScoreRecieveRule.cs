using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Model.Entities
{
    /// <summary>
    /// 处理规则
    /// </summary>
    [SqlSugar.SugarTable("ScoreRecieveRule")]
    public class ScoreRecieveRule
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 任务ID
        /// </summary>
        public Guid ProcessID { get; set; }
        /// <summary>
        /// 规则类型
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
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool DeleteFlag { get; set; }
    }
}
