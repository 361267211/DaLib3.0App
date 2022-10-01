using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Dtos.Application
{
    /// <summary>
    /// 导航栏目列表
    /// </summary>
    public class NavgationItemDto
    {
        /// <summary>
        /// 导航栏目ID
        /// </summary>
        public string NavId { get; set; }

        /// <summary>
        /// 导航栏目名称
        /// </summary>
        public string NavName { get; set; }

        /// <summary>
        /// 关联应用-逗号分隔字符串
        /// </summary>
        public string RelationApp { get; set; }

        /// <summary>
        /// 关联应用数量
        /// </summary>
        public int RelationAppCount { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public string CreateTime { get; set; }
    }
}
