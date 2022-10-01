using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys
{
    public class UserMedalEvent : BaseEntity<Guid>
    {
        /// <summary>
        /// 勋章Id
        /// </summary>
        public Guid MedalObtainTaskId { get; set; }
        /// <summary>
        /// 用户标识
        /// </summary>
        [Required]
        [StringLength(100)]
        public string UserKey { get; set; }
        /// <summary>
        /// 触发时间
        /// </summary>
        public DateTime TriggerTime { get; set; }
        /// <summary>
        /// 是否重新开始计数
        /// </summary>
        public bool TriggerReset { get; set; }
    }
}
