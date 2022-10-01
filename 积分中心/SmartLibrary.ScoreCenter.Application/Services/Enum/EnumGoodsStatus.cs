/*********************************************************
* 名    称：EnumGoodsStatus.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：商品状态枚举
* 更新历史：
*
* *******************************************************/
using System.ComponentModel;

namespace SmartLibrary.ScoreCenter.Application.Services.Enum
{
    /// <summary>
    /// 商品状态枚举
    /// </summary>
    public enum EnumGoodsStatus
    {
        [Description("下架")]
        下架 = 0,
        [Description("上架")]
        上架 = 1,
    }
}
