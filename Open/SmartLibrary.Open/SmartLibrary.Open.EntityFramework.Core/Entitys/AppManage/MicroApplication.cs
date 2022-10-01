/*********************************************************
 * 名    称：Application
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/1 14:40:07
 * 描    述：标准应用
 *
 * 更新历史：
 *
 * *******************************************************/


using System;
using Furion.DatabaseAccessor;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.Open.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 标准应用
    /// </summary>
    public class MicroApplication : Entity<Guid>
    {
        /// <summary>
        /// 应用名称
        /// </summary>
        [StringLength(100), Required]
        public string Name { get; set; }
        /// <summary>
        /// 服务类型 1、基础应用 2、资源服务 3、学术情报 4、读者服务 5、分析决策 6、管理与馆务 
        /// </summary>
        [StringLength(48), Required]
        public string ServiceType { get; set; }
        /// <summary>
        /// 开发者标识
        /// </summary>
        [StringLength(48), Required]
        public string DevId { get; set; }
        /// <summary>
        /// 适用终端 1-PC端、2-APP端、3-小程序端、4-自适应移动端、5-显示屏
        /// </summary>
        [Required]
        public string Terminal { get; set; }
        /// <summary>
        /// 使用场景  1-前台 2-后台 3-通用
        /// </summary>
        [StringLength(48), Required]
        public string UseScene { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        [StringLength(50), Required]
        public string Intro { get; set; }
        /// <summary>
        /// 描述信息
        /// </summary>
        [StringLength(1000), Required]
        public string Desc { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        [StringLength(50), Required]
        public string Icon { get; set; }
        /// <summary>
        /// 是否支持免费试用
        /// </summary>
        [Required]
        public bool FreeTry { get; set; }
        /// <summary>
        /// 建议定价
        /// </summary>
        [Required]
        public decimal AdvisePrice { get; set; }
        /// <summary>
        /// 定价类型 1-一次性收费 2-年费
        /// </summary>
        [Required]
        public int PriceType { get; set; }
        /// <summary>
        /// 状态 1-正常  2-下架
        /// </summary>
        [Required]
        public int Status { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [Required]
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// 路由标识
        /// </summary>
        [StringLength(50), Required]
        public string RouteCode { get; set; }
        /// <summary>
        /// 推荐分
        /// </summary>
        [Range(0, 10)]
        public int RecommendScore { get; set; }
    }
}