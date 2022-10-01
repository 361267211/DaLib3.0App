/*********************************************************
* 名    称：AppOrderEnum.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：2021
* 描    述：订单枚举
* 更新历史：
*
* *******************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Common
{
    public enum EnumOrderAuthType
    {
        [Description("正式授权")]
        正式授权 = 1,
        [Description("试用授权")]
        试用授权 = 2,
    }

    public enum EnumOrderOpenType
    {
        [Description("首次授权")]
        首次授权 = 1,
        [Description("续费授权")]
        续费授权 = 2,
        [Description("试用延期")]
        试用延期 = 3,
    }

    public enum EnumOrderStatus
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

    public enum EnumOrderWay
    {

        [Description("中央授权")]
        中央授权 = 1,
        [Description("客户申请")]
        客户申请 = 2,
    }
}
