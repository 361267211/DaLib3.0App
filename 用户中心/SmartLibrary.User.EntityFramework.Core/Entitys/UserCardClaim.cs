using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.User.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 用户领卡申请
    /// </summary>
    public class UserCardClaim : BaseEntity<Guid>
    {
        /// <summary>
        /// 卡ID
        /// </summary>
        public Guid CardID { get; set; }
        /// <summary>
        /// 申请人ID
        /// </summary>
        public Guid UserID { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime ApplyTime { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
        public string Remark { get; set; }


    }
}
