
using Furion.DatabaseAccessor;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartLibrary.SceneManage.EntityFramework.Core.DbContexts;
using SmartLibrary.SceneManage.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.SceneManage.Application
{
    /// <summary>
    /// 数据库迁移的专用接口示例
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        private readonly SceneManageDbContext _storeDbContext;
        private readonly ILogger<ProductController> _logger;
        public ProductController(SceneManageDbContext storeDbContext,
            ILogger<ProductController> logger)
        {
            _logger = logger;
            _storeDbContext = storeDbContext;
        }

        /// <summary>
        /// 缓存测试 获取用户名称
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public int GetUserName()
        {
            _storeDbContext.Database.Migrate();
            return 1;
        }

        /// <summary>
        /// Exceptionless日志记录测试
        /// </summary>
        /// <param name="msg">记录消息</param>
        /// <returns></returns>
        [HttpGet]
        public async Task LogInfo(string msg)
        {
            _logger.LogError(msg);
            await Task.FromResult(msg);
        }
    }
}
