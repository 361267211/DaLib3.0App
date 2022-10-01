using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Model.Entities
{
    /// <summary>
    /// 用户组
    /// </summary>
    [SqlSugar.SugarTable("Group")]
    public class Group
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 用户组名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 数据来源 0:规则创建 1:导入创建
        /// </summary>
        public int SourceFrom { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        public Guid CreateUserID { get; set; }
        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 最近同步时间
        /// </summary>
        public DateTime LastSyncTime { get; set; }
          
    }
}
