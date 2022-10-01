using Furion.DependencyInjection;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.AspNetCore.Http;
using SmartLibrary.Core.GrpcClientHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Services.Grpc
{
    /// <summary>
    /// grpc客户端获取
    /// </summary>
    public class GrpcClientResolver : IGrpcClientResolver, IScoped
    {
        private readonly IGrpcTargetAddressResolver _GrpcTargetAddressResolver;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly GrpcChannelPool _ChannelPool = new();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resolveNamed"></param>
        /// <param name="httpContextAccessor"></param>
        public GrpcClientResolver(Func<string, IScoped, object> resolveNamed,
                                  IHttpContextAccessor httpContextAccessor)
        {
            _GrpcTargetAddressResolver = resolveNamed("AppCenterGrpcTargetResolver", default) as IGrpcTargetAddressResolver;
            _HttpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 获取grpc客户端
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <returns></returns>
        public TClient EnsureClient<TClient>() where TClient : ClientBase<TClient>
        {
            var orgCode = _HttpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "OrgCode")?.Value;
            var uri = new Uri(_GrpcTargetAddressResolver.GetGrpcTargetAddress(orgCode));
            const string authorizationHeaderName = "Authorization";
            var token = _HttpContextAccessor.HttpContext?.Request.Headers[authorizationHeaderName];

            var invoker = _ChannelPool.GetChannel(uri).Intercept(x =>
            {
                var tmp = x ?? new Metadata();
                if (token.HasValue)
                    tmp.Add(authorizationHeaderName, token);

                return tmp;
            });
            return Activator.CreateInstance(typeof(TClient), invoker) as TClient;
        }
    }
}
