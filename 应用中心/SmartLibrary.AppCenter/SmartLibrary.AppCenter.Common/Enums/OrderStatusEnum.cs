using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Common.Enums
{
    /// <summary>
    /// 订单状态
    /// </summary>
    public enum OrderStatusEnum
    {
        [Description("取消")]
        取消 = -2,
        [Description("驳回")]
        驳回 = -1,
        [Description("待审核")]
        待审核 = 0,
        [Description("正常")]
        正常 = 1,
        [Description("过期")]
        过期 = 2,
    }
}
