using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.EntityFramework.Core.Entitys.Permission
{
    /// <summary>
    /// 系统角色
    /// </summary>
    public class SysRole : EntityBase<Guid>
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
        /// 备注
        /// </summary>

        public string Remark { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>

        public bool DeleteFlag { get; set; }

    }
}
