using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys
{
    public class UserMedal:BaseEntity<Guid>
    {
        /// <summary>
        /// 勋章任务
        /// </summary>
        public Guid MedalObtainTaskId { get; set; }
        /// <summary>
        /// 用户标识
        /// </summary>
        [Required]
        [StringLength(100)]
        public string UserKey { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        ///当前达成次数
        /// </summary>
        /// </summary>
        public int TriggerTime { get; set; }
        /// <summary>
        /// 共需达成次数
        /// </summary>
        public int TotalTime { get; set; }
    }
}
