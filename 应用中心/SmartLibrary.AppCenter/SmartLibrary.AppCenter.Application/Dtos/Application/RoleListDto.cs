using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Dtos.Application
{
    /// <summary>
    /// 角色列表
    /// </summary>
    public class RoleListDto
    {
        /// <summary>
        /// 角色id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 授权应用
        /// </summary>
        public string AuthApps {  get; set; }

        /// <summary>
        /// 添加日期
        /// </summary>
        public string CreateTime { get; set; }

    }
}
