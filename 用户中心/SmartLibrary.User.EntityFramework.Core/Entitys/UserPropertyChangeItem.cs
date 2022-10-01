using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.User.EntityFramework.Core.Entitys
{
    public class UserPropertyChangeItem : BaseEntity<Guid>
    {
        /// <summary>
        /// 日志主记录
        /// </summary>
        public Guid LogID { get; set; }

        /// <summary>
        /// 读者ID
        /// </summary>
        public Guid UserID { get; set; }
        /// <summary>
        /// 是否字段
        /// </summary>
        public bool IsField { get; set; }
        /// <summary>
        /// 属性类型
        /// </summary>
        public int PropertyType { get; set; }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// 属性编码
        /// </summary>
        public string PropertyCode { get; set; }
        /// <summary>
        /// 旧值
        /// </summary>
        public string OldValue { get; set; }
        /// <summary>
        /// 新值
        /// </summary>
        public string NewValue { get; set; }
    }
}
