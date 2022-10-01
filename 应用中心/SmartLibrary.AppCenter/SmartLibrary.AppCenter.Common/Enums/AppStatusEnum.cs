using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartLibrary.AppCenter.Common.Attributes;

namespace SmartLibrary.AppCenter.Common.Enums
{
    /// <summary>
    /// 应用状态 枚举
    /// </summary>
    public enum AppStatusEnum
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Enum("正常")]
        Normal =1,

        /// <summary>
        /// 下架
        /// </summary>
        [Enum("下架")]
        Off =2
    }
}
