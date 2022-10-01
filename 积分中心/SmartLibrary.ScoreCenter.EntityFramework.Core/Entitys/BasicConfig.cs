using System;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys
{
    public class BasicConfig : BaseEntity<Guid>
    {
        /// <summary>
        /// 是否显示积分等级
        /// </summary>
        public bool ShowLevel { get; set; }
        /// <summary>
        /// 是否显示规则
        /// </summary>
        public bool ShowRule { get; set; }
        /// <summary>
        /// 积分规则
        /// </summary>
        [StringLength(8000)]
        public string RuleContent { get; set; }
    }
}
