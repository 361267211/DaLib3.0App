/*********************************************************
* 名    称：EnumSourceFrom.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分奖惩读者来源方式
* 更新历史：
*
* *******************************************************/
using System.ComponentModel;

namespace SmartLibrary.ScoreCenter.Application.Services.Enum
{
    /// <summary>
    /// 积分奖惩读者来源方式
    /// </summary>
    public enum EnumSourceFrom
    {
        [Description("直接添加")]
        直接添加=0,
        [Description("规则查询")]
        规则查询=1,
    }
}
