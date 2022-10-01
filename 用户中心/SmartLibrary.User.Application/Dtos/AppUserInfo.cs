/*********************************************************
* 名    称：AppUserInfo.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户信息
* 更新历史：
*
* *******************************************************/
using System;
using System.Collections.Generic;

namespace SmartLibrary.User.Application.Dtos
{
    /// <summary>
    /// 读者信息
    /// </summary>
    public class AppUserInfo
    {
        /// <summary>
        /// UserKey
        /// </summary>

        public string UserKey { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserID { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string UserPhone { get; set; }
        /// <summary>
        /// 手机认证
        /// </summary>
        public bool MobileIdentity { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string UserEmail { get; set; }
        /// <summary>
        /// 邮箱认证
        /// </summary>
        public bool EmailIdentity { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string UserIdCard { get; set; }
        /// <summary>
        /// 身份证认证
        /// </summary>
        public bool IdCardIdentity { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 是否馆员
        /// </summary>
        public bool IsStaff { get; set; }


    }

    /// <summary>
    /// 读者后台权限
    /// </summary>
    public class AppUserPermission
    {
        /// <summary>
        /// UserKey
        /// </summary>
        public string UserKey { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserID { get; set; }
        public AppUserPermission()
        {
            RoleCodes = new List<string>();
            PermissionList = new List<string>();
        }
        /// <summary>
        /// 读者角色
        /// </summary>
        public List<string> RoleCodes { get; set; }
        /// <summary>
        /// 权限列表
        /// </summary>
        public List<string> PermissionList { get; set; }
        /// <summary>
        /// 是否开启敏感信息过滤
        /// </summary>
        public bool SensitiveFilter { get; set; } = true;
        /// <summary>
        /// 是否能看见敏感信息
        /// </summary>
        public bool SeeSensitiveInfo
        {
            get
            {
                if (SensitiveFilter)
                {
                    return RoleCodes.Contains("SensitiveInfoVisitor");
                }
                else
                {
                    return true;
                }
            }
        }
    }

    /// <summary>
    /// 读者前台权限
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
