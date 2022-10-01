/*********************************************************
* 名    称：SysRoleMenuDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220302
* 描    述：角色权限关联
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.Dtos.Permission
{
    /// <summary>
    /// 角色权限
    /// </summary>
    public class SysRoleMenuDto
    {
        /// <summary>
        /// 角色ID
        /// </summary>

        public Guid RoleID { get; set; }

        /// <summary>
        /// 菜单权限ID
        /// </summary>

        public Guid MenuPermissionID { get; set; }
    }
}
