using DotNetCore.CAP;
using Furion;
using Furion.DatabaseAccessor;
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartLibrary.Identity.Application.Dtos.Cap;
using SmartLibrary.Identity.Application.Services.Consts;
using SmartLibrary.Identity.Common.Const;
using SmartLibrary.Identity.Common.Dtos;
using SmartLibrary.Identity.EntityFramework.Core.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application.Filter
{
    public class CapActionEventFilter : IAsyncActionFilter, IOrderedFilter
    {
        private string MiniProfilerCategory => "CapEventFilter";
        public int Order => -999;

        /// <summary>
        /// 数据库上下文池
        /// </summary>
        private readonly IDbContextPool _dbContextPool;
        private readonly ICapPublisher _capPublisher;
        private readonly IdentityDbContext _dbContext;
        private readonly TenantInfo _tenantInfo;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContextPool"></param>
        /// <param name="capPublisher"></param>
        /// <param name="dbContext"></param>
        /// <param name="tenantInfo"></param>
        public CapActionEventFilter(IDbContextPool dbContextPool
            , ICapPublisher capPublisher
            , IdentityDbContext dbContext
            , TenantInfo tenantInfo)
        {
            _dbContextPool = dbContextPool;
            _capPublisher = capPublisher;
            _dbContext = dbContext;
            _tenantInfo = tenantInfo;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // 获取动作方法描述器
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var method = actionDescriptor.MethodInfo;

            // 判断是否贴有事件特性特性或者没有获取到租户信息
            if (!method.IsDefined(typeof(CapActionEventAttribute), true) || string.IsNullOrWhiteSpace(_tenantInfo.Name))
            {
                // 调用方法
                var resultContext = await next();
            }
            else
            {


                // 获取工作单元特性
                var capEventAttribute = method.GetCustomAttribute<CapActionEventAttribute>();
                // 打印工作单元开始消息
                App.PrintToMiniProfiler(MiniProfilerCategory, $"Beginning CapActionEventFilter {capEventAttribute.EventName}");

                var msgBody = new PublishEvent.ActionEventMsg
                {
                    TenantName = _tenantInfo.Name,
                    AppCode = SiteGlobalConfig.AppCode,
                    AppName = SiteGlobalConfig.AppName,
                    EventCode = capEventAttribute.EventCode,
                    EventName = capEventAttribute.EventName,
                    UserKey = ""
                };
                //配置启用事务
                if (capEventAttribute.UnitOfWork)
                {
                    // 开启事务
                    _dbContextPool.BeginTransaction(capEventAttribute.EnsureTransaction);
                    // 调用方法
                    var resultContext = await next();
                    // 获取UserKey
                    var userKey = App.User.FindFirst("UserKey") != null ? App.User.FindFirst("UserKey").Value : "";
                    msgBody.UserKey = userKey;
                    //没有异常，则投递消息
                    if (resultContext.Exception == null)
                    {
                        using (var tran = _dbContext.Database.BeginTransaction(_capPublisher, true))
                        {
                            await _capPublisher.PublishAsync(PublishEvent.ActionEvent, msgBody);
                        }
                    }
                    // 提交事务
                    _dbContextPool.CommitTransaction(false, resultContext.Exception);
                }
                else
                {
                    var resultContext = await next();
                    //执行后获取userkey
                    var userKey = App.User.FindFirst("UserKey") != null ? App.User.FindFirst("UserKey").Value : "";
                    msgBody.UserKey = userKey;
                    if (resultContext.Exception == null)
                    {
                        await _capPublisher.PublishAsync(PublishEvent.ActionEvent, msgBody);
                    }

                }

                // 打印工作单元结束消息
                App.PrintToMiniProfiler(MiniProfilerCategory, $"Ending CapActionEventFilter {capEventAttribute.EventName}");
            }
        }
    }
}
