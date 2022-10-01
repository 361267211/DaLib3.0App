/*********************************************************
* 名    称：IRedisConnectionService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：Redis连接服务
* 更新历史：
*
* *******************************************************/
using StackExchange.Redis;

namespace SmartLibrary.ScoreCenter.Common.Services
{
    /// <summary>
    /// Redis连接服务
    /// </summary>
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
