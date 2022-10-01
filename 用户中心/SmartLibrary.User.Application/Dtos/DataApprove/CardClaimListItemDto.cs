﻿/*********************************************************
* 名    称：CardClaimListItemDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220302
* 描    述：读者领卡行对象
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.Dtos.DataApprove
{
    /// <summary>
    /// 读者领卡行对象
    /// </summary>
    public class CardClaimListItemDto
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime ApplyTime { get; set; }
        /// <summary>
        /// 卡ID
        /// </summary>
        public Guid CardID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserID { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string UserPhone { get; set; }
        /// <summary>
        /// 院系
        /// </summary>
        public string UserCollege { get; set; }
        /// <summary>
        /// 申请卡号
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 审批状态
        /// </summary>
        public int Status { get; set; }
    }
}
