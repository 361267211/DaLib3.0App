using SmartLibrary.News.Utility.EnumTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application.Enums
{
    /// <summary>
    /// 名    称：TerminalsDeliveryEnum
    /// 作    者：张泽军
    /// 创建时间：2021/9/10 15:59:44
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public enum TerminalsDeliveryEnum
    {
        [EnumAttribute("PC门户")]
        PC = 1,

        [EnumAttribute("微信小程序")]
        WechatApp = 2,

        [EnumAttribute("手机APP")]
        App = 3,

        [EnumAttribute("大厅查询机")]
        QueryMachine = 4
    }
}
