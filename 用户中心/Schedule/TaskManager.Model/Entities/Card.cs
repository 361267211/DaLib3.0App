using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Model.Entities
{
    /// <summary>
    /// 卡管理
    /// </summary>
    [SqlSugar.SugarTable("Card")]
    [Serializable]
    public class Card
    {
        public Guid ID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }
        /// <summary>
        /// 物理吗
        /// </summary>
        public string PhysicNo { get; set; }
        /// <summary>
        /// 认证号
        /// </summary>
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
        public decimal Deposit { get; set; }
        /// <summary>
        /// 卡密码
        /// </summary>
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
        /// 删除标记
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
        /// <summary>
        /// 同步标记
        /// </summary>
        public string AsyncReaderId { get; set; }
    }
}
