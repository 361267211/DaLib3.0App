/*********************************************************
* 名    称：UserGroupDelDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户组删除数据
* 更新历史：
*
* *******************************************************/
using System;
using System.Collections.Generic;

namespace SmartLibrary.User.Application.Dtos.UserGroup
{
    /// <summary>
    /// 用户组删除数据
    /// </summary>
    public class UserGroupDelDto
    {
        public UserGroupDelDto()
        {
            UserIds = new List<Guid>();
        }
        /// <summary>
        /// 用户组Id
        /// </summary>
        public Guid GroupId { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public List<Guid> UserIds { get; set; }
    }
}
