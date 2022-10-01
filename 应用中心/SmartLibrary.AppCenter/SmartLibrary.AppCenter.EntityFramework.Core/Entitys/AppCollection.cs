/*********************************************************
 * 名    称：AppCollection
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/9/16 13:40:01
 * 描    述：个人应用中心收藏
 *
 * 更新历史：
 *
 * *******************************************************/
using Furion.DatabaseAccessor;
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.AppCenter.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 个人应用中心收藏
    /// </summary>
    public class AppCollection : Entity<Guid>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [StringLength(50), Required]
        public string UserKey { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        [StringLength(50), Required]
        public string AppId { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [Required]
        public bool DeleteFlag { get; set; }
    }
}
