using SmartLibrary.News.Utility.EnumTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application.Enums
{
    /// <summary>
    /// 名    称：SysMesListEnum
    /// 作    者：张泽军
    /// 创建时间：2021/9/15 14:51:49
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public enum SysMesListEnum
    {
        /// <summary>
        /// 列表显示发布日期
        /// </summary>
        [EnumAttribute("列表显示发布日期")]
        ListShowPublishDate = 1,

        /// <summary>
        /// 列表显示新闻访问次数
        /// </summary>
        [EnumAttribute("列表显示新闻访问次数")]
        ListShowHitCount = 2,

        /// <summary>
        /// 列表显示摘要
        /// </summary>
        [EnumAttribute("列表显示摘要")]
        ListShowContentAttr = 3,

        /// <summary>
        /// 列表显示新闻标签
        /// </summary>
        [EnumAttribute("列表显示新闻标签")]
        ListShowNewsLable = 4,

        /// <summary>
        /// 详情显示发布日期
        /// </summary>
        [EnumAttribute("详情显示发布日期")]
        DetailShowPublishDate = 5,

        /// <summary>
        /// 详情显示访问次数
        /// </summary>
        [EnumAttribute("详情显示访问次数")]
        DetailShowHitCount = 6,

        /// <summary>
        /// 详情显示审核信息
        /// </summary>
        [EnumAttribute("详情显示审核信息")]
        DetailShowAudit = 7
    }
}
