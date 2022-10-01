/*********************************************************
* 名    称：UserGroupDelInput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户组删除读者
* 更新历史：
*
* *******************************************************/
using System;
using System.Collections.Generic;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 用户组删除读者
    /// </summary>
    public class UserGroupDelInput
    {
        public UserGroupDelInput()
        {
            UserIds = new List<Guid>();
        }
        public Guid GroupId { get; set; }
        public List<Guid> UserIds { get; set; }
    }
}
