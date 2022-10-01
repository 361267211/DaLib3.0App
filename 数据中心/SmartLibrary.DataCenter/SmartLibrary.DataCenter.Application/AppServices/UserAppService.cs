
using Furion.DatabaseAccessor;
using Furion.DynamicApiController;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SmartLibrary.DataCenter.EntityFramework.Core.Entitys;
using SmartLibraryUser;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.DataCenter.Application
{
    /// <summary>
    /// 应用服务接口
    /// </summary>
    public class UserAppService : IDynamicApiController
    {
        private readonly IDistributedCache _cache;
        private readonly IUserService _userService;
        public IConfiguration _configuration { get; }

        private readonly IRepository<Asset> _assetRepository;

        public UserAppService(IUserService userService, IConfiguration configuration,
        IDistributedCache cache, IRepository<Asset> assetRepository)
        {
            _configuration = configuration;
            _userService = userService;
            _assetRepository = assetRepository;
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
        /// 多租户测试 插入一条Asset数据，并立即返回其信息
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Asset> InsertAsset(string title)
        {
            var asset = await _assetRepository.Entities.AddAsync(new Asset() { Title = title, CreatedTime = DateTime.Now });
            return asset.Entity;
        }

        /// <summary>
        /// 多租户测试 获取所有Asset
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedList<Asset>> GetAllAsset()
        {
            PagedList<Asset> asset = await _assetRepository.Entities.ToPagedListAsync();
            return asset.Adapt<PagedList<Asset>>();
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

        /// <summary>
        /// 设置redis缓存值
        /// </summary>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<String> GetCach(string redisValue)
        {

            //获取缓存
            string res = "";
            var obj = await _cache.GetAsync("cachedTimeUTC");
            if (obj != null)
                res = Encoding.UTF8.GetString(obj);
            return res;
        }
    }
}
