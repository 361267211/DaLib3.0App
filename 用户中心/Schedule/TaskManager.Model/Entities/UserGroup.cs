using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Model.Entities
{
    /// <summary>
    /// 用户组实体
    /// </summary>
    [SqlSugar.SugarTable("UserGroup")]
    public class UserGroup
    {
        public UserGroup()
        {
            CreateTime = DateTime.Now;
            UpdateTime = DateTime.Now;
        }
        /// <summary>
        /// 主键标识
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 用户组ID
        /// </summary>
        public Guid GroupID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserID { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>
        public bool DeleteFlag { get; set; }
        /// <summary>
        /// 预备删除
        /// </summary>
        public bool LaterDel { get; set; }
        /// <summary>
        /// 预备新增
        /// </summary>
        public bool LaterInsert { get; set; }
        public DateTimeOffset CreateTime { get; set; }
        public DateTimeOffset UpdateTime { get; set; }
    }
}
