using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartLibrary.AppCenter.Common.Attributes;

namespace SmartLibrary.AppCenter.Common.Enums
{
    /// <summary>
    /// 采购类型，授权类型
    /// </summary>
    public enum PurchaseTypeEnum
    {
        /// <summary>
        /// 正式
        /// </summary>
        [Enum("正式")]
        Formal = 1,

        /// <summary>
        /// 试用
        /// </summary>
        [Enum("试用")]
        Try = 2
    }
}
