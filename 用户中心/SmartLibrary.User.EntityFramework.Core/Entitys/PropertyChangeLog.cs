using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.User.EntityFramework.Core.Entitys
{
    public class PropertyChangeLog : BaseEntity<Guid>
    {
        /// <summary>
        /// 属性ID
        /// </summary>
        public Guid PropertyID { get; set; }
        /// <summary>
        /// 属性类型 0：属性 1：属性组
        /// </summary>
        public int PropertyType { get; set; }
        /// <summary>
        /// 修改类型
        /// </summary>
        public int ChangeType { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ChangeTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public Guid ChangeUserID { get; set; }
        /// <summary>
        /// 修改人姓名
        /// </summary>
        [StringLength(50)]
        public string ChangeUserName { get; set; }
        /// <summary>
        /// 修改人电话
        /// </summary>
        [StringLength(50)]
        public string ChangeUserPhone { get; set; }
        /// <summary>
        /// 审批状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
        public string Remark { get; set; }
        /// <summary>
        /// 变更内容
        /// </summary>
        [StringLength(500)]
        public string Content { get; set; }

    }

}
