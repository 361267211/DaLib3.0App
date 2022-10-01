using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services.Dtos
{
    public class CustomerAppUsageViewModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 应用ID
        /// </summary>
        public Guid AppID { get; set; }
        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// 应用分支ID
        /// </summary>
        public Guid AppBranchID { get; set; }
        /// <summary>
        /// 应用分支名称
        /// </summary>
        public string AppBranchName { get; set; }

        /// <summary>
        /// 部署环境ID
        /// </summary>
        public Guid DeploymentID { get; set; }
        /// <summary>
        /// 部署环境名称
        /// </summary>
        public string DeploymentName { get; set; }
        /// <summary>
        /// 部署环境编码
        /// </summary>
        public string DeploymentCode { get; set; }
        /// <summary>
        /// 客户ID
        /// </summary>
        public Guid CustomerID { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 客户编码
        /// </summary>
        public string CustomerOwner { get; set; }

        /// <summary>
        /// 授权类型 1、正式授权 2、试用授权
        /// </summary>
        public int AuthType { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
