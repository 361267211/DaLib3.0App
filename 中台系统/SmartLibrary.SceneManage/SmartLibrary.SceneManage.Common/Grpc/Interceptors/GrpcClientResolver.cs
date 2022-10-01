/*********************************************************
 * 名    称：IgnoreMappingAttribute
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：自定义特性 添加了此特性的属性不会被配置到siteglobalconfig上。
 *
 * 更新历史：
 *
 * *******************************************************/

using Furion.DependencyInjection;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using SmartLibrary.Core.GrpcClientHelper;
using SmartLibrary.SceneManage.Common.Dtos;
using SmartLibraryAppRoute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static SmartLibraryAppRoute.AppRouteGrpcService;

namespace SmartLibrary.SceneManage.Common
{
    public class GrpcClientResolver : IScoped, IGrpcClientResolver
    {
        private readonly IGrpcTargetAddressResolver _grpcTargetAddressResolver;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDistributedCache _distributedCache;


        private readonly GrpcChannelPool _channelPool = new GrpcChannelPool();
        public GrpcClientResolver(IHttpContextAccessor httpContextAccessor
            , Func<string, IScoped, object> resolveNamed
            , IDistributedCache distributedCache)
        {
            _grpcTargetAddressResolver = resolveNamed("CustomGrpcTargetResolver", default) as IGrpcTargetAddressResolver;
            _httpContextAccessor = httpContextAccessor;
            _distributedCache = distributedCache;
        }


        public TClient EnsureClient<TClient>(string serviceName) where TClient : ClientBase<TClient>
        {
            var tenantCode = _httpContextAccessor.HttpContext.User.FindFirst(e => e.Type == "OrgCode")?.Value;
            var uri = new Uri(SiteGlobalConfig.GrpcRegist.CloudUrl);//string.IsNullOrEmpty(serviceName) ? new Uri(SiteGlobalConfig.GrpcRegist.CloudUrl) : GetUriByTenantCode(tenantCode, serviceName).Result;
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"];
            var invoker = _channelPool.GetChannel(uri).Intercept(x =>
            {
                var tmp = x ?? new Metadata();
                tmp.Add("Authorization", $"{token}");

                return tmp;
            });
            return Activator.CreateInstance(typeof(TClient), invoker) as TClient;
        }
        private async Task<AppRouteListReply> GetRouteList(string tenantCode)
        {
            var cachedRouteList = await _distributedCache.GetStringAsync($"{tenantCode}_RouteList");
            if (string.IsNullOrEmpty(cachedRouteList))
            {
                var result = new AppRouteListReply();
                var grpcClient = EnsureClient<AppRouteGrpcServiceClient>(nameof(AppRouteGrpcService));
                result = await grpcClient.GetAppRouteListAsync(new AppRouteListRequest
                {
                    TenantCode = tenantCode
                });
                await _distributedCache.SetStringAsync($"{tenantCode}_RouteList", JsonSerializer.Serialize(result), new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) });

                return result;
            }
            else
            {
                return JsonSerializer.Deserialize<AppRouteListReply>(cachedRouteList);
            }
            
        }

        private async Task<Uri> GetUriByTenantCode(string tenantCode,string appRouteCode)
        {
            var routeList = await GetRouteList(tenantCode);
            var currentTenantRouteList = routeList.TenantRouteList.FirstOrDefault(p => p.TenantCode == tenantCode).AppRouteList;
            var currentServiceRoute = currentTenantRouteList.FirstOrDefault(p => p.AppRouteCode == appRouteCode);
            return new Uri(currentServiceRoute.GrpcApiGateway);
        }
    }
}
