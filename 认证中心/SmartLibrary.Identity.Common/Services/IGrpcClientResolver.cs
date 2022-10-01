using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Common.Services
{
    /// <summary>
    /// 创建grpc客户端的解析器
    /// </summary>
    public interface IGrpcClientResolver
    {
        TClient EnsureClient<TClient>() where TClient : ClientBase<TClient>;
    }
}
