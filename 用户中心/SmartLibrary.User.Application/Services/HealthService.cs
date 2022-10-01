
/*********************************************************
* 名    称：HealthService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20210831
* 描    述：grpc服务心跳检查
* 更新历史：
*
* *******************************************************/
using Furion.DependencyInjection;
using Grpc.Core;
using Grpc.Health.V1;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application
{
    public class HealthService : Health.HealthBase, IHealthService, IScoped
    {
        public override Task<HealthCheckResponse> Check(HealthCheckRequest request, ServerCallContext context)
        {
            //TODO:检查逻辑
            return Task.FromResult(new HealthCheckResponse() { Status = HealthCheckResponse.Types.ServingStatus.Serving });
        }

        public override async Task Watch(HealthCheckRequest request, IServerStreamWriter<HealthCheckResponse> responseStream, ServerCallContext context)
        {
            //TODO:检查逻辑
            await responseStream.WriteAsync(new HealthCheckResponse()
            { Status = HealthCheckResponse.Types.ServingStatus.Serving });
        }
    }
}