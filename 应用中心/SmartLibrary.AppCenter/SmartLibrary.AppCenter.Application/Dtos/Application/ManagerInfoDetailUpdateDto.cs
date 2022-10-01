using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Dtos.Application
{
    /// <summary>
    /// 管理员授予角色
    /// </summary>
    public class ManagerInfoDetailUpdateDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserKey { get; set; }

        /// <summary>
        /// 是否是超级管理员
        /// </summary>
        public bool IsSuper { get; set; }

        /// <summary>
        /// 勾选的角色ID数组
        /// </summary>
        public List<string> CheckedRoleIds { get; set; }
    }
}
