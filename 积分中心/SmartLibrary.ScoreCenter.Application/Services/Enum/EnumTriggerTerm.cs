/*********************************************************
* 名    称：EnumTriggerTerm.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：触发方式
* 更新历史：
*
* *******************************************************/
using System.ComponentModel;

namespace SmartLibrary.ScoreCenter.Application.Services.Enum
{
    /// <summary>
    /// 触发方式
    /// </summary>
    public enum EnumTriggerTerm
    {
        [Description("永久")]
        永久 = 0,
        [Description("每天")]
        每天 = 1,
        [Description("每周")]
        每周 = 2,
        [Description("每月")]
        每月 = 3,
        [Description("每年")]
        每年 = 4,
    }
}
