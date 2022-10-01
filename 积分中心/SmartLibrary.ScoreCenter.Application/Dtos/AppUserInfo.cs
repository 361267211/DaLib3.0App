/*********************************************************
* 名    称：AppUserInfo.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：用户信息
* 更新历史：
*
* *******************************************************/
using System.Collections.Generic;

namespace SmartLibrary.ScoreCenter.Application.Dtos
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class AppUserInfo
    {
        public string UserKey { get; set; }
        public string UserName { get; set; }
        public string UserPhoto { get; set; }
        public string UserPhone { get; set; }
        public string UserEmail { get; set; }
        public string UserIdCard { get; set; }
        public int Status { get; set; }
        public bool IsStaff { get; set; }
    }
    /// <summary>
    /// 用户权限信息
    /// </summary>
    public class AppUserPermission
    {
        public string UserKey { get; set; }
        public AppUserPermission()
        {
            RoleCodes = new List<string>();
            PermissionList = new List<string>();
        }
        public List<string> RoleCodes { get; set; }
        public List<string> PermissionList { get; set; }
    }
    /// <summary>
    /// 读者权限信息
    /// </summary>
    public class AppReaderPermission
    {
        /// <summary>
        /// UserKey
        /// </summary>
        public string UserKey { get; set; }
        /// <summary>
        /// 是否有权限
        /// </summary>
        public bool HasPermission { get; set; }
    }
}
