/*********************************************************
* 名    称：DtmClient.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：Dtm客户端
* 更新历史：
*
* *******************************************************/
using SmartLibrary.DtmClient.Dtm.Tcc;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SmartLibrary.DtmClient.Dtm
{
    /// <summary>
    /// Dtm客户端
    /// </summary>
    public class DtmClient : IDtmClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;
        private bool _disposedValue;

        public DtmClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("dtmClient");
            _options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };
        }

        /// <summary>
        /// 执行Post请求
        /// </summary>
        /// <param name="client"></param>
        /// <param name="reqUrl"></param>
        /// <param name="contentBody"></param>
        /// <returns></returns>
        private async Task<DtmResult> DtmPostAsync(HttpClient client, string reqUrl, object contentBody)
        {
            var content = new StringContent(JsonSerializer.Serialize(contentBody, _options));
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await client.PostAsync(reqUrl, content);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"请求返回错误,StatusCode:{response.StatusCode}");
            }
            var dtmcontent = await response.Content.ReadAsStringAsync();
            var dtmResult = JsonSerializer.Deserialize<DtmResult>(dtmcontent, _options);
            return dtmResult;
        }

        /// <summary>
        /// 注册分支事务
        /// </summary>
        /// <param name="registerTcc"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> RegisterTccBranch(RegisterTccBranch registerTcc, CancellationToken cancellationToken)
        {
            var dtmResult = await DtmPostAsync(_httpClient, DtmServerInfo.Register_Tcc_Branch, registerTcc);
            CheckStatus(dtmResult);
            return dtmResult.Success;
        }

        /// <summary>
        /// Tcc准备
        /// </summary>
        /// <param name="tccBody"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> TccPrepare(TccBody tccBody, CancellationToken cancellationToken)
        {
            var dtmResult = await DtmPostAsync(_httpClient, DtmServerInfo.Prepare, tccBody);
            CheckStatus(dtmResult);
            return dtmResult.Success;
        }
        /// <summary>
        /// Tcc确认
        /// </summary>
        /// <param name="tccBody"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> TccSubmit(TccBody tccBody, CancellationToken cancellationToken)
        {
            var dtmResult = await DtmPostAsync(_httpClient, DtmServerInfo.Submit, tccBody);
            CheckStatus(dtmResult);
            return dtmResult.Success;
        }
        /// <summary>
        /// Tcc取消操作
        /// </summary>
        /// <param name="tccBody"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> TccAbort(TccBody tccBody, CancellationToken cancellationToken)
        {
            var dtmResult = await DtmPostAsync(_httpClient, DtmServerInfo.Abort, tccBody);
            CheckStatus(dtmResult);
            return dtmResult.Success;
        }

        /// <summary>
        /// Dtm服务器申请Id
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> GenGid(CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync(DtmServerInfo.New_Gid);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"请求返回错误,StatusCode: {response.StatusCode}");
            }
            var content = await response.Content.ReadAsStringAsync();

            var dtmgid = JsonSerializer.Deserialize<DtmGid>(content, _options);
            return dtmgid.Gid;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (!_disposedValue)
            {
                this._httpClient?.Dispose();
                _disposedValue = true;
            }
            GC.SuppressFinalize(this);
        }











        private void CheckStatus(DtmResult dtmResult)
        {
            if (dtmResult.Success != true)
            {
                throw new Exception($"Dtm处理失败:Message :{ dtmResult.Message }");
            }
        }
    }
}
