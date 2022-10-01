/*********************************************************
* 名    称：EnumMedalTriggerWay.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：勋章获取方式枚举
* 更新历史：
*
* *******************************************************/
using System.ComponentModel;

namespace SmartLibrary.ScoreCenter.Application.Services.Enum
{
    /// <summary>
    /// 勋章获取方式枚举
    /// </summary>
    public enum EnumMedalTriggerWay
    {
        [Description("发生次数")]
        发生次数 = 0,
        [Description("发生天数")]
        发生天数 = 1,
    }
}
