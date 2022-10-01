/*********************************************************
* 名    称：EnumOrderStatus.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：订单状态枚举
* 更新历史：
*
* *******************************************************/
using System.ComponentModel;

namespace SmartLibrary.ScoreCenter.Application.Services.Enum
{
    /// <summary>
    /// 订单状态枚举
    /// </summary>
    public enum EnumOrderStatus
    {
        [Description("待出库")]
        待出库 = 0,
        [Description("已出库")]
        已出库 = 1,
    }
}
