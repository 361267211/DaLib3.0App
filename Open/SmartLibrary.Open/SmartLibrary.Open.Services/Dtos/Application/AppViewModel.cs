using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services.Dtos
{
    /// <summary>
    /// 应用详情视图模型
    /// </summary>
    public class AppViewModel
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 应用名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 应用类型
        /// </summary>
        public int AppType { get; set; }
        /// <summary>
        /// 服务类型集合
        /// </summary>
        public List<string> ServiceTypes { get; set; }
        /// <summary>
        /// 服务包集合
        /// </summary>
        public List<string> ServicePacks { get; set; }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public string DevID { get; set; }
        /// <summary>
        /// 适用终端
        /// </summary>
        public List<string> Terminal { get; set; }
        /// <summary>
        /// 应用场景
        /// </summary>
        public List<string> Scene { get; set; }
        /// <summary>
        /// 应用简介
        /// </summary>
        public string Intro { get; set; }
        /// <summary>
        /// 应用介绍
        /// </summary>
        public string Desc { get; set; }

        public string Icon { get; set; }

        /// <summary>
        /// 是否免费试用
        /// </summary>
        public bool FreeTry { get; set; }
        /// <summary>
        /// 建议售价
        /// </summary>
        public decimal? AdvisePrice { get; set; }
        /// <summary>
        /// 定价类型,0:一次付清，1：月付费，2：年付费
        /// </summary>
        public int PriceType { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }
}
