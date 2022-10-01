using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.User.EntityFramework.Core.Entitys
{
    public class UserPropertyChangeLog : BaseEntity<Guid>
    {
        /// <summary>
        /// 变更类型
        /// </summary>
        public int ChangeType { get; set; }
        /// <summary>
        /// 变更时间
        /// </summary>
        public DateTime ChangeTime { get; set; }
        /// <summary>
        /// 修改人ID
        /// </summary>
        public Guid ChangeUserID { get; set; }
        /// <summary>
        /// 修改人姓名
        /// </summary>
        [StringLength(50)]
        public string ChangeUserName { get; set; }
        /// <summary>
        /// 修改人电话
        /// </summary>
        [StringLength(50)]
        public string ChangeUserPhone { get; set; }
        /// <summary>
        /// 审批状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
        public string Remark { get; set; }
        /// <summary>
        /// 变更内容
        /// </summary>
        [StringLength(500)]
        public string Content { get; set; }
    }
}
