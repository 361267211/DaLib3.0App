/*********************************************************
* 名    称：EnumScoreType.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分类型枚举
* 更新历史：
*
* *******************************************************/
using System.ComponentModel;

namespace SmartLibrary.ScoreCenter.Application.Services.Enum
{
    /// <summary>
    /// 积分类型枚举
    /// </summary>
    public enum EnumScoreType
    {
        [Description("减积分")]
        减积分 = -1,
        [Description("加积分")]
        加积分 = 1
    }
}
