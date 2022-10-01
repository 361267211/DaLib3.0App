using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Tasks.GrpcClientHelper;

namespace TaskManager.Tasks.Services
{
    public class GrpcClientResolver : IGrpcClientResolver
    {
        private readonly GrpcChannelPool _channelPool = new GrpcChannelPool();
        public TClient EnsureClient<TClient>(string fabioUrl, string token) where TClient : ClientBase<TClient>
        {
            var uri = new Uri(fabioUrl);
            var invoker = _channelPool.GetChannel(uri).Intercept(x =>
            {
                var tmp = x ?? new Metadata();
                tmp.Add("Authorization", $"Bearer {token}");

                return tmp;
            });
            return Activator.CreateInstance(typeof(TClient), invoker) as TClient;
        }
    }
}
