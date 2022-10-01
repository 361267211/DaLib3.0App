/*********************************************************
* 名    称：RedisServiceExtension.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：自定义RedisService中间件注入
* 更新历史：
*
* *******************************************************/
using Microsoft.Extensions.DependencyInjection;
using SmartLibrary.User.Common.Dtos;
using SmartLibrary.User.Common.Services;
using System;

namespace SmartLibrary.User.Common.Extensions
{
    /// <summary>
    /// 自定义RedisService中间件注入
    /// </summary>
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
