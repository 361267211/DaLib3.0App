using Furion.DependencyInjection;
using SmartLibrary.AppCenter.Common.Dtos;
using SmartLibrary.Core.GrpcClientHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Services.Grpc
{
    /// <summary>
    /// 
    /// </summary>
    public class AppCenterGrpcTargetResolver : IGrpcTargetAddressResolver, IScoped
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgCode"></param>
        /// <returns></returns>
        public string GetGrpcTargetAddress(string orgCode)
        {
            //返回测试地址fabio网关，实际应该查询开放平台获取对应地址
            return SiteGlobalConfig.AppBaseConfig.GrpcGateway;
        }
    }
}
