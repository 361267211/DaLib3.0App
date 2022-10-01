using SmartLibrary.AppCenter.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Common.Enums
{
    /// <summary>
    /// 卡状态
    /// </summary>
    public enum EnumCardStatus
    {
        [Enum("正常")]
        正常 = 1,

        [Enum("挂失")]
        挂失 = 2,

        [Enum("停用")]
        停用 = 3,
    }
}
