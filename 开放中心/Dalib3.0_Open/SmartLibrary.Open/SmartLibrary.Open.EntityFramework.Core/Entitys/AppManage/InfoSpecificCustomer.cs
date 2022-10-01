/*********************************************************
 * 名    称：InfoSpecificCustomer
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/1 14:40:07
 * 描    述：消息特定客户
 *
 * 更新历史：
 *
 * *******************************************************/

using Furion.DatabaseAccessor;
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.Open.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 消息特定客户
    /// </summary>
    public class InfoSpecificCustomer:Entity<Guid>
    {
        /// <summary>
        /// 活动消息标识
        /// </summary>
        [StringLength(48), Required]
        public string ActInfoId { get; set; }

        /// <summary>
        /// 客户标识
        /// </summary>
        [StringLength(48), Required]
        public string CustomerId { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [Required]
        public bool DeleteFlag { get; set; }

    }
}