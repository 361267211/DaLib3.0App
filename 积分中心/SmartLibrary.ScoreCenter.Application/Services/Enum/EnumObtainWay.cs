/*********************************************************
* 名    称：EnumObtainWay.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：奖品领取方式
* 更新历史：
*
* *******************************************************/
using System.ComponentModel;

namespace SmartLibrary.ScoreCenter.Application.Services.Enum
{
    /// <summary>
    /// 奖品领取方式
    /// </summary>
    public enum EnumObtainWay
    {
        [Description("到馆领取")]
        到馆领取 = 0,
        [Description("邮寄")]
        邮寄 = 1,
    }
}
