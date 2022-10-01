using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services.AppManagement.Redis
{
    public interface IRedisConnectionService
    {
        /// <summary>
        /// 通过缓存获取连接实例
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        ConnectionMultiplexer GetConnectionMultiplexer(string connectionString);
    }
}
