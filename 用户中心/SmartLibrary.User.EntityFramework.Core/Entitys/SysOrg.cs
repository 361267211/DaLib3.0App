using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.User.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 组织架构
    /// </summary>
    public class SysOrg : BaseEntity<Guid>
    {
        /// <summary>
        /// 父级Id
        /// </summary>
        [StringLength(200)]
        public string Pid { get; set; }
        /// <summary>
        /// 节点Id
        /// </summary>
        public int Path { get; set; }
        /// <summary>
        /// 全路径
        /// </summary>
        [StringLength(200)]
        public string FullPath { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        /// <summary>
        /// 全称
        /// </summary>
        [StringLength(500)]
        public string FullName { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        [StringLength(50)]
        public string Code { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
        public string Remark { get; set; }
    }
}
