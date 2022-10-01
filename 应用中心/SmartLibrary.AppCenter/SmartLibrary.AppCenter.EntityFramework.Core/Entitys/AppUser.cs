/*********************************************************
 * 名    称：AppUser
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/9/15 18:18:20
 * 描    述：应用用户设置
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
    /// 用户权限设置
    /// </summary>
    public class AppUser : Entity<Guid>
    {
        /// <summary>
        /// 用户群标识
        /// </summary>
        [StringLength(100), Required]
        public string UserSetId { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        [StringLength(100), Required]
        public string AppId { get; set; }

        /// <summary>
        /// 用户集合类型
        /// 1：用户类型，2：用户分组
        /// </summary>
        [Required]
        public int UserSetType { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [Required]
        public bool DeleteFlag { get; set; }
    }
}
