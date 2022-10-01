using SmartLibrary.News.Utility.EnumTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application.Enums
{
    /// <summary>
    /// 名    称：AuditStatusEnum
    /// 作    者：张泽军
    /// 创建时间：2021/9/10 13:09:07
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public enum AuditStatusEnum
    {
        /// <summary>
        /// 待提交
        /// </summary>
        [EnumAttribute("待提交", "提交")]
        UnSubmit = 0,

        /// <summary>
        /// 待初审
        /// </summary>
        [EnumAttribute("待初审", "初审")]
        UnPreliminaryAudit = 1,

        /// <summary>
        /// 待初校
        /// </summary>
        [EnumAttribute("待初校", "初校")]
        UnPreliminaryCheck = 2,

        /// <summary>
        /// 待二审
        /// </summary>
        [EnumAttribute("待二审", "二审")]
        UnSecondAudit = 3,

        /// <summary>
        /// 待二校
        /// </summary>
        [EnumAttribute("待二校", "二校")]
        UnSecondCheck = 4,

        /// <summary>
        /// 待终审
        /// </summary>
        [EnumAttribute("待终审", "终审")]
        UnFinallyAudit = 5,

        /// <summary>
        /// 待终校
        /// </summary>
        [EnumAttribute("待终校", "终校")]
        UnFinallyCheck = 6,

        /// <summary>
        /// 待发布
        /// </summary>
        [EnumAttribute("待发布", "发布")]
        UnPublish = 7,

        /// <summary>
        /// 已发布
        /// </summary>
        [EnumAttribute("已发布", "发布")]
        Published = 8,

        /// <summary>
        /// 已退回
        /// </summary>
        [EnumAttribute("已退回", "退回")]
        SendBack = 9,

        /// <summary>
        /// 已下架
        /// </summary>
        [EnumAttribute("已下架", "已下架")]
        OffShelf = 10
    }
}
