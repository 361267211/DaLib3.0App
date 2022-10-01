/*********************************************************
* 名    称：IGrpcClientResolver.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：GrpcClient解析器
* 更新历史：
*
* *******************************************************/
using Grpc.Core;

namespace SmartLibrary.ScoreCenter.Common.Services
{
    /// <summary>
    /// 创建grpc客户端的解析器
    /// </summary>
    public interface IGrpcClientResolver
    {
        TClient EnsureClient<TClient>() where TClient : ClientBase<TClient>;
    }
}
