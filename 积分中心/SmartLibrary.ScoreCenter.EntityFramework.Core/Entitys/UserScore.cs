using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys
{
    public class UserScore:BaseEntity<Guid>
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        [Required]
        [StringLength(100)]
        public string UserKey { get; set; }
        /// <summary>
        /// 当前可用积分
        /// </summary>
        public int AvailableScore { get; set; }
        /// <summary>
        /// 冻结积分
        /// </summary>
        public int FreezeScore { get; set; }
    }
}
