using Furion.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using SmartLibrary.Identity.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Common.Services
{
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
