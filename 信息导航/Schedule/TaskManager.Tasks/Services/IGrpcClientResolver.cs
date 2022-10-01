using Grpc.Core;
using Microsoft.Extensions.Configuration;

namespace TaskManager.Tasks.Services
{
    /// <summary>
    /// 创建grpc客户端解析器
    /// </summary>
    public interface IGrpcClientResolver
    {
        /// <summary>
        /// 获取GrpcClient
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <returns></returns>
        TClient EnsureClient<TClient>(string fabioUrl, string token) where TClient : ClientBase<TClient>;
    }
}
