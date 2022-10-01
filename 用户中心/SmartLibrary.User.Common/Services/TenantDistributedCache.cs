/*********************************************************
* 名    称：TenantDistributedCache.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：多租户分布式缓存，通过scope区分
* 更新历史：
*
* *******************************************************/
using Furion.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using SmartLibrary.User.Common.Extensions;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SmartLibrary.User.Common.Services
{
    /// <summary>
    /// 多租户分布式缓存，通过scope区分
    /// </summary>
    public class TenantDistributedCache : ITenantDistributedCache, IScoped
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _publicScope = "public-scope";
        public TenantDistributedCache(
            IHttpContextAccessor httpContextAccessor,
            IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
            _httpContextAccessor = httpContextAccessor;
        }

        private string TenantIdResolve(bool isPublic)
        {
            var tenantId = _publicScope;
            if (!isPublic)
            {
                tenantId = _httpContextAccessor.EnsureOwner();
            }
            return tenantId;
        }

        /// <summary>
        /// 获取缓存，默认从context中获取租户
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isPublic">是否从公共区域获取数据</param>
        /// <returns></returns>
        public byte[] Get(string key, bool isPublic = false)
        {
            var tenantId = this.TenantIdResolve(isPublic);
            return _distributedCache.Get($"{tenantId}:{key}");
        }

        /// <summary>
        /// 获取缓存，默认从context中获取租户
        /// </summary>
        /// <param name="key"></param>
        /// <param name="token"></param>
        /// <param name="isPublic">是否从公共区域获取数据</param>
        /// <returns></returns>
        public async Task<byte[]> GetAsync(string key, CancellationToken token = default, bool isPublic = false)
        {
            var tenantId = this.TenantIdResolve(isPublic);
            return await _distributedCache.GetAsync($"{tenantId}:{key}", token);
        }

        /// <summary>
        /// 刷新过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isPublic">是否从公共区域获取数据</param>
        public void Refresh(string key, bool isPublic = false)
        {
            var tenantId = this.TenantIdResolve(isPublic);
            _distributedCache.Refresh($"{tenantId}:{key}");
        }

        /// <summary>
        /// 刷新过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="token"></param>
        /// <param name="isPublic">是否从公共区域获取数据</param>
        /// <returns></returns>
        public async Task RefreshAsync(string key, CancellationToken token = default, bool isPublic = false)
        {
            var tenantId = this.TenantIdResolve(isPublic);
            await _distributedCache.RefreshAsync($"{tenantId}:{key}", token);
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isPublic">是否从公共区域获取数据</param>
        public void Remove(string key, bool isPublic = false)
        {
            var tenantId = this.TenantIdResolve(isPublic);
            _distributedCache.Remove($"{tenantId}:{key}");
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="token"></param>
        /// <param name="isPublic">是否从公共区域获取数据</param>
        /// <returns></returns>
        public async Task RemoveAsync(string key, CancellationToken token = default, bool isPublic = false)
        {
            var tenantId = this.TenantIdResolve(isPublic);
            await _distributedCache.RemoveAsync($"{tenantId}:{key}", token);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public void Set(string key, byte[] value, DistributedCacheEntryOptions options, bool isPublic = false)
        {
            var tenantId = this.TenantIdResolve(isPublic);
            _distributedCache.Set($"{tenantId}:{key}", value, options);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default, bool isPublic = false)
        {
            var tenantId = this.TenantIdResolve(isPublic);
            await _distributedCache.SetAsync($"{tenantId}:{key}", value, options, token);
        }

        public TResult Get<TResult>(string key, bool isPublic = false)
        {
            var byteValue = Get(key, isPublic);
            try
            {
                return (TResult)JsonSerializer.Deserialize(byteValue, typeof(TResult));
            }
            catch
            {
                return default(TResult);
            }
        }

        public async Task<TResult> GetAsync<TResult>(string key, CancellationToken token = default, bool isPublic = false)
        {
            var byteValue = await GetAsync(key, token, isPublic);
            try
            {
                return (TResult)JsonSerializer.Deserialize(byteValue, typeof(TResult));
            }
            catch
            {
                return default(TResult);
            }
        }

        public void Set<T>(string key, T value, DistributedCacheEntryOptions options, bool isPublic = false)
        {
            try
            {
                var byteVal = JsonSerializer.SerializeToUtf8Bytes(value);
                Set(key, byteVal, options, isPublic);
            }
            catch
            {
                //donothing
            }
        }

        public async Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options, CancellationToken token = default, bool isPublic = false)
        {
            try
            {
                var byteVal = JsonSerializer.SerializeToUtf8Bytes(value);
                await SetAsync(key, byteVal, options, token, isPublic);
            }
            catch
            {
                //donothing
            }
        }
    }
}
