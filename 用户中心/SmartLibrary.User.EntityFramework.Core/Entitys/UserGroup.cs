using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.User.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 用户组关系
    /// </summary>
    public class UserGroup : BaseEntity<Guid>
    {
        /// <summary>
        /// 用户组ID
        /// </summary>
        public Guid GroupID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserID { get; set; }
        /// <summary>
        /// 预备删除
        /// </summary>
        public bool LaterDel { get; set; }
        /// <summary>
        /// 预备新增
        /// </summary>
        public bool LaterInsert { get; set; }
    }
}
