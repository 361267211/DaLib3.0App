using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Dtos.UserApplication
{
    /// <summary>
    /// 首页推荐应用
    /// </summary>
    public class RecommendAppMoreDto
    {
        /// <summary>
        /// 应用中心链接地址
        /// </summary>
        public string MoreUrl { get; set; }

        /// <summary>
        /// 推荐应用列表
        /// </summary>
        public List<RecommendAppDto> RecommendApps { get; set; }
    }
}
