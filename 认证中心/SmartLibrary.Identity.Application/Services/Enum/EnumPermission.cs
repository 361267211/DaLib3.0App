using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application.Services.Enum
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
