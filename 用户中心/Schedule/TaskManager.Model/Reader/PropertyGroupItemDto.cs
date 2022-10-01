using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Model.Reader
{
    public class PropertyGroupItemDto
    {
        /// <summary>
        /// 分组项
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 所属分组
        /// </summary>
        public Guid GroupID { get; set; }
        ///// <summary>
        ///// 父级ID
        ///// </summary>
        //public Guid ParentID { get; set; }
        /// <summary>
        /// 分组项目名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        ///// <summary>
        ///// 等级
        ///// </summary>
        //public int Level { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 审批状态
        /// </summary>
        public int ApproveStatus { get; set; }
        /// <summary>
        /// 系统内置
        /// </summary>
        public bool SysBuildIn { get; set; }
    }
}
