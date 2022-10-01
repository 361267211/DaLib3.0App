using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.GrpcService.ExceptionHandler
{
    /// <summary>
    /// 服务端拦截异常
    /// </summary>

    public class ServerSideExceptionInterceptor : Grpc.Core.Interceptors.Interceptor
    {
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            Grpc.Core.ServerCallContext context,
            Grpc.Core.UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                var result = await continuation.Invoke(request, context);
                return result;
            }
            catch (Exception e)
            {
                var innerException = FlatterException(e);
                var exceptionMetaData = new Grpc.Core.Metadata();
                exceptionMetaData.Add("exception", innerException.Message);
                context.ResponseTrailers.Add("exception", innerException.Message);

                throw innerException;
            }

        }

        /// <summary>
        /// 将异常信息展平
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        private Exception FlatterException(Exception exception)
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
