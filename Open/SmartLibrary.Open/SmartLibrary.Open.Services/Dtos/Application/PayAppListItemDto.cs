using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services.Dtos.Application
{
    public class PayAppListItemDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; }
        /// <summary>
        /// 应用图标
        /// </summary>
        public string AppIcon { get; set; }
        /// <summary>
        /// 推荐星级
        /// </summary>
        public int Star { get; set; }
        /// <summary>
        /// 应用介绍
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 参考价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 服务周期
        /// </summary>
        public int PriceType { get; set; }
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
