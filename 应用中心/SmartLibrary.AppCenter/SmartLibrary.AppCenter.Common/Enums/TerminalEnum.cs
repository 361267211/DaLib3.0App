using SmartLibrary.AppCenter.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Common.Enums
{
    /// <summary>
    /// APP适用终端
    /// </summary>
    public enum TerminalEnum
    {
        /// <summary>
        /// PC端
        /// </summary>
        [Enum("PC端")]
        PC = 1,

        /// <summary>
        /// APP端
        /// </summary>
        [Enum("APP端")]
        APP = 2,

        /// <summary>
        /// 小程序端
        /// </summary>
        [Enum("小程序端")]
        SmallApp = 3,

        /// <summary>
        /// 自适应移动端
        /// </summary>
        [Enum("自适应移动端")]
        H5 = 4,

        /// <summary>
        /// 显示屏
        /// </summary>
        [Enum("显示屏")]
        SmartDevice = 5
    }
}
