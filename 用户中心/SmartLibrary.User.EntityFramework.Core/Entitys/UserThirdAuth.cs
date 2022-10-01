using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.User.EntityFramework.Core.Entitys
{
    public class UserThirdAuth : BaseEntity<Guid>
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Required]
        public Guid UserID { get; set; }
        /// <summary>
        /// 平台标识
        /// </summary>
        [Required]
        public string LoginType { get; set; }
        /// <summary>
        /// 开放平台Id
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 联合Id
        /// </summary>
        public string UnionId { get; set; }
        /// <summary>
        /// 绑定时间
        /// </summary>
        public DateTime? BindTime { get; set; }
    }
}
