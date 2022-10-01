using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.Application
{
    /// <summary>
    /// WebApi心跳检查
    /// </summary>
    public class HealthAppService : IDynamicApiController
    {
        /// <summary>
        /// WebApi心跳检查
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public int Index()
        {
            return 1;
        }


        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="userKey">用户Key</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetToken([FromBody] string userKey)
        {
            if (string.IsNullOrEmpty(userKey))
                userKey = "cqu_vipsmart00001";
            // api路径（请求接口）
            string url = "http://192.168.21.71:8077/api/Auth/AccessToken";
            string paramStr = "{\"orgId\":\"string\",\"orgSecret\":\"string\",\"orgCode\":\"cqu\",\"userKey\":\"" + userKey + "\"}";
            byte[] postData = Encoding.UTF8.GetBytes(paramStr);
            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "application/json"); //采取POST方式必须加的header
            client.Headers.Add("ContentLength", postData.Length.ToString());
            byte[] responseData = client.UploadData(url, "POST", postData); //得到返回字符流
            string result = Encoding.UTF8.GetString(responseData); //解码
            return await Task.FromResult(result);
        }
    }
}
