using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.User.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 卡管理
    /// </summary>
    public class Card : BaseEntity<Guid>
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserID { get; set; }
        /// <summary>
        /// 卡号
        /// </summary>
        [StringLength(100)]
        public string No { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        [StringLength(200)]
        public string BarCode { get; set; }
        /// <summary>
        /// 物理码
        /// </summary>
        [StringLength(200)]
        public string PhysicNo { get; set; }
        /// <summary>
        /// 认证号
        /// </summary>
        [StringLength(200)]
        public string IdentityNo { get; set; }
        /// <summary>
        /// 卡类型 
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 卡类型名称
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 卡状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 是否主卡
        /// </summary>
        public bool IsPrincipal { get; set; }
        /// <summary>
        /// 发证日期
        /// </summary>
        public DateTime IssueDate { get; set; }
        /// <summary>
        /// 截止日期
        /// </summary>
        public DateTime ExpireDate { get; set; }
        /// <summary>
        /// 押金
        /// </summary>
        [Column("Deposit", TypeName = "decimal(18,2)")]
        public decimal Deposit { get; set; }
        /// <summary>
        /// 卡密码
        /// </summary>
        [StringLength(500)]
        public string Secret { get; set; }
        /// <summary>
        /// 用途 0:无指定用途 1:临时馆员卡登陆凭据
        /// </summary>
        public int Usage { get; set; }
        /// <summary>
        /// 系统内置
        /// </summary>
        public bool SysBuildIn { get; set; }
        /// <summary>
        /// 同步原读者ID
        /// </summary>
        [StringLength(100)]
        public string AsyncReaderId { get; set; }
        /// <summary>
        /// 密码变更时间
        /// </summary>
        public DateTime? SecretChangeTime { get; set; }
    }
}
