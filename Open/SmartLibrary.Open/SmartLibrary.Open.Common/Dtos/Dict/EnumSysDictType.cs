/*********************************************************
* 名    称：EnumSysDictType.cs
* 作    者：刘启平
* 联系方式：电话[13627622058],邮件[83379242@qq.com]
* 创建时间：2021013
* 描    述：字典类型枚举
* 更新历史：
*
* *******************************************************/
using System.ComponentModel;

namespace SmartLibrary.Open.Common.Dtos
{
    /// <summary>
    /// 字典类型枚举
    /// </summary>
    public enum EnumSysDictType
    {
        [Description("服务类型")]
        ServiceType = 1000,
        [Description("服务包")]
        ServicePack = 1001,
    }
}
