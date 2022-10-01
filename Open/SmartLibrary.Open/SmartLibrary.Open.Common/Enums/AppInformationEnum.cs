using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Common.Enums
{
    /// <summary>
    /// 消息状态枚举
    /// </summary>
    public enum EnumInformationStatus
    {
        [Description("正常")]
        正常 = 1,

        [Description("置顶")]
        置顶 = 2
    }

    /// <summary>
    /// AppDynamic InfoType 枚举
    /// </summary>
    public enum EnumInfoType
    {
        [Description("应用动态")]
        应用动态 = 1,

        [Description("活动消息")]
        活动消息 = 2,

        [Description("使用教程")]
        使用教程 = 3
    }
}
