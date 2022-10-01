using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Model.Entities
{
    /// <summary>
    /// 积分手动调整
    /// </summary>
    [SqlSugar.SugarTable("ScoreManualProcess")]
    public class ScoreManualProcess
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [Required]
        [StringLength(150)]
        public string Desc { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// 积分有效期
        /// </summary>
        public int ValidTerm { get; set; }
        /// <summary>
        /// 执行状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 用户来源
        /// </summary>
        public int SourceFrom { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string OperatorName { get; set; }
        /// <summary>
        /// 操作者标识
        /// </summary>
        public string OperatorUserKey { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperatorTime { get; set; }
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
    }
}
