using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Furion.Reflection;

namespace SmartLibrary.Open.Services.SM.Internal
{
    public class GlobalDispatchProxy : AspectDispatchProxy, IDispatchProxy
    {
        /// <summary>
        /// 当前服务实例
        /// </summary>
        public object Target { get; set; }

        /// <summary>
        /// 服务提供器，可以用来解析服务，如：Services.GetService()
        /// </summary>
        public IServiceProvider Services { get; set; }

        /// <summary>
        /// 拦截方法
        /// </summary>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public override object Invoke(MethodInfo method, object[] args)
        {
            Console.WriteLine("SayHello 方法被调用了");

            var result = method.Invoke(Target, args);

            Console.WriteLine("SayHello 方法返回值：" + result);

            return result;
        }

        // 异步无返回值
        public override async Task InvokeAsync(MethodInfo method, object[] args)
        {
            Console.WriteLine("SayHello 方法被调用了");

            var task = method.Invoke(Target, args) as Task;
            await task;

            Console.WriteLine("SayHello 方法调用完成");
        }

        // 异步带返回值
        public override async Task<T> InvokeAsyncT<T>(MethodInfo method, object[] args)
        {
            Console.WriteLine("SayHello 方法被调用了");

            var taskT = method.Invoke(Target, args) as Task<T>;
            var result = await taskT;

            Console.WriteLine("SayHello 方法返回值：" + result);

            return result;
        }
    }
}

