/*********************************************************
 * 名    称：ManageRole
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/9/15 14:57:45
 * 描    述：数据库模型ManageRole示例。
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
    /// 管理角色，用于应用中心管理权限分配
    /// </summary>
    public class ManagerRole : Entity<Guid>
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        [StringLength(50), Required]
        public string Name { get; set; }

        /// <summary>
        /// 角色说明
        /// </summary>
        [StringLength(200)]
        public string Remark { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [Required]
        public bool DeleteFlag { get; set; }
    }
}
