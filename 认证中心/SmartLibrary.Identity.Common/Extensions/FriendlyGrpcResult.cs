using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Common.Extensions
{
    public class GrpcResult<T>
    {
        public bool Succ { get; set; }
        public T Data { get; set; }
        public Exception Exception { get; set; }
    }
    public static class FriendlyGrpc<T> where T : class
    {
        public static async Task<GrpcResult<T>> WrapResultAsync(Func<Task<T>> func)
        {
            try
            {
                if (func != null)
                {
                    var tResult =await func();
                    return new GrpcResult<T>
                    {
                        Succ = true,
                        Data = tResult,
                        Exception = null
                    };
                }
                else
                {
                    return new GrpcResult<T>
                    {
                        Succ = false,
                        Data = default(T),
                        Exception = new Exception("无执行方法")
                    };
                }
            }
            catch (Exception ex)
            {
                return new GrpcResult<T>
                {
                    Succ = false,
                    Data = default(T),
                    Exception = new Exception($"方法执行异常:{ex.Message}")
                };
            }
        }
    }
}
