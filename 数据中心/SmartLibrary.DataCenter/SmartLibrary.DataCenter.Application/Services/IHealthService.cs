
using Grpc.Core;
using Grpc.Health.V1;
using System.Threading.Tasks;

namespace SmartLibrary.DataCenter.Application
{
    public interface IHealthService
    {
        Task<HealthCheckResponse> Check(HealthCheckRequest request, ServerCallContext context);

        Task Watch(HealthCheckRequest request, IServerStreamWriter<HealthCheckResponse> responseStream, ServerCallContext context);
    }
}