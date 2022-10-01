using Furion.DependencyInjection;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Filter
{
    /// <summary>
    /// 全局异常输出日志
    /// </summary>
    public class LogExceptionHandler : IGlobalExceptionHandler, ISingleton
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            Log.Error("异常信息：" + context.Exception?.Message);
            Log.Error("异常堆栈：" + context.Exception?.StackTrace);
            Console.WriteLine("异常信息：" + context.Exception?.Message);
            Console.WriteLine("异常堆栈：" + context.Exception?.StackTrace);

            return Task.CompletedTask;
        }
    }
}
