using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Model.Entities
{
    /// <summary>
    /// 属性组
    /// </summary>
    [SqlSugar.SugarTable("PropertyGroup")]
    public class PropertyGroup
    {
        /// <summary>
        /// 标识
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 分组名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 类型 0:内置 1:扩展
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 系统内置，系统内置属性组的可选项固定
        /// </summary>
        public bool SysBuildIn { get; set; }
        /// <summary>
        /// 编码必填
        /// </summary>
        public bool RequiredCode { get; set; }
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
