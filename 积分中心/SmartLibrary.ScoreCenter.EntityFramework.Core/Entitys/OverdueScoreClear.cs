using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys
{
    public class OverdueScoreClear:BaseEntity<int>
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        [Required]
        [StringLength(100)]
        public string UserKey { get; set; }
        /// <summary>
        /// 清算积分过期时间
        /// </summary>
        public DateTime ExpireTime { get; set; }
        /// <summary>
        /// 超过积分
        /// </summary>
        public int OverScore { get; set; }
        /// <summary>
        /// 状态 0：未处理 1：已处理
        /// </summary>
        public int Status { get; set; }
    }
}
