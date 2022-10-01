using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.User.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 用户组
    /// </summary>
    public class Group : BaseEntity<Guid>
    {
        /// <summary>
        /// 用户组名称
        /// </summary>
        [StringLength(20)]
        public string Name { get; set; }
        /// <summary>
        /// 用户组描述信息
        /// </summary>
        [StringLength(200)]
        public string Desc { get; set; }
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
        /// <summary>
        /// 关联任务Key
        /// </summary>
        [StringLength(100)]
        public string RefTaskKey { get; set; }
    }
}
