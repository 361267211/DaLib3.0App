/*********************************************************
 * 名    称：Information
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/1 14:40:07
 * 描    述：应用信息
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
    /// 应用信息
    /// </summary>
    public class Information : Entity<Guid>
    {
        /// <summary>
        /// 消息标题
        /// </summary>
        [StringLength(100), Required]
        public string Title { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        [StringLength(4000), Required]
        public string Content { get; set; }

        /// <summary>
        /// 发布日期
        /// </summary>
        [Required]
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// 消息状态 1-正常  2-置顶
        /// </summary>
        [Required]
        public int Status { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [Required]
        public bool DeleteFlag { get; set; }

    }
}