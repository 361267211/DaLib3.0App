/*********************************************************
* 名    称：TccGlobalTransaction.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：Tcc全局事务处理器
* 更新历史：
*
* *******************************************************/
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SmartLibrary.DtmClient.Dtm.Tcc
{
    /// <summary>
    /// Tcc全局事务处理器
    /// </summary>
    public class TccGlobalTransaction
    {
        private readonly IDtmClient _dtmClient;
        private readonly ILogger logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _options;

        public TccGlobalTransaction(IDtmClient dtmClient
            , ILoggerFactory factory
            , IHttpClientFactory httpClientFactory)
        {
            _dtmClient = dtmClient;
            _httpClientFactory = httpClientFactory;
            logger = factory.CreateLogger<TccGlobalTransaction>();
            _options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };
        }

        /// <summary>
        /// 开启全局事务，注册分支事务
        /// </summary>
        /// <param name="tcc_cb"></param>
        /// <param name="headers"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<DtmExecuteResult> Excecute(Func<Tcc, Task> tcc_cb, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
        {
            var tcc = new Tcc(_dtmClient, _httpClientFactory, await this.GenGid());

            var tbody = new TccBody
            {
                Gid = tcc.Gid,
                Trans_Type = "tcc",
                Branch_Headers = headers ?? new Dictionary<string, string>()
            };

            try
            {
                await _dtmClient.TccPrepare(tbody, cancellationToken);

                await tcc_cb(tcc);

                await _dtmClient.TccSubmit(tbody, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "submitting or abort global transaction error");
                await _dtmClient.TccAbort(tbody, cancellationToken);
                return new DtmExecuteResult { IsSuccess = false, ErrMsg = ex.Message };
            }
            return new DtmExecuteResult { IsSuccess = true };
        }
        /// <summary>
        /// 要求事务协调器分发全局事务Id
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> GenGid(CancellationToken cancellationToken = default)
        {
            return await _dtmClient.GenGid(cancellationToken);
        }

    }
}
