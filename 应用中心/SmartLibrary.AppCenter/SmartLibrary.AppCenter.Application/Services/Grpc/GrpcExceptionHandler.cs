using Furion.DependencyInjection;
using Furion.FriendlyException;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Services.Grpc
{
    public class GrpcExceptionHandler : IGlobalExceptionHandler, ISingleton
    {

        public Task OnExceptionAsync(ExceptionContext context)
        {
            if (FlatterException(context.Exception) is RpcException rpc)
            {
                var trailer = rpc.Trailers.Get("serviceSideException");
                if (trailer != null) throw new RpcException(rpc.Status, trailer.Value);
            }
            throw context.Exception;
        }


        private static Exception FlatterException(Exception exception)
        {
            var result = exception;
            while (result is AggregateException aggregateException)
            {
                result = aggregateException.InnerException;
            }
            return result;
        }
    }
}
