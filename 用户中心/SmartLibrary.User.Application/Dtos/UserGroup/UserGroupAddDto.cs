/*********************************************************
* 名    称：UserGroupAddDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户组添加
* 更新历史：
*
* *******************************************************/
using System;
using System.Collections.Generic;

namespace SmartLibrary.User.Application.Dtos.UserGroup
{
    /// <summary>
    /// 用户组添加
    /// </summary>
    public class UserGroupAddDto
    {
        public UserGroupAddDto()
        {
            UserIds = new List<Guid>();
        }
        /// <summary>
        /// 用户组ID
        /// </summary>
        public Guid GroupId { get; set; }
        /// <summary>
        /// 用户ID集合
        /// </summary>
        public List<Guid> UserIds { get; set; }
    }
}
