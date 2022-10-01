/*********************************************************
 * 名    称：Deployment
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/1 14:40:07
 * 描    述：应用部署环境
 *
 * 更新历史：
 *
 * *******************************************************/
using Furion.DatabaseAccessor;
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.Open.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 应用部署环境
    /// </summary>
    public class Deployment : Entity<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(100), Required]
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [StringLength(20), Required]
        public string Code { get; set; }

        /// <summary>
        /// 客户标识
        /// </summary>
        [StringLength(48), Required]
        public string CustomerId { get; set; }

        /// <summary>
        /// Api网关地址
        /// </summary>
        [StringLength(100), Required]
        public string ApiGateway { get; set; }

        /// <summary>
        /// Grpc网关地址
        /// </summary>
        [StringLength(100), Required]
        public string GrpcGateway { get; set; }

        /// <summary>
        /// 前台地址
        /// </summary>
        [StringLength(100)]
        public string WebGateway { get; set; }

        /// <summary>
        /// 后台地址
        /// </summary>
        [StringLength(100)]
        public string MgrGateway { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(500), Required]
        public string Desc { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [Required]
        public bool DeleteFlag { get; set; }

    }
}