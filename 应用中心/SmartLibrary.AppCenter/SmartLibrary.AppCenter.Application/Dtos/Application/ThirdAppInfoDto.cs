using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Dtos.Application
{
    /// <summary>
    /// 三方应用信息
    /// </summary>
    public class ThirdAppInfoDto
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// 应用类型，多选，存储ID串，逗号分隔
        /// </summary>
        public string AppType { get; set; }

        /// <summary>
        /// 开发者
        /// </summary>
        public string Developer { get; set; }

        /// <summary>
        /// 支持终端，多选，存储123，逗号分隔
        /// </summary>
        public string Terminal { get; set; }

        /// <summary>
        /// 添加日期
        /// </summary>
        public string CreateTime { get; set; }
    }
}
