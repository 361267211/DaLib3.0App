using SmartLibrary.News.Utility.EnumTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application.Enums
{
    /// <summary>
    /// 名    称：NewsBodyTemplateTypeEnum
    /// 作    者：张泽军
    /// 创建时间：2021/9/16 14:42:02
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public enum NewsBodyTemplateTypeEnum
    {
        /// <summary>
        /// 头部
        /// </summary>
        [EnumAttribute("头部")]
        Head = 1,

        /// <summary>
        /// 尾部
        /// </summary>
        [EnumAttribute("尾部")]
        Foot = 2
    }
}
