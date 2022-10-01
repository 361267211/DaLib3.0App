using SmartLibrary.Navigation.Utility.EnumTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.Application.Enums
{
    /// <summary>
    /// 名    称：SideListEnum
    /// 作    者：张泽军
    /// 创建时间：2021/10/8 14:46:54
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public enum SideListEnum
    {
        /// <summary>
        /// 无
        /// </summary>
        [EnumAttribute("无")]
        None = 0,

        /// <summary>
        /// 左侧显示同标签栏目
        /// </summary>
        [EnumAttribute("左侧显示同标签栏目")]
        SameLableColumn = 1,

        /// <summary>
        /// 左侧显示栏目内容
        /// </summary>
        [EnumAttribute("左侧显示栏目内容")]
        SameColumnNavigationContent = 2
    }
}
