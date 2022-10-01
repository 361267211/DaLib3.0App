/*********************************************************
* 名    称：CardDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220301
* 描    述：读者卡
* 更新历史：
*
* *******************************************************/
using System;
using System.Collections.Generic;

namespace SmartLibrary.User.Application.Dtos.Card
{
    /// <summary>
    /// 读者卡
    /// </summary>
    public class CardDto
    {
        public CardDto()
        {
            Properties = new List<CardPropertyDto>();
        }
        /// <summary>
        /// 卡ID
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid? UserID { get; set; }

        /// <summary>
        /// 卡号
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 条码号
        /// </summary>
        public string BarCode { get; set; }
        /// <summary>
        /// 物理号
        /// </summary>
        public string PhysicNo { get; set; }
        /// <summary>
        /// 认证好
        /// </summary>
        public string IdentityNo { get; set; }
        /// <summary>
        /// 卡类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 开类型
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 卡状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 发卡时间
        /// </summary>
        public DateTime? IssueDate { get; set; }
        /// <summary>
        /// 押金
        /// </summary>
        public decimal Deposit { get; set; }
        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime? ExpireDate { get; set; }
        /// <summary>
        /// 用途 0:无指定用途 1:临时馆员卡登陆凭据
        /// </summary>
        public int Usage { get; set; }
        /// <summary>
        /// 系统内置
        /// </summary>
        public bool SysBuildIn { get; set; }
        /// <summary>
        /// 是否主卡
        /// </summary>
        public bool IsPrincipal { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Secret { get; set; }
        /// <summary>
        /// 密码变更时间
        /// </summary>
        public DateTime? SecretChangeTime { get; set; }
        public List<CardPropertyDto> Properties { get; set; }

    }
}
