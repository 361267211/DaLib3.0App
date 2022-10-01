/*********************************************************
* 名    称：UserRoleOutput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户角色Id
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
    public class UserRoleOutput
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserID { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        public Dictionary<string, Guid> Roles { get; set; }
    }
}
