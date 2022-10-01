/*********************************************************
* 名    称：EnumPermissionType.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：菜单权限枚举
* 更新历史：
*
* *******************************************************/
using System.ComponentModel;

namespace SmartLibrary.ScoreCenter.Application.Services.Enum
{
    /// <summary>
    /// 菜单权限类型
    /// </summary>
    public enum EnumPermissionType
    {
        /// <summary>
        /// 目录
        /// </summary>
        [Description("目录")]
        Dir = 0,

        /// <summary>
        /// 菜单
        /// </summary>
        [Description("菜单")]
        Menu = 1,

        /// <summary>
        /// 查询
        /// </summary>
        [Description("查询")]
        Query = 3,
        /// <summary>
        /// 操作
        /// </summary>
        [Description("操作")]
        Operate = 4,
        /// <summary>
        /// 接口
        /// </summary>
        [Description("接口")]
        Api = 5,
    }
}
