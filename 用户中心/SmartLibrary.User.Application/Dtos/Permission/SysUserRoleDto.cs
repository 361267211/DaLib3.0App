/*********************************************************
* 名    称：SysUserRoleDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220302
* 描    述：用户角色关系
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.Dtos.Permission
{
    /// <summary>
    /// 用户角色关系
    /// </summary>
    public class SysUserRoleDto
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>

        public Guid UserID { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>

        public Guid RoleID { get; set; }
    }
}
