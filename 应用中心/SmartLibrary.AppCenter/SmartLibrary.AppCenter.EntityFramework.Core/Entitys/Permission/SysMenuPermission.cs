using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.EntityFramework.Core.Entitys.Permission
{
    /// <summary>
    /// 系统菜单及权限配置
    /// </summary>
    public class SysMenuPermission : Entity<Guid>
    {
        /// <summary>
        /// 父ID
        /// </summary>
        [Comment("父级id")]
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

        /// <summary>
        /// 分组标识
        /// </summary>

        public string Category { get; set; }

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
        /// 是否删除
        /// </summary>

        public bool DeleteFlag { get; set; }


    }
}
