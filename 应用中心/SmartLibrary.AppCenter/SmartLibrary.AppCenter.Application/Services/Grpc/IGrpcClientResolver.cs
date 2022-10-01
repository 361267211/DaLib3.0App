using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Services.Grpc
{
    public interface IGrpcClientResolver
    {
        /// <summary>
        /// 获取grpc客户端
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <returns></returns>
        TClient EnsureClient<TClient>() where TClient : ClientBase<TClient>;
    }
}
