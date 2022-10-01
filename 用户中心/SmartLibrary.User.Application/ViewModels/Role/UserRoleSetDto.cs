/*********************************************************
* 名    称：UserRoleSetDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户角色设置
* 更新历史：
*
* *******************************************************/
using System;
using System.Collections.Generic;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 用户角色设置
    /// </summary>
    public class UserRoleSetDto
    {
        public UserRoleSetDto()
        {
            RoleIds = new List<Guid>();
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public List<Guid> RoleIds { get; set; }
    }
}
