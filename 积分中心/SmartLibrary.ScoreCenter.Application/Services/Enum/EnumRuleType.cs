/*********************************************************
* 名    称：EnumRuleType.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：规则枚举
* 更新历史：
*
* *******************************************************/
using System.ComponentModel;

namespace SmartLibrary.ScoreCenter.Application.Services.Enum
{
    /// <summary>
    /// 规则枚举
    /// </summary>
    public enum EnumRuleType
    {
        [Description("用户组")]
        用户组 = 0,
        [Description("用户类型")]
        用户类型 = 1,
    }
}
