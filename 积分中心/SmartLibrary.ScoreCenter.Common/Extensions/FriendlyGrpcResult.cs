/*********************************************************
* 名    称：FriendlyGrpc.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：Grpc请求结果封装
* 更新历史：
*
* *******************************************************/
using System;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.Common.Extensions
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
