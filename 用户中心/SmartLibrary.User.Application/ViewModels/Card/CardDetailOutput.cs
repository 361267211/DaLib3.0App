/*********************************************************
* 名    称：CardDetailOutput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：读者卡详情
* 更新历史：
*
* *******************************************************/
using System;
using System.Collections.Generic;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 读者卡详情
    /// </summary>
    public class CardDetailOutput
    {
        public CardDetailOutput()
        {
            Properties = new List<CardPropertyItemOutput>();
            UserProperties = new List<UserPropertyItemOutput>();
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid? UserID { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string UserPhone { get; set; }
        /// <summary>
        /// 用户类型
        /// </summary>
        public string UserType { get; set; }
        /// <summary>
        /// 学号
        /// </summary>
        public string UserStudentNo { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string UserEmail { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public string UserIdCard { get; set; }
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
        public string Status { get; set; }
        /// <summary>
        /// 用户属性
        /// </summary>
        public List<CardPropertyItemOutput> Properties { get; set; }
        public List<UserPropertyItemOutput> UserProperties { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset CreateTime { get; set; }
        /// <summary>
        /// 编辑时间
        /// </summary>
        public DateTimeOffset UpdateTime { get; set; }
        /// <summary>
        /// 发卡日期
        /// </summary>
        public DateTime IssueDate { get; set; }
        /// <summary>
        /// 过期日期
        /// </summary>
        public DateTime ExpireDate { get; set; }
        /// <summary>
        /// 押金
        /// </summary>
        public decimal Deposit { get; set; }
        /// <summary>
        /// 是否主卡
        /// </summary>
        public bool IsPrincipal { get; set; }
    }
}
