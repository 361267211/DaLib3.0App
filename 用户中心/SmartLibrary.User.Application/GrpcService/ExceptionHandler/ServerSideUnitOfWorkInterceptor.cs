using Furion;
using Furion.DatabaseAccessor;
using Grpc.Core;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.GrpcService.ExceptionHandler
{
    public class ServerSideUnitOfWorkInterceptor : Grpc.Core.Interceptors.Interceptor
    {
        /// <summary>
        /// MiniProfiler分类名
        /// </summary>
        private const string MiniProfilerCategory = "unitOfWorkGrpc";

        private readonly IDbContextPool _dbContextPool;
        public ServerSideUnitOfWorkInterceptor(IDbContextPool dbContextPool)
        {
            _dbContextPool = dbContextPool;
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            var resultContext = default(TResponse);
            //调用方法
            var method = continuation.Method;
            //判断是否手动提交
            var isManualSaveChanges = method.IsDefined(typeof(ManualCommitAttribute), true);
            //判断是否贴有工作单元

            // 判断是否贴有工作单元特性
            if (!method.IsDefined(typeof(UnitOfWorkAttribute), true))
            {
                // 调用方法
                resultContext = await continuation.Invoke(request, context);
                try
                {
                    // 判断是否异常，并且没有贴 [ManualSaveChanges] 特性
                    if (!isManualSaveChanges) _dbContextPool.SavePoolNow();
                }
                catch
                {
                    //DoNothing
                }

            }
            else
            {
                // 打印工作单元开始消息
                App.PrintToMiniProfiler(MiniProfilerCategory, "Beginning");

                // 获取工作单元特性
                var unitOfWorkAttribute = method.GetCustomAttribute<UnitOfWorkAttribute>();

                try
                {
                    // 开启事务
                    _dbContextPool.BeginTransaction(unitOfWorkAttribute.EnsureTransaction);

                    // 调用方法
                    resultContext = await continuation.Invoke(request, context);

                    // 提交事务
                    _dbContextPool.CommitTransaction(isManualSaveChanges, null);
                }
                catch (Exception ex)
                {
                    // 打印工作单元结束消息
                    App.PrintToMiniProfiler(MiniProfilerCategory, "Ending With Exception");
                    // 提交事务
                    _dbContextPool.CommitTransaction(isManualSaveChanges, ex);
                }


                // 打印工作单元结束消息
                App.PrintToMiniProfiler(MiniProfilerCategory, "Ending");
            }

            // 手动关闭
            _dbContextPool.CloseAll();
            //返回结果
            return resultContext;
        }
    }
}
