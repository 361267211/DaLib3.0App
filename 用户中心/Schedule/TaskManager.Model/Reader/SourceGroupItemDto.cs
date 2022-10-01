using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Model.Reader
{
    public class SourceGroupItemDto
    {
        /// <summary>
        /// 属性组编码
        /// </summary>
        public string GroupCode { get; set; }
        /// <summary>
        /// 选项名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 选项编码
        /// </summary>
        public string Code { get; set; }
    }
}
