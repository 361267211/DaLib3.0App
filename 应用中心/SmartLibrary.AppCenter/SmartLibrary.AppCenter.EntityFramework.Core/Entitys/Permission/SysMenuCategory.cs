using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.EntityFramework.Core.Entitys.Permission
{
    /// <summary>
    /// 系统菜单分组
    /// </summary>
    public class SysMenuCategory : Entity<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>

        public string Code { get; set; }

        /// <summary>
        /// 默认展示
        /// </summary>

        public bool IsDefault { get; set; }

        /// <summary>
        /// 序号
        /// </summary>

        public int Sort { get; set; }

        /// <summary>
        /// 是否可见
        /// </summary>

        public bool Visible { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool DeleteFlag { get; set; }
    }
}
