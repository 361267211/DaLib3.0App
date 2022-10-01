using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.EntityFramework.Core.Dto.Permission
{
    /// <summary>
    /// 
    /// </summary>
    public class SysRoleInfoDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        ///<example>foobar</example>
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

        /// <summary>
        /// 权限id
        /// </summary>
        public List<SysRoleMenuDto> Permissions { get; set; }

    }
}
