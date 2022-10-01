
using SmartLibrary.Open.Common.AssemblyBase;
namespace SmartLibrary.Open.Services.Dtos.Deployee
{
    /// <summary>
    /// 部署信息查询条件
    /// </summary>
    public class DeploymentTableQuery:TableQueryBase
    {
        /// <summary>
        /// 客户标识
        /// </summary>
        public string CustomerId { get; set; }
    }
}
