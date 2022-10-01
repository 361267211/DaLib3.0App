using SmartLibrary.News.Utility.EnumTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application.Enums
{
    /// <summary>
    /// 名    称：SideListEnum
    /// 作    者：张泽军
    /// 创建时间：2021/9/15 14:42:02
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public enum SideListEnum
    {
        /// <summary>
        /// 左侧显示同标签栏目
        /// </summary>
        [EnumAttribute("左侧显示同标签栏目")]
        SameLableColumn = 1,

        /// <summary>
        /// 左侧显示新闻标签
        /// </summary>
        [EnumAttribute("左侧显示新闻标签")]
        SameColumnNewsLable = 2,

        /// <summary>
        /// 左侧显示新闻标签
        /// </summary>
        [EnumAttribute("左侧显示新闻标签")]
        IsShowAuditProcess = 7,
    }
}
