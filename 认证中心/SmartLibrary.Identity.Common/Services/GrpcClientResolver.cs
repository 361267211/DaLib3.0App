using Furion.DependencyInjection;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using SmartLibrary.Core.GrpcClientHelper;
using SmartLibrary.Identity.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Common.Services
{
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
