/*********************************************************
 * 名    称：ActivityInfo
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/1 14:40:07
 * 描    述：活动消息
 *
 * 更新历史：
 *
 * *******************************************************/


using System;
using Furion.DatabaseAccessor;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SmartLibrary.Open.EntityFramework.Core.Entitys
{

    /// <summary>
    /// 活动消息
    /// </summary>
    public class ActivityInfo : Entity<Guid>
    {
        /// <summary>
        /// 是否公开消息
        /// </summary>
        [Required]
        [Comment("是否公开消息")]
        public bool IsPublic { get; set; }

        /// <summary>
        /// 消息标识
        /// </summary>
        [StringLength(48), Required]
        [Comment("消息标识")]
        public string InfoId { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [Required]
        [Comment("删除标识")]
        public bool DeleteFlag { get; set; }

    }
}