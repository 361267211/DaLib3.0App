
using Furion.DatabaseAccessor;
using Furion.DynamicApiController;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SmartLibrary.SceneManage.EntityFramework.Core.Entitys;
using SmartLibraryUser;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.SceneManage.Application
{
    /// <summary>
    /// 应用服务接口
    /// </summary>
    public class UserAppService : IDynamicApiController
    {
        private readonly IDistributedCache _cache;
        private readonly IUserService _userService;
        public IConfiguration _configuration { get; }

        public UserAppService(IUserService userService, IConfiguration configuration,
        IDistributedCache cache)
        {
            _configuration = configuration;
            _userService = userService;
            _cache = cache;
        }

        /// <summary>
        /// 缓存测试 获取用户名称
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Task<UserReply> GetUserName(UserRequest request)
        {
            var result = _userService.GetUserName(request);
            return result;
        }

      

        /// <summary>
        /// 设置redis缓存值
        /// </summary>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<String> SetCach(string redisValue)
        {
            var currentTimeUTC = DateTime.UtcNow.ToString();
            byte[] encodedCurrentTimeUTC = Encoding.UTF8.GetBytes(currentTimeUTC);

            // 设置分布式缓存
            var options = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(200));

            await _cache.SetAsync("cachedTimeUTC", encodedCurrentTimeUTC, options);
            //获取缓存
            string res = "";
            var obj = await _cache.GetAsync("cachedTimeUTC");
            if (obj != null)
                res = Encoding.UTF8.GetString(obj);
            return res;
        }
    }
}
