using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.User.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 读者信息权限配置
    /// </summary>
    public class InfoPermitReader : BaseEntity<Guid>
    {
        /// <summary>
        /// 权限类型 0:申领读者卡 1:完善个人信息
        /// </summary>
        public int ConfigType { get; set; }
        /// <summary>
        /// 读者类型 0:用户类型 1:用户组
        /// </summary>
        public int ReaderType { get; set; }
        /// <summary>
        /// 关联ID
        /// </summary>
        [StringLength(100)]
        public string RefID { get; set; }
    }
}
