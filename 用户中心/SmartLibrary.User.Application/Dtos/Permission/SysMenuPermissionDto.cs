/*********************************************************
* 名    称：SysMenuPermissionDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220302
* 描    述：权限管理菜单
* 更新历史：
*
* *******************************************************/
using System;
using System.Collections.Generic;

namespace SmartLibrary.User.Application.Dtos.Permission
{
    /// <summary>
    /// 权限管理菜单
    /// </summary>
    public class SysMenuPermissionDto
    {
        public SysMenuPermissionDto()
        {
            PermissionNodes = new List<SysMenuPermissionDto>();
        }
        /// <summary>
        /// 主键标识
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 父ID
        /// </summary>
        public string Pid { get; set; }

        /// <summary>
        /// 节点路径
        /// </summary>

        public string Path { get; set; }

        /// <summary>
        /// 全路径
        /// </summary>

        public string FullPath { get; set; }

        /// <summary>
        /// 名称
        /// </summary>

        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>

        public int Type { get; set; }

        /// <summary>
        /// 图标
        /// </summary>

        public string Icon { get; set; }

        /// <summary>
        /// 路由
        /// </summary>

        public string Router { get; set; }

        /// <summary>
        /// 组件
        /// </summary>

        public string Component { get; set; }

        /// <summary>
        /// 权限标识:接口的Action
        /// </summary>

        public string Permission { get; set; }

        ///// <summary>
        ///// 分组标识
        ///// </summary>

        //public string Category { get; set; }

        /// <summary>
        /// 打开方式
        /// </summary>

        public int OpenWay { get; set; }

        /// <summary>
        /// 是否可见
        /// </summary>

        public bool Visible { get; set; }

        /// <summary>
        /// 排序
        /// </summary>

        public int Sort { get; set; }

        /// <summary>
        /// 备注
        /// </summary>

        public string Remark { get; set; }

        /// <summary>
        /// 是否应用系统菜单
        /// </summary>

        public bool IsSysMenu { get; set; }
        /// <summary>
        /// 下级Permission
        /// </summary>
        public IEnumerable<SysMenuPermissionDto> PermissionNodes { get; set; }


    }
}
