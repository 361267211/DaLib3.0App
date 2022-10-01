using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Dtos.Application
{
    /// <summary>
    /// 管理员权限详情
    /// </summary>
    public class ManagerInfoDetailDto
    {
        /// <summary>
        ///用户 唯一ID
        /// </summary>
        public string UserKey { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 账号类型
        /// </summary>
        public string AccountType { get; set; }

        /// <summary>
        /// 账号类型
        /// </summary>
        public string ShowAccountType { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public string ExpireDate { get; set; }

        /// <summary>
        /// 是否是超级管理员
        /// </summary>
        public bool IsSuper { get; set; }

        /// <summary>
        /// 勾选的角色ID
        /// </summary>
        public List<string> CheckedRoleIds { get; set; }
    }
}
