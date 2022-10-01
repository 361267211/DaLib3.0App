using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.User.EntityFramework.Core.Entitys
{
    /// 系统菜单及权限配置
    public class SysMenuPermission : BaseEntity<Guid>
    {
        /// <summary>
        /// 编码
        /// </summary>
        [StringLength(20)]
        public string Code { get; set; }
        /// <summary>
        /// 父ID
        /// </summary>
        [Comment("父级id")]
        [StringLength(200)]
        public string Pid { get; set; }

        /// <summary>
        /// 节点路径
        /// </summary>
        [StringLength(20)]
        public string Path { get; set; }

        /// <summary>
        /// 全路径
        /// </summary>
        [StringLength(200)]
        public string FullPath { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>

        public int Type { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [StringLength(20)]
        public string Icon { get; set; }

        /// <summary>
        /// 路由
        /// </summary>
        [StringLength(50)]
        public string Router { get; set; }

        /// <summary>
        /// 组件
        /// </summary>
        [StringLength(50)]
        public string Component { get; set; }

        /// <summary>
        /// 权限标识:接口的Action
        /// </summary>
        [StringLength(100)]
        public string Permission { get; set; }


        /// <summary>
        /// 打开方式
        /// </summary>

        public int OpenWay { get; set; }

        /// <summary>
        /// 是否可见
        /// </summary>

        public bool Visible { get; set; }

        ///// <summary>
        ///// 排序
        ///// </summary>

        //public int Sort { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(100)]
        public string Remark { get; set; }

        /// <summary>
        /// 是否应用系统菜单
        /// </summary>

        public bool IsSysMenu { get; set; }

    }
}
