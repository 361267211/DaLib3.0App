/*********************************************************
* 名    称：AppGatewayEnsureService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：网关地址获取
* 更新历史：
*
* *******************************************************/
using Furion.DependencyInjection;
using SmartLibrary.AppCenter;
using SmartLibrary.ScoreCenter.Application.Dtos;
using SmartLibrary.ScoreCenter.Application.Services.Interface;
using SmartLibrary.ScoreCenter.Common.Services;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.Application.Services.Impl
{
    /// <summary>
    /// App网关地址获取
    /// </summary>
    public class AppGatewayEnsureService : IAppGatewayEnsureService, IScoped
    {
        private readonly IGrpcClientResolver _grpcClientResolver;
        /// <summary>
        /// 服务初始化
        /// </summary>
        /// <param name="grpcClientResolver"></param>
        public AppGatewayEnsureService(IGrpcClientResolver grpcClientResolver)
        {
            _grpcClientResolver = grpcClientResolver;
        }

        /// <summary>
        /// 通过appcode查询应用是否部署以及网关地址
        /// </summary>
        /// <param name="appcode"></param>
        /// <returns></returns>
        public async Task<AppGatewayResult> GetAppServiceAddress(string appcode)
        {
            var appCenterClient = _grpcClientResolver.EnsureClient<AppCenterGrpcService.AppCenterGrpcServiceClient>();
            var appGatewayRequest = new GetAppGateHostByCodeRequest { RouteCode = appcode ?? "" };
            var urlResult = await appCenterClient.GetAppGateHostByCodeAsync(appGatewayRequest);
            var result = new AppGatewayResult
            {
                InUse = !string.IsNullOrWhiteSpace(urlResult.GateHost),
                Gateway = !string.IsNullOrWhiteSpace(urlResult.GateHost) ? $"{urlResult.GateHost}/{appcode}/api" : ""
            };
            return result;
        }
    }
}
