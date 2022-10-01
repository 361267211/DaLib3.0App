using System;
using Furion.JsonSerialization;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartLibrary.AppCenter;
using SmartLibrary.GuessUserLike;
using SmartLibrary.User.Application.Dtos;
using SmartLibrary.User.Application.Dtos.Common;
using SmartLibrary.User.Application.ViewModels;
using SmartLibrary.User.Application.ViewModels.UserScene;
using SmartLibrary.User.Common.Extensions;
using SmartLibrary.User.Common.Services;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using SmartLibrary.User.Common.Dtos;

namespace SmartLibrary.User.Application.AppServices
{
    /// <summary>
    /// 读者服务
    /// </summary>
    [Authorize(Policy = PolicyKey.TokenAuth)]
    public class SceneAppService : BaseAppService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IGrpcClientResolver _grpcClientResolver;

        public SceneAppService(IGrpcClientResolver grpcClientResolver,
                               IHttpClientFactory httpClientFactory)
        {
            _grpcClientResolver = grpcClientResolver;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// 获取我的书斋信息
        /// </summary>
        /// <returns></returns>
        public async Task<MyStudyInfoOutput> GetMyStudyInfo()
        {
            var currentUser = CurrentUser;
            var result = new MyStudyInfoOutput();

            var appCenterClient = _grpcClientResolver.EnsureClient<AppCenterGrpcService.AppCenterGrpcServiceClient>();
            var allAppsTask = appCenterClient.GetAllAppsAsync(new Google.Protobuf.WellKnownTypes.Empty()).ResponseAsync;
           
            var guessLikeGrpcClient = _grpcClientResolver.EnsureClient<SmartGuessUserLikeGrpcService.SmartGuessUserLikeGrpcServiceClient>();
            var guessLikeTask = guessLikeGrpcClient.GuessWhatsMyInterestedAsync(new GuessWhatsMyInterestedRequest { PageIndex = 1 }).ResponseAsync;

            await Task.WhenAll(allAppsTask, guessLikeTask);

            if (allAppsTask.Result != null)
            {
                var appList = new List<AppInfo>();
                foreach (var item in allAppsTask.Result.AppList)
                {
                    appList.Add(new AppInfo
                    {
                        AppId = item.AppId,
                        Icon = item.Icon,
                        BackUrl = item.BackUrl,
                        FrontUrl = item.FrontUrl,
                        Name = item.Name,
                        RouteCode = item.RouteCode
                    });
                }
                result.Items.Add(new MyStudyItemOutput { Code = "Borrow", Name = "当前借阅", Count = 10, Order = 1, LinkUrl = appList.First(c => c.RouteCode == "bookborrow")?.FrontUrl });
                result.Items.Add(new MyStudyItemOutput { Code = "Space", Name = "空间预约", Count = 0, Order = 2, LinkUrl = appList.First(c => c.RouteCode == "spaceorder")?.FrontUrl });
                result.Items.Add(new MyStudyItemOutput { Code = "Collect", Name = "我的收藏", Count = 12, Order = 3, LinkUrl = "#" });
                result.Items.Add(new MyStudyItemOutput { Code = "Score", Name = "我的积分", Count = 112, Order = 4, LinkUrl = appList.First(c => c.RouteCode == "scorecenter")?.FrontUrl });
                result.Items.Add(new MyStudyItemOutput { Code = "Visit", Name = "本月到馆", Count = 26, Order = 5, LinkUrl = "#" });
            }
            if (guessLikeTask.Result != null)
            {
                result.Recommends = guessLikeTask.Result.Items.Adapt<List<MyStudyRecommendItem>>();
            }

            return result;
        }

        private async Task<GetStatisItemsReply> RequestFromApiAsync(GetStatisItemsRequest input)
        {
            var userKey = CurrentUser.UserKey;
            input.UserKey = userKey;
            using (var httpClient = _httpClientFactory.CreateClient("webapi2.2"))
            {
                string content = JsonConvert.SerializeObject(input);
                var buffer = Encoding.UTF8.GetBytes(content);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var step1 = await httpClient.PostAsync("/api/v1/GetStatisItems", byteContent);
                step1.EnsureSuccessStatusCode();
                var json = await step1.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(json)) return default;
                var temp = JSON.Deserialize<ApiResult<GetStatisItemsReply>>(json);
                return temp.result;
            }
        }
        /// <summary>
        /// 获取重大首页-个人统计信息  1.当前借阅，2.我的成果，3.累计积分，4.期刊订阅，5.今年到馆，6.预约座位，7.门户访问
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IReadOnlyList<StaticsItem>> GetStatisItems([FromBody] GetStatisItemsRequest input)
        {
            var scoreTask = this._grpcClientResolver.EnsureClient<ScoreCenter.ScoreCenterGrpcService.ScoreCenterGrpcServiceClient>()
                 .GetScoreByUserKeyAsync(new ScoreCenter.UserScoreRequest { UserKey = CurrentUser.UserKey }).ResponseAsync;
            var appCenterClient = _grpcClientResolver.EnsureClient<AppCenterGrpcService.AppCenterGrpcServiceClient>();
            var allAppsTask = appCenterClient.GetAllAppsAsync(new Google.Protobuf.WellKnownTypes.Empty()).ResponseAsync;
            var task1 = RequestFromApiAsync(input);
            await Task.WhenAll(task1, allAppsTask,scoreTask);
            var temp = task1.Result;
            var allApps = allAppsTask.Result.AppList;

            var userKey = this.CurrentUser.UserKey;
            var result = new[]
            { 
                new StaticsItem
                {
                    Count = temp.MyCourrentBorrowCount, Title = "当前借阅",
                    Link = allApps.FirstOrDefault(c => c.RouteCode == "bookborrow")?.FrontUrl ?? "#"
                },
                new StaticsItem { Count = temp.MyAchievementsCount, Title = "我的成果", Link = "#" },
                new StaticsItem
                {
                    Count =scoreTask.Result.UserScore, 
                    Title = "当前积分",
                    Link = allApps.FirstOrDefault(c => c.RouteCode == "scorecenter")?.FrontUrl ?? "#"
                },
                new StaticsItem { Count = temp.MyCollectJournalCount, Title = "期刊订阅", Link = GetUrl(userKey,"/user/journal") },
                new StaticsItem { Count = temp.MyYearComeInLibCount, Title = "今年到馆", Link = "#" },
                new StaticsItem { Count = temp.MyReservedSeatCount, Title = "预约座位", Link =  GetUrl(userKey,"/wechatredirect?aid=1&code=99173AEFB0E9665556FF114A000D1245&readerid=&link=3") },
                new StaticsItem { Count = temp.MyVisitPortalCount, Title = "门户访问", Link = "#" },
            };

            return result;

        }


        private static string GetUrl(string userKey, string redirectUrl)
        {
            static string MD5Encode(string str, Encoding encoding = null)
            {
                encoding = encoding == null ? Encoding.ASCII : encoding;
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                var result = md5.ComputeHash(encoding.GetBytes(str));
                var strResult = BitConverter.ToString(result);
                return strResult.Replace("-", "");
            }
            var code = MD5Encode($"{SiteGlobalConfig.OldSite.Aid}{SiteGlobalConfig.OldSite.Secret}{userKey}");
            var redirect = redirectUrl;
            return $"{SiteGlobalConfig.OldSite.SiteUrl}/caslogin/appredirect?aid={SiteGlobalConfig.OldSite.Aid}&code={code}&readerId={userKey}&redirect={HttpUtility.UrlEncode(redirect)}";
        }
    }
}
