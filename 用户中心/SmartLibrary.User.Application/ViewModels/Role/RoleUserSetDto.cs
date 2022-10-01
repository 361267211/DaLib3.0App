/*********************************************************
* 名    称：RoleUserSetDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：角色用户输入
* 更新历史：
*
* *******************************************************/
using System;
using System.Collections.Generic;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 角色用户关系
    /// </summary>
    public class RoleUserSetDto
    {
        public RoleUserSetDto()
        {
            UserIds = new List<Guid>();
        }
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid RoleID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public List<Guid> UserIds { get; set; }
    }
}
