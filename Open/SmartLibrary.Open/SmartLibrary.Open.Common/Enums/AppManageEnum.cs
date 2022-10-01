/*********************************************************
 * 名    称：AppStatusEnum
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/3 13:41:01
 * 描    述：应用状态枚举
 *
 * 更新历史：
 *
 * *******************************************************/

using SmartLibrary.Open.Common.Attributes;

namespace SmartLibrary.Open.Common.Enums
{
    /// <summary>
    /// 应用状态枚举
    /// </summary>
    public enum AppStatusEnum
    {
        [Enum("正常")]
        Normal = 1,
        
        [Enum("下架")]
        Disabled = 2
    }

    /// <summary>
    /// 应用适用场景枚举
    /// </summary>
    public enum AppSeceneTypeEnum
    {
        [Enum("前台")]
        Portal = 1,

        [Enum("后台")]
        Manage = 2,

        [Enum("前台,后台")]
        Universal = 3
    }


    /// <summary>
    /// 应用订购类型枚举
    /// </summary>
    public enum AppPurchaseTypeEnum
    {
        [Enum("正式授权")]
        Formal = 1,

        [Enum("试用授权")]
        Trial = 2
    }



    /// <summary>
    /// 适用终端 1-PC端、2-APP端、3-小程序端、4-自适应移动端、5-显示屏
    /// </summary>
    public enum AppTerminalTypeEnum
    {
        [Enum("PC")]
        Pc = 1,

        [Enum("APP")]
        App = 2,

        [Enum("小程序")]
        MicroApp = 3,

        [Enum("自适应移动端")]
        H5 = 4,

        [Enum("显示屏")]
        OtherScreen = 5
    }
}
