using SmartLibrary.News.Utility.EnumTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application.Enums
{
    /// <summary>
    /// 名    称：AuditPowerEunm
    /// 作    者：张泽军
    /// 创建时间：2021/9/17 10:48:47
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：新闻操作权限
    /// </summary>
    public enum AuditPowerEunm
    {
        /// <summary>
        /// 管理权限 管理权限具有其他所有权限
        /// </summary>
        [EnumAttribute("管理")]
        Manage = 0,
        /// <summary>
        /// 撰稿权限
        /// </summary>
        [EnumAttribute("撰稿")]
        Edit = 1,
        /// <summary>
        /// 初审权限
        /// </summary>
        [EnumAttribute("初审")]
        PreliminaryAudit = 2,
        /// <summary>
        /// 初校权限
        /// </summary>
        [EnumAttribute("初校")]
        PreliminaryCheck = 3,
        /// <summary>
        /// 二审权限
        /// </summary>
        [EnumAttribute("二审")]
        SecondAudit = 4,
        /// <summary>
        /// 二校权限
        /// </summary>
        [EnumAttribute("二校")]
        SecondCheck = 5,
        /// <summary>
        /// 终审权限
        /// </summary>
        [EnumAttribute("终审")]
        FinallyAudit = 6,
        /// <summary>
        /// 终校权限
        /// </summary>
        [EnumAttribute("终校")]
        FinallyCheck = 7,
        /// <summary>
        /// 发布权限
        /// </summary>
        [EnumAttribute("发布")]
        Publish = 8
    }
}
