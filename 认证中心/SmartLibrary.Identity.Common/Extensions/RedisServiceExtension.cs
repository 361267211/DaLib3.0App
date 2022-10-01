using Microsoft.Extensions.DependencyInjection;
using SmartLibrary.Identity.Common.Dtos;
using SmartLibrary.Identity.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Common.Extensions
{
    public static class RedisServiceExtension
    {
        public static IServiceCollection AddRedisService(this IServiceCollection services, Action<RedisServiceOption> action)
        {
            services.Configure<RedisServiceOption>(action);
            //添加RedisConnectionService
            services.AddSingleton<IRedisConnectionService, RedisConnectionService>();
            //添加RedisService
            services.AddScoped<IRedisService, RedisService>();
            return services;
        }
    }
}
