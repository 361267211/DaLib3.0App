using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.User.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 基础配置
    /// </summary>
    public class BasicConfigSet : BaseEntity<Guid>
    {
        /// <summary>
        /// 启用敏感信息过滤
        /// </summary>
        public bool SensitiveFilter { get; set; }
        /// <summary>
        /// 读者审批
        /// </summary>
        public bool UserInfoConfirm { get; set; }
        /// <summary>
        /// 属性审批
        /// </summary>
        public bool PropertyConfirm { get; set; }
        /// <summary>
        /// 是否可以认领读者卡
        /// </summary>
        public bool CardClaim { get; set; }
        /// <summary>
        /// 是否可以完善个人信息
        /// </summary>
        public bool UserInfoSupply { get; set; }
    }
}
