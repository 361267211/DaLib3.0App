using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Model.Reader
{
    public class PropertyGroupDto
    {
        public PropertyGroupDto()
        {
            Items = new List<PropertyGroupItemDto>();
        }
        /// <summary>
        /// 主键标识
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 系统内置则不可修改
        /// </summary>
        public bool SysBuildIn { get; set; }
        /// <summary>
        /// 编码必填
        /// </summary>
        public bool RequiredCode { get; set; }

        public List<PropertyGroupItemDto> Items { get; set; }
    }
}
