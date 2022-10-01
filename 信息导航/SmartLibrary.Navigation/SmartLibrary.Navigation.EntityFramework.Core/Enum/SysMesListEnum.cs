using SmartLibrary.Navigation.Utility.EnumTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.Application.Enums
{
    /// <summary>
    /// 名    称：SysMesListEnum
    /// 作    者：张泽军
    /// 创建时间：2021/10/8 14:35:52
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
        /// 列表显示访问击次数
        /// </summary>
        [EnumAttribute("列表显示访问击次数")]
        ListShowHitCount = 2,

        /// <summary>
        /// 列表显示信息摘要
        /// </summary>
        [EnumAttribute("列表显示信息摘要")]
        ListShowContentAttr = 3,

        /// <summary>
        /// 详情显示发布日期
        /// </summary>
        [EnumAttribute("详情显示发布日期")]
        DetailShowPublishDate = 4,

        /// <summary>
        /// 详情显示访问次数
        /// </summary>
        [EnumAttribute("详情显示访问次数")]
        DetailShowHitCount = 5
    }
}
