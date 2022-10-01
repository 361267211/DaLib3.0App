using Furion.DependencyInjection;
using SmartLibrary.LogAnalysis.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SmartLibrary.LogAnalysis.Common.Dtos;
using System.Xml.Serialization;
using Furion.JsonSerialization;

namespace SmartLibrary.LogAnalysis.Application.Services
{
    public class LogAnalysisService : ILogAnalysisService, IScoped
    {


        /// <summary>
        /// 返回结果
        /// </summary>
        [Serializable]
        public class ApiResult<T>
        {
            /// <summary>
            /// 状态
            /// </summary>
            public int status { get; set; }
            /// <summary>
            /// 信息
            /// </summary>
            public string info { get; set; }
            /// <summary>
            /// 结果
            /// </summary>
            public T result { get; set; }
            /// <summary>
            /// 匹配结果总数
            /// </summary>
            public int count { get; set; }
        }

        private class ApiResultRoot
        {
            public IndexStatisticsDto Result { get; set; }
        }

        private readonly IHttpClientFactory _httpClientFactory;

        private static readonly JsonSerializerOptions s_JsonSerializerOptions = new JsonSerializerOptions
            { PropertyNameCaseInsensitive = true };





        public LogAnalysisService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// 随机数发生器
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        private static int GenerateRandomNumber(int max)
        {
            var randomKey = Guid.NewGuid().ToString();
            var random = new Random(randomKey.Select(Convert.ToInt32).Sum());
            return random.Next(max / 10, max);
        }
        private static int Generate1e3RandomNumber() => GenerateRandomNumber(1000);
        private static int Generate1e6RandomNumber() => GenerateRandomNumber(100_000);
        public Task<IReadOnlyList<DisplayLogAnalysisResponseDto>> GetDisplayLogAnalysesAsync(DisplayLogAnalysisRequestDto requestDto)
        {

            DateTime dtStart, dtEnd;
            if (requestDto.From.HasValue && requestDto.To.HasValue) { dtStart = requestDto.From.Value; dtEnd = requestDto.To.Value; }
            else
            {
                dtEnd = DateTime.Now;
                switch (requestDto.Range)
                {
                    case 1: dtStart = dtEnd.AddDays(-1).Date; break;
                    case 3: dtStart = dtEnd.AddDays(-3).Date; break;
                    case 7:
                        dtStart = dtEnd.AddDays(0 - Convert.ToInt32(dtEnd.DayOfWeek)).Date;
                        break;
                    case 365: dtStart = new DateTime(dtEnd.Year, 1, 1); break;
                    default: throw new NotSupportedException("参数错误，不支持的日期范围");
                }
            }
            if (dtStart > dtEnd) throw new ArgumentException("日期范围错误");
            if (requestDto.Type < 1 || requestDto.Type > 2) throw new ArgumentException("不支持的统计类型");

            var randomNumberGenerators = new
             (int, string, Func<int>)[] {

            (1,"图书馆入馆人次",Generate1e6RandomNumber),
            (1,"图书借阅册数",Generate1e6RandomNumber),
            (1,"座位预约次数",Generate1e6RandomNumber),
            (1,"门户访问人次",Generate1e6RandomNumber),
            (1,"统一检索下载数",Generate1e6RandomNumber),
            (1,"快应用使用人次",Generate1e6RandomNumber),


            (2,"纸本图书册数",Generate1e6RandomNumber),

            (2,"电子图书种数",Generate1e6RandomNumber),
            (2,"数据库个数",Generate1e3RandomNumber),

            (2,"期刊篇数",Generate1e6RandomNumber),
            (2,"中文期刊种数",Generate1e6RandomNumber),
            (2,"外文期刊种数",Generate1e3RandomNumber),

            (2,"本校毕业论文",Generate1e6RandomNumber),
            (2,"数字记忆",Generate1e6RandomNumber),
            (2,"特色专题",Generate1e6RandomNumber),
             };



            IReadOnlyList<DisplayLogAnalysisResponseDto> result = randomNumberGenerators.Where(x => x.Item1 == requestDto.Type).Select(x => new DisplayLogAnalysisResponseDto
            {
                Count = x.Item3.Invoke(),
                DisplayTitle = x.Item2
            }).ToArray();
            return Task.FromResult(result);
        }

        public async Task<IndexStatisticsDto> GetIndexStatisticsAsync()
        {
            using (var httpClient = this._httpClientFactory.CreateClient(nameof(SiteGlobalConfig.IndexStatisticsLink)))
            {
                var step1 = await httpClient.GetAsync("/api/v1.0/index/getindexstatistics");
                step1.EnsureSuccessStatusCode();
                var json = await step1.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(json)) return default;
                var temp = JsonSerializer.Deserialize<ApiResultRoot>(json, s_JsonSerializerOptions);
                return temp?.Result;
            }
        }

        /// <summary>
        /// 获取热门下载的文献
        /// </summary>
        /// <returns></returns>
        public async Task<List<ILibTitleInfo>> GetRecommendHotBookAsync()
        {
            using (var httpClient = this._httpClientFactory.CreateClient("webapi2.2"))
            {
                var step1 = await httpClient.GetAsync("/api/v1.0/RecommendHotBook");
                step1.EnsureSuccessStatusCode();
                var json = await step1.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(json)) return default;
                var temp = JSON.Deserialize<ApiResult<List<ILibTitleInfo>>>(json);
                return temp?.result;
            }
        }
        /// <summary>
        /// 获取热门借阅的文献
        /// </summary>
        /// <returns></returns>
        public async Task<List<ILibTitleInfo>> GetRecommendHotBorrowBookAsync()
        {
            using (var httpClient = this._httpClientFactory.CreateClient("webapi2.2"))
            {
                var step1 = await httpClient.GetAsync("/api/v1.0/RecommendHotBrorrowBook");
                step1.EnsureSuccessStatusCode();
                var json = await step1.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(json)) return default;
                var temp = JSON.Deserialize<ApiResult<List<ILibTitleInfo>>>(json);
                return temp?.result;
            }
        }
    }
}
