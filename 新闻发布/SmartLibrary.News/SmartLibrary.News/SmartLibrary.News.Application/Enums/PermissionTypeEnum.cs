using SmartLibrary.News.Utility.EnumTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application.Enums
{
    /// <summary>
    /// 名    称：PermissionTypeEnum
    /// 作    者：张泽军
    /// 创建时间：2021/10/19 8:59:37
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    /// <summary>
    /// 系统菜单类型
    /// </summary>
    public enum PermissionTypeEnum
    {
        /// <summary>
        /// 目录
        /// </summary>
        [EnumAttribute("目录")]
        Dir = 0,

        /// <summary>
        /// 菜单
        /// </summary>
        [EnumAttribute("菜单")]
        Menu = 1,

        /// <summary>
        /// 按钮
        /// </summary>
        [EnumAttribute("按钮")]
        Btn = 2,
        /// <summary>
        /// 查询
        /// </summary>
        [EnumAttribute("查询")]
        Query = 3,
        /// <summary>
        /// 操作
        /// </summary>
        [EnumAttribute("操作")]
        Operate = 4,
        /// <summary>
        /// 接口
        /// </summary>
        [EnumAttribute("接口")]
        Api = 5
    }
}
