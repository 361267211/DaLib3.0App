using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services.Dtos.Deployee
{
    /// <summary>
    /// 部署信息视图模型
    /// </summary>
    public class DeploymentViewModel
    {
        /// <summary>
        /// 部署信息ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// api网关地址
        /// </summary>
        public string ApiGateway { get; set; }
        /// <summary>
        /// Grpc网关地址
        /// </summary>
        public string GrpcGateway { get; set; }

        /// <summary>
        /// 前台地址
        /// </summary>
        public string WebGateway { get; set; }
        /// <summary>
        /// 后台地址
        /// </summary>
        public string MgrGateway { get; set; }
        /// <summary>
        /// 描述信息
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
    }
}
