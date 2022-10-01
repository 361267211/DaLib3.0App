using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services.Dtos.Deployee
{
    public class DeploymentDto
    {
        /// <summary>
        /// 部署信息ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "请输入名称")]
        [MaxLength(50, ErrorMessage = "名称最多输入50个字符")]
        public string Name { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        [Required(ErrorMessage = "请输入编码")]
        [MaxLength(20, ErrorMessage = "编码最多输入20个字符")]
        public string Code { get; set; }
        /// <summary>
        /// api网关地址
        /// </summary>
        [Required(ErrorMessage = "请输入Api网关地址")]
        [MaxLength(100, ErrorMessage = "Api网关地址最多输入100个字符")]
        public string ApiGateway { get; set; }
        /// <summary>
        /// Grpc网关地址
        /// </summary>
        [Required(ErrorMessage = "请输入grpc网关地址")]
        [MaxLength(100, ErrorMessage = "grpc网关地址最多输入100个字符")]
        public string GrpcGateway { get; set; }
        /// <summary>
        /// 描述信息
        /// </summary>
        [MaxLength(500, ErrorMessage = "描述信息最多输入500个字符")]
        public string Desc { get; set; }
    }
}
