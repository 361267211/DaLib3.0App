using Grpc.Core;
using SmartLibrary.Assembly.Application.Protos;
using SmartLibrary.Open.EntityFramework.Core.Entitys.Assembly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Application.GrpcAppServices
{
   public interface IAssemblyDeliveryService
    {
        /// <summary>
        /// 将文献传递到开放中心
        /// </summary>
        /// <param name="entryPointId"></param>
        /// <returns></returns>
        Task<DeliverySharedAssemblyReply> DeliverySharedAssembly(DeliverySharedAssemblyRequest request, ServerCallContext context);
    }
}
