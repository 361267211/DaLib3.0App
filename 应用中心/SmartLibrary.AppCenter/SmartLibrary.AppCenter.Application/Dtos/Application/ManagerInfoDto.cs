using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Dtos.Application
{
    /// <summary>
    /// 管理员信息
    /// </summary>
    public class ManagerInfoDto
    {
        /// <summary>
        /// 唯一ID
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
        /// 卡号
        /// </summary>
        public string Card { get; set; }

        /// <summary>
        /// 账号类型
        /// </summary>
        public string ShowAccountType { get; set; }

        /// <summary>
        /// 账号类型
        /// </summary>
        public string AccountType { get; set; }

        /// <summary>
        /// 账号状态
        /// </summary>
        public string ShowAccountStatus { get; set; }

        /// <summary>
        /// 账号状态
        /// </summary>
        public int AccountStatus { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public string ExpireDate { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public string LastLoginDate { get; set; }

        /// <summary>
        /// 管理角色
        /// </summary>
        public string ManagerRoleString { get; set; }

        /// <summary>
        /// 管理角色ID
        /// </summary>
        public string ManagerRoleIds { get; set; }
    }
}
