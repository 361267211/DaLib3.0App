using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 用户注册属性配置
    /// </summary>
    public class UserRegisterProperty : BaseEntity<Guid>
    {
        /// <summary>
        /// 属性编码
        /// </summary>
        [StringLength(200)]
        public string PropertyCode { get; set; }
        public bool IsEnable { get; set; }
        public bool IsCheck { get; set; }
    }
}
