using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys
{
    public class ScoreManualProcess : BaseEntity<Guid>
    {
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
    }
}
