using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.User.EntityFramework.Core.Entitys
{
    public class UserImportTempData : BaseEntity<Guid>
    {
        /// <summary>
        /// 操作ID
        /// </summary>
        public Guid BatchId { get; set; }
        [Required]
        [StringLength(100)]
        public string UserName { get; set; }
        [StringLength(100)]
        public string UserGender { get; set; }
        [StringLength(100)]
        public string UserPhone { get; set; }
        [StringLength(100)]
        public string UserType { get; set; }
        [StringLength(100)]
        public string UserTypeName { get; set; }
        [StringLength(100)]
        public string StudentNo { get; set; }
        [StringLength(100)]
        public string Unit { get; set; }
        [StringLength(100)]
        public string Edu { get; set; }
        [StringLength(100)]
        public string College { get; set; }
        [StringLength(100)]
        public string CollegeName { get; set; }
        [StringLength(100)]
        public string CollegeDepart { get; set; }
        [StringLength(100)]
        public string CollegeDepartName { get; set; }
        [StringLength(100)]
        public string Major { get; set; }
        [StringLength(100)]
        public string Grade { get; set; }
        [StringLength(100)]
        public string Class { get; set; }
        [StringLength(100)]
        public string IdCard { get; set; }
        [StringLength(100)]
        public string Email { get; set; }

        public DateTime? Birthday { get; set; }
        [StringLength(100)]
        public string Addr { get; set; }
        [StringLength(200)]
        public string AddrDetail { get; set; }
        [StringLength(100)]
        public string CardNo { get; set; }
        [StringLength(100)]
        public string CardType { get; set; }
        [StringLength(100)]
        public string CardTypeName { get; set; }
        public bool Error { get; set; }
        public string ErrorMsg { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpireTime { get; set; }
        /// <summary>
        /// 用户标识
        /// </summary>
        [StringLength(100)]
        public string UserKey { get; set; }
    }
}
