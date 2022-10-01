using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys
{
    public class ScoreRecieveUser : BaseEntity<Guid>
    {
        /// <summary>
        /// 手动调节任务ID
        /// </summary>
        public Guid ProcessID { get; set; }
        /// <summary>
        /// 用户标识
        /// </summary>
        [Required]
        [StringLength(100)]
        public string UserKey { get; set; }

        /// <summary>
        /// 数据来源
        /// </summary>
        public int SourceFrom { get; set; }
    }
}
