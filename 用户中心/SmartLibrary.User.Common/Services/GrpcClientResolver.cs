/*********************************************************
* 名    称：GrpcClientResolver.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：GrpcClient获取服务
* 更新历史：
*
* *******************************************************/
using Furion.DependencyInjection;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using SmartLibrary.Core.GrpcClientHelper;
using SmartLibrary.User.Common.Extensions;
using System;

namespace SmartLibrary.User.Common.Services
{
    /// <summary>
    /// Grpc获取服务
    /// </summary>
    public class GrpcClientResolver : IGrpcClientResolver, IScoped
    {
        private readonly IGrpcTargetAddressResolver _grpcTargetAddressResolver;
        private readonly IHttpContextAccessor _httpContextAccessor;


        private readonly GrpcChannelPool _channelPool = new GrpcChannelPool();
        public GrpcClientResolver(IGrpcTargetAddressResolver grpcTargetAddressResolver, IHttpContextAccessor httpContextAccessor)
        {
            _grpcTargetAddressResolver = grpcTargetAddressResolver;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 获取GrpcClient
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <returns></returns>
        public TClient EnsureClient<TClient>() where TClient : ClientBase<TClient>
        {
            var uri = new Uri(this._grpcTargetAddressResolver.GetGrpcTargetAddress(this._httpContextAccessor.EnsureOwner()));
            var token = this._httpContextAccessor.HttpContext?.Request.Headers[HeaderNames.Authorization];
            var invoker = _channelPool.GetChannel(uri).Intercept(x =>
            {
                var tmp = x ?? new Metadata();
                tmp.Add(HeaderNames.Authorization, token);

                return tmp;
            });
            return Activator.CreateInstance(typeof(TClient), invoker) as TClient;
        }
    }
}
