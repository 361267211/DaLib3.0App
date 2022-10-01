using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Model.Entities
{
    /// <summary>
    /// 属性组选项
    /// </summary>
    [SqlSugar.SugarTable("PropertyGroupItem")]
    public class PropertyGroupItem 
    {
        /// <summary>
        /// 标识
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 组ID
        /// </summary>
        public Guid GroupID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 数据状态 0:未激活 1:正常
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 审批状态 0:审批中 1:正常
        /// </summary>
        public int ApproveStatus { get; set; }
        /// <summary>
        /// 内置选项
        /// </summary>
        public bool SysBuildIn { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>
        public bool DeleteFlag { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset CreateTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTimeOffset UpdateTime { get; set; }

    }
}
