using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.User.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 属性管理
    /// </summary>
    public class Property : BaseEntity<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        /// <summary>
        /// 标识符
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Code { get; set; }
        /// <summary>
        /// 类型 0：文本 1：数值 2：时间 3：是非 4：属性组 5：地址 6：附件
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 描述信息
        /// </summary>
        [StringLength(50)]
        public string Intro { get; set; }
        /// <summary>
        /// 是否必填
        /// </summary>
        public bool Required { get; set; }
        /// <summary>
        /// 是否唯一
        /// </summary>
        public bool Unique { get; set; }
        /// <summary>
        /// 是否在列表展示
        /// </summary>
        public bool ShowOnTable { get; set; }

        /// <summary>
        /// 支持检索
        /// </summary>
        public bool CanSearch { get; set; }
        /// <summary>
        /// 读者属性
        /// </summary>
        public bool ForReader { get; set; }

        /// <summary>
        /// 卡属性
        /// </summary>
        public bool ForCard { get; set; }
        /// <summary>
        /// 系统内置
        /// </summary>
        public bool SysBuildIn { get; set; }
        /// <summary>
        /// 属性组ID
        /// </summary>
        public Guid? PropertyGroupID { get; set; }
        /// <summary>
        /// 数据状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 审批状态
        /// </summary>
        public int ApproveStatus { get; set; }
    }
}
