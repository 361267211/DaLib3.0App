/*********************************************************
* 名    称：EnumValidTerm.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分有效期
* 更新历史：
*
* *******************************************************/
using System.ComponentModel;

namespace SmartLibrary.ScoreCenter.Application.Services.Enum
{
    /// <summary>
    /// 积分有效期
    /// </summary>
    public enum EnumValidTerm
    {
        [Description("永久")]
        永久 = 0,
        [Description("1年")]
        一年 = 1,
        [Description("2年")]
        两年 = 2,
        [Description("3年")]
        三年 = 3,
        [Description("4年")]
        四年 = 4,
        [Description("5年")]
        五年 = 5
    }
}
