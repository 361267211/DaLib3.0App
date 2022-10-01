/*********************************************************
* 名    称：EnumManualProcessStatus.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分奖惩状态枚举
* 更新历史：
*
* *******************************************************/
using System.ComponentModel;

namespace SmartLibrary.ScoreCenter.Application.Services.Enum
{
    /// <summary>
    /// 积分奖惩状态枚举
    /// </summary>
    public enum EnumManualProcessStatus
    {
        [Description("待处理")]
        待处理 = 0,
        [Description("处理中")]
        处理中 = 1,
        [Description("完成")]
        完成 = 2,
        [Description("失败")]
        失败 = -1,
    }
}
