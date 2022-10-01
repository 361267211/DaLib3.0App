using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Dtos.Application
{
    /// <summary>
    /// 付费应用推荐
    /// </summary>
    public class AppPayDto
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
        /// 应用图标
        /// </summary>
        public string AppIcon { get; set; }

        /// <summary>
        /// 推荐指数
        /// </summary>
        public int Star { get; set; }

        /// <summary>
        /// 应用内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 指导价格
        /// </summary>
        public string Price { get; set; }

        /// <summary>
        /// 是否支持免费试用
        /// </summary>
        public bool IsFreeTry { get; set; }

        /// <summary>
        /// 开发商
        /// </summary>
        public string Developer { get; set; }
    }
}
