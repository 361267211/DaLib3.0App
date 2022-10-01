using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartLibrary.Open.Services.Dtos.Redis;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services.AppManagement.Redis
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


        public static async IAsyncEnumerable<StreamPosition> TryCreateConsumerGroup(this IDatabase database, StreamPosition[] positions, string consumerGroup, ILogger logger = null)
        {
            foreach (var position in positions)
            {
                var created = false;
                try
                {
                    var stream = position.Key;
                    var streamExist = await database.KeyExistsAsync(stream);
                    if (!streamExist)
                    {
                        if (await database.StreamCreateConsumerGroupAsync(stream, consumerGroup,
                            StreamPosition.NewMessages))
                        {
                            logger.LogInformation(
                                $"Redis stream [{position.Key}] created with consumer group [{consumerGroup}]");
                            created = true;
                        }
                    }
                    else
                    {
                        var groupInfo = await database.StreamGroupInfoAsync(stream);

                        if (groupInfo.All(g => g.Name != consumerGroup))
                        {
                            if (await database.StreamCreateConsumerGroupAsync(stream, consumerGroup,
                                StreamPosition.NewMessages))
                            {
                                logger.LogInformation(
                                    $"Redis stream [{position.Key}] created with consumer group [{consumerGroup}]");
                                created = true;
                            }
                        }
                        else
                        {
                            created = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger?.LogError(ex, $"Redis error while creating consumer group [{consumerGroup}] of stream [{position.Key}]");
                }

                if (created)
                    yield return position;
            }
        }


    }
}
