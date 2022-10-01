/*********************************************************
 * 名    称：AppNavigation
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/9/22 19:21:19
 * 描    述：导航栏目应用关联
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
    /// 导航栏目应用关联
    /// </summary>
    public class AppNavigation : Entity<Guid>
    {
        /// <summary>
        /// 导航栏目ID
        /// </summary>
        [Required, StringLength(100)]
        public string NavigationId { get; set; }

        /// <summary>
        /// 应用标识
        /// </summary>
        [StringLength(100), Required]
        public string AppId { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [Required]
        public int OrderIndex { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [Required]
        public bool DeleteFlag { get; set; }
    }
}
