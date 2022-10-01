using Furion.DynamicApiController;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartLibrary.Identity.Application.Services.Interface;
using SmartLibrary.Identity.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace SmartLibrary.Identity.Application.AppServices
{
    /// <summary>
    /// 三方登录
    /// </summary>
    public class ThirdPartAuthAppService : IDynamicApiController
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILoginConfigService _loginConfigService;
        public ThirdPartAuthAppService(IHttpContextAccessor contextAccessor
            , IHttpClientFactory httpClientFactory
            , ILoginConfigService loginConfigService)
        {
            _contextAccessor = contextAccessor;
            _httpClientFactory = httpClientFactory;
            _loginConfigService = loginConfigService;
        }


        /// <summary>
        /// Cas代理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<string> CasProxy()
        {
            var request = _contextAccessor.HttpContext.Request;
            var ticket = request.Query["ticket"].ToString();
            var service = request.Query["service"].ToString();
            var responseStr = await HttpGet($"{SiteGlobalConfig.CasUrl}/cas/serviceValidate?ticket={ticket}&service={service}");
            return responseStr;
        }
        /// <summary>
        /// 获取http请求数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private async Task<string> HttpGet(string url)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36");
            var resultBytes = await client.GetByteArrayAsync(url);
            return Encoding.UTF8.GetString(resultBytes);
        }
    }
}
