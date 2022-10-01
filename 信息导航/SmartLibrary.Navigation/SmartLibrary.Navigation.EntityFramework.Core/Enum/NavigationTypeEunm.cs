using SmartLibrary.Navigation.Utility.EnumTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.Application.Enums
{
    /// <summary>
    /// 名    称：NavigationTypeEunm
    /// 作    者：张泽军
    /// 创建时间：2021/10/10 10:01:31
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public enum NavigationTypeEunm
    {
        /// <summary>
        /// 内容导航 
        /// </summary>
        [EnumAttribute("内容导航")]
        Content = 0,

        /// <summary>
        /// 关联目录 
        /// </summary>
        [EnumAttribute("关联目录")]
        Associated = 1,

        /// <summary>
        /// 外部链接 
        /// </summary>
        [EnumAttribute("外部链接")]
        OutLink = 2
    }
}
