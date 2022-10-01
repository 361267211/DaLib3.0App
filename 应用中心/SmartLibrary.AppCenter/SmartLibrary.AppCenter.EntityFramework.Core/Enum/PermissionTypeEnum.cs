using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.EntityFramework.Core.Enum
{
    /// <summary>
    /// 系统菜单类型
    /// </summary>
    public enum PermissionTypeEnum
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
        /// 按钮
        /// </summary>
        [Description("按钮")]
        Btn = 2,

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
