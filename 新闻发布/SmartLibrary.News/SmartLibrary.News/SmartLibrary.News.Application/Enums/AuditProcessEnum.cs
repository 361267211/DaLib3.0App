using SmartLibrary.News.Utility.EnumTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application.Enums
{
    /// <summary>
    /// 名    称：AuditProcessEnum
    /// 作    者：张泽军
    /// 创建时间：2021/9/17 11:19:40
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：栏目新闻发布流程
    /// </summary>
    public enum AuditProcessEnum
    {
        /// <summary>
        /// 撰稿
        /// </summary>
        [EnumAttribute("撰稿")]
        Edit = 0,
        /// <summary>
        /// 提交 权限同撰稿
        /// </summary>
        [EnumAttribute("提交")]
        Submit = 1,
        /// <summary>
        /// 初审
        /// </summary>
        [EnumAttribute("初审")]
        PreliminaryAudit = 2,
        /// <summary>
        /// 初校
        /// </summary>
        [EnumAttribute("初校")]
        PreliminaryCheck = 3,
        /// <summary>
        /// 二审
        /// </summary>
        [EnumAttribute("二审")]
        SecondAudit = 4,
        /// <summary>
        /// 二校
        /// </summary>
        [EnumAttribute("二校")]
        SecondCheck = 5,
        /// <summary>
        /// 终审
        /// </summary>
        [EnumAttribute("终审")]
        FinallyAudit = 6,
        /// <summary>
        /// 终校
        /// </summary>
        [EnumAttribute("终校")]
        FinallyCheck = 7,
        /// <summary>
        /// 发布
        /// </summary>
        [EnumAttribute("发布")]
        Publish = 8
    }
}
