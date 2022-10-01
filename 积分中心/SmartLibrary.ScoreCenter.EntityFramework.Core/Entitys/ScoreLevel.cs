using System;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys
{
    public class ScoreLevel : BaseEntity<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        /// <summary>
        /// 达成积分
        /// </summary>
        public int ArchiveScore { get; set; }
    }
}
