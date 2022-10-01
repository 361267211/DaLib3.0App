using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Model.Entities
{
    /// <summary>
    /// 处理读者
    /// </summary>
    [SqlSugar.SugarTable("ScoreRecieveUser")]
    public class ScoreRecieveUser
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public Guid ID { get; set; }
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
        /// <summary>
        /// 是否删除
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
