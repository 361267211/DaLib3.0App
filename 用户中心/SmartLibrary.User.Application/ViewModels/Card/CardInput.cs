/*********************************************************
* 名    称：CardInput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：读者卡编辑输入
* 更新历史：
*
* *******************************************************/
using System;
using System.Collections.Generic;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 读者卡编辑输入
    /// </summary>
    public class CardInput
    {
        public CardInput()
        {
            Properties = new List<CardPropertyInput>();
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
        /// 卡状态
        /// </summary>
        public int Status { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string Secret { get; set; }
        /// <summary>
        /// 押金
        /// </summary>
        public decimal Deposit { get; set; }
        /// <summary>
        /// 是否主卡
        /// </summary>
        public bool IsPrincipal { get; set; }
        /// <summary>
        /// 卡属性
        /// </summary>
        public List<CardPropertyInput> Properties { get; set; }
    }
}
