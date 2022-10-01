/*********************************************************
* 名    称：SyncUserGroupConst.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户数据同步常量参数
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.Dtos.UserGroup
{
    /// <summary>
    /// 用户数据同步参数
    /// </summary>
    public class SyncUserGroupConst
    {
        public const string JobName = "SyncUserGroupJob";
        public const string GroupName = "SyncUserGroup";
        public const string Cron = "0 0 0/2 * * ? *";
        public const string AssemblyFullName = "TaskManager.Tasks";
        public const string ClassFullName = "TaskManager.Tasks.Job.SyncUserGroupJob";
    }
    /// <summary>
    /// 用户组数据同步参数
    /// </summary>
    public class SyncUserGroupParamsDto
    {
        public Guid GroupID { get; set; }
    }
}
