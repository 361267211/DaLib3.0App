/*********************************************************
 * 名    称：AppRole
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/9/15 17:46:21
 * 描    述：应用权限角色
 *
 * 更新历史：
 *
 * *******************************************************/
using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.AppCenter.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 应用权限角色
    /// </summary>
    public class AppManager : Entity<Guid>
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        [StringLength(100), Required]
        public string ManageRoleId { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        [StringLength(100), Required]
        public string AppId { get; set; }

        /// <summary>
        /// 管理员类型
        /// 1：管理员，2：操作员，3：浏览者
        /// </summary>
        [Required]
        public int ManagerType { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [Required]
        public bool DeleteFlag { get; set; }
    }
}
