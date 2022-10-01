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
using SmartLibrary.Core.GrpcClientHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.DatabaseTerrace.Application.Interceptors
{
    public class GrpcClientResolver : IScoped, IGrpcClientResolver
    {
        private readonly IGrpcTargetAddressResolver _grpcTargetAddressResolver;
        private readonly IHttpContextAccessor _httpContextAccessor;


        private readonly GrpcChannelPool _channelPool = new GrpcChannelPool();
        public GrpcClientResolver(IGrpcTargetAddressResolver grpcTargetAddressResolver, IHttpContextAccessor httpContextAccessor)
        {
            _grpcTargetAddressResolver = grpcTargetAddressResolver;
            _httpContextAccessor = httpContextAccessor;
        }


        public TClient EnsureClient<TClient>() where TClient : ClientBase<TClient>
        {
            var uri = new Uri(this._grpcTargetAddressResolver.GetGrpcTargetAddress(this._httpContextAccessor.HttpContext.User.FindFirst(e => e.Type == "OrgCode").Value));
            var token = this._httpContextAccessor.HttpContext?.Request.Headers["Authorization"];
            var invoker = _channelPool.GetChannel(uri).Intercept(x =>
            {
                var tmp = x ?? new Metadata();
                tmp.Add("Authorization", $"{token}");

                return tmp;
            });
            return Activator.CreateInstance(typeof(TClient), invoker) as TClient;
        }
    }
}
