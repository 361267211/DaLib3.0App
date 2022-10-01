/*********************************************************
* 名    称：EnumGoodsType.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：奖品类型枚举
* 更新历史：
*
* *******************************************************/
using System.ComponentModel;

namespace SmartLibrary.ScoreCenter.Application.Services.Enum
{
    /// <summary>
    /// 奖品类型枚举
    /// </summary>
    public enum EnumGoodsType
    {
        [Description("实物奖品")]
        实物奖品 = 0,
        [Description("虚拟奖品")]
        虚拟奖品 = 1,
    }
}
