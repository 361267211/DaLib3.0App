using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Furion;
using Furion.DependencyInjection;
using Furion.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OneOf;
using SmartLibrary.Open.Services.Search;
using SmartLibrary.Open.Services.SM.Internal;
using SmartLibrary.Search.EsSearchProxy.Core;
using SmartLibrary.Search.EsSearchProxy.Core.Interfaces;

namespace SmartLibrary.Open.Services
{
    public class ServicePartStartUp : AppStartup
    {
        public void ConfigureService(IServiceCollection services)
        {
            var allServices = typeof(SearchBoxItemService).Assembly.GetTypes()
                 .Where(x => !x.IsAbstract)
                 .Where(x => x.IsAssignableTo(typeof(IPrivateDependency)))
                 .SelectMany(x =>
                 {
                     var interfaces = x.GetInterfaces();
                     var serviceInterface =
                         interfaces.Where(y => !y.IsAssignableTo(typeof(IPrivateDependency))).ToArray();
                     var serviceLifetime = new[]
                         {
                            KeyValuePair.Create( typeof(ISingleton),ServiceLifetime.Singleton),
                            KeyValuePair.Create(     typeof(IScoped),ServiceLifetime.Scoped),
                           KeyValuePair.Create(  typeof(ITransient),ServiceLifetime.Transient)
                         }
                           .FirstOrDefault(y => interfaces.Contains(y.Key));
                     if (serviceLifetime.Key == null) throw new NotSupportedException("不支持的生命周期");
                     if (serviceInterface.Length == 0) serviceInterface = new[] { x };
                     return serviceInterface.Select(y => new ServiceDescriptor(y, x, serviceLifetime.Value));//类似于Autofac中的builder.RegisterType(x).AsImpleteService()的效果
                 });
            foreach (var item in allServices)
            {
                services.Add(item);
            }

            services.AddHttpContextAccessor();//如果有，则取消改行
            services.AddEsSearchProxy(x =>
            {

                x.SiteId = 996;
                x.SitePassword = "krs123456";
                x.SiteUserName = OneOf<string, Func<IServiceProvider, string>>.FromT1(y =>
                {
                    var httpContextAccessor = y.GetRequiredService<IHttpContextAccessor>();
                    var result = httpContextAccessor.HttpContext?.User.FindFirstValue("OrgCode");
                    return result ?? "krs";
                });
                x.EsApiBase = new Uri("http://essmartapi.cqvip.com");
                x.ConnectionTimeOut = TimeSpan.FromSeconds(45);

            });
        }
    }
}
