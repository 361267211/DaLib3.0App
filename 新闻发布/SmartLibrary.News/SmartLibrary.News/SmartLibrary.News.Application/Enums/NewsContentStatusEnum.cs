using SmartLibrary.News.Utility.EnumTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application.Enums
{
    /// <summary>
    /// 名    称：NewsContentStatusEnum
    /// 作    者：张泽军
    /// 创建时间：2021/9/14 11:00:52
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public enum NewsContentStatusEnum
    {
        [EnumAttribute("发布")]
        Publish = 1,

        [EnumAttribute("下架")]
        OffShelf = 2
    }
}
