/*********************************************************
* 名    称：Tcc.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：Tcc方式处理事务
* 更新历史：
*
* *******************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SmartLibrary.DtmClient.Dtm.Tcc
{
    /// <summary>
    /// Tcc方式处理事务
    /// </summary>
    public class Tcc
    {
        public IdGenerator idGen;
        public string dtm;
        private string gid;
        private IDtmClient dtmClient;
        private readonly IHttpClientFactory _httpClientFactory;

        public Tcc(IDtmClient dtmHttpClient, IHttpClientFactory httpClientFactory, string gid)
        {
            this.dtmClient = dtmHttpClient;
            this._httpClientFactory = httpClientFactory;
            this.Gid = gid;
            this.idGen = new IdGenerator();
        }

        public string Gid { get => gid; set => gid = value; }

        /// <summary>
        /// 注册事务分支
        /// </summary>
        /// <param name="body"></param>
        /// <param name="tryUrl"></param>
        /// <param name="confirmUrl"></param>
        /// <param name="cancelUrl"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public async Task<string> CallBranch(object body, string tryUrl, string confirmUrl, string cancelUrl, CancellationToken cancellationToken = default, Dictionary<string, string> headers = null)
        {
            var branchId = this.idGen.NewBranchId();
            var registerTccBranch = new RegisterTccBranch()
            {
                Branch_id = branchId,
                Cancel = cancelUrl,
                Confirm = confirmUrl,
                Status = "prepared",
                Trans_type = "tcc",
                Gid = this.Gid,
                Try = tryUrl,
                Data = JsonSerializer.Serialize(body)
            };

            await dtmClient.RegisterTccBranch(registerTccBranch, cancellationToken);

            var tryHttpClient = _httpClientFactory.CreateClient();
            var tryContent = new StringContent(JsonSerializer.Serialize(body));
            tryContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            if (headers != null && headers.Any())
            {
                foreach (var header in headers)
                {
                    if (!tryHttpClient.DefaultRequestHeaders.Contains(header.Key))
                    {
                        tryHttpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
            }
            tryUrl = $"{tryUrl}?gid={this.Gid}&trans_type=tcc&branch_id={branchId}&op=try";

            var response = await tryHttpClient.PostAsync(tryUrl, tryContent);
            await CheckResult(response);
            return "SUCCESS";
        }
        /// <summary>
        /// 检查Response结果
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public async Task CheckResult(HttpResponseMessage response)
        {
            int errorCode = 400;
            var result = await response.Content.ReadAsStringAsync();
            //var reason = response.ReasonPhrase;
            if ((int)response.StatusCode >= errorCode)
            {
                throw new Exception($"{result}");
            }
            if (string.IsNullOrWhiteSpace(result))
            {
                throw new Exception("无响应类型");
            }
            if (!result.Contains("SUCCESS"))
            {
                throw new Exception("请求执行失败");
            }
        }
    }
}
