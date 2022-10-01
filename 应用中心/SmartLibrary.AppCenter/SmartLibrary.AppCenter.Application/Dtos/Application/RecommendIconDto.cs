using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Dtos.Application
{
    /// <summary>
    /// 推荐图标
    /// </summary>
    public class RecommendIconDto
    {
        /// <summary>
        /// 在线地址
        /// </summary>
        public string IconUrl { get; set; }

        /// <summary>
        /// 相对路径
        /// </summary>
        public string RelativeUrl { get; set; }
    }
}
