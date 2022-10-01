/*********************************************************
* 名    称：EnumGoodsScoreRange.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分区间枚举
* 更新历史：
*
* *******************************************************/
using System.ComponentModel;

namespace SmartLibrary.ScoreCenter.Application.Services.Enum
{
    /// <summary>
    /// 积分区间枚举
    /// </summary>
    public enum EnumGoodsScoreRange
    {
        [Description("0 - 500")]
        R1 = 1,
        [Description("501 - 2000")]
        R2 = 2,
        [Description("2001 - 5000")]
        R3 = 3,
        [Description("5001 - 10000")]
        R4 = 4,
        [Description("10001及以上")]
        R5 = 5,
    }
}
