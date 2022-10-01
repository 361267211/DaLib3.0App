using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Common.Services
{
    public interface ITenantDistributedCache
    {
        //
        // 摘要:
        //     Gets a value with the given key.
        //
        // 参数:
        //   key:
        //     A string identifying the requested value.
        //
        // 返回结果:
        //     The located value or null.
        byte[] Get(string key, bool isPublic = false);
        //
        // 摘要:
        //     Gets a value with the given key.
        //
        // 参数:
        //   key:
        //     A string identifying the requested value.
        //
        //   token:
        //     Optional. The System.Threading.CancellationToken used to propagate notifications
        //     that the operation should be canceled.
        //
        // 返回结果:
        //     The System.Threading.Tasks.Task that represents the asynchronous operation, containing
        //     the located value or null.
        Task<byte[]> GetAsync(string key, CancellationToken token = default, bool isPublic = false);
        //
        // 摘要:
        //     Gets a value with the given key.
        //
        // 参数:
        //   key:
        //     A string identifying the requested value.
        //
        // 返回结果:
        //     The located value or null.
        TResult Get<TResult>(string key, bool isPublic = false);
        //
        // 摘要:
        //     Gets a value with the given key.
        //
        // 参数:
        //   key:
        //     A string identifying the requested value.
        //
        //   token:
        //     Optional. The System.Threading.CancellationToken used to propagate notifications
        //     that the operation should be canceled.
        //
        // 返回结果:
        //     The System.Threading.Tasks.Task that represents the asynchronous operation, containing
        //     the located value or null.
        Task<TResult> GetAsync<TResult>(string key, CancellationToken token = default, bool isPublic = false);
        //
        // 摘要:
        //     Refreshes a value in the cache based on its key, resetting its sliding expiration
        //     timeout (if any).
        //
        // 参数:
        //   key:
        //     A string identifying the requested calue.
        void Refresh(string key, bool isPublic);
        //
        // 摘要:
        //     Refreshes a value in the cache based on its key, resetting its sliding expiration
        //     timeout (if any).
        //
        // 参数:
        //   key:
        //     A string identifying the requested value.
        //
        //   token:
        //     Optional. The System.Threading.CancellationToken used to propagate notifications
        //     that the operation should be canceled.
        //
        // 返回结果:
        //     The System.Threading.Tasks.Task that represents the asynchronous operation.
        Task RefreshAsync(string key, CancellationToken token = default, bool isPublic = false);
        //
        // 摘要:
        //     Removes the value with the given key.
        //
        // 参数:
        //   key:
        //     A string identifying the requested value.
        void Remove(string key, bool isPublic);
        //
        // 摘要:
        //     Removes the value with the given key.
        //
        // 参数:
        //   key:
        //     A string identifying the requested value.
        //
        //   token:
        //     Optional. The System.Threading.CancellationToken used to propagate notifications
        //     that the operation should be canceled.
        //
        // 返回结果:
        //     The System.Threading.Tasks.Task that represents the asynchronous operation.
        Task RemoveAsync(string key, CancellationToken token = default, bool isPublic = false);
        //
        // 摘要:
        //     Sets a value with the given key.
        //
        // 参数:
        //   key:
        //     A string identifying the requested value.
        //
        //   value:
        //     The value to set in the cache.
        //
        //   options:
        //     The cache options for the value.
        void Set(string key, byte[] value, DistributedCacheEntryOptions options, bool isPublic = false);
        //
        // 摘要:
        //     Sets the value with the given key.
        //
        // 参数:
        //   key:
        //     A string identifying the requested value.
        //
        //   value:
        //     The value to set in the cache.
        //
        //   options:
        //     The cache options for the value.
        //
        //   token:
        //     Optional. The System.Threading.CancellationToken used to propagate notifications
        //     that the operation should be canceled.
        //
        // 返回结果:
        //     The System.Threading.Tasks.Task that represents the asynchronous operation.
        Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default, bool isPublic = false);
        //
        // 摘要:
        //     Sets a value with the given key.
        //
        // 参数:
        //   key:
        //     A string identifying the requested value.
        //
        //   value:
        //     The value to set in the cache.
        //
        //   options:
        //     The cache options for the value.
        void Set<T>(string key, T value, DistributedCacheEntryOptions options, bool isPublic = false);
        //
        // 摘要:
        //     Sets the value with the given key.
        //
        // 参数:
        //   key:
        //     A string identifying the requested value.
        //
        //   value:
        //     The value to set in the cache.
        //
        //   options:
        //     The cache options for the value.
        //
        //   token:
        //     Optional. The System.Threading.CancellationToken used to propagate notifications
        //     that the operation should be canceled.
        //
        // 返回结果:
        //     The System.Threading.Tasks.Task that represents the asynchronous operation.
        Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options, CancellationToken token = default, bool isPublic = false);
    }
}
