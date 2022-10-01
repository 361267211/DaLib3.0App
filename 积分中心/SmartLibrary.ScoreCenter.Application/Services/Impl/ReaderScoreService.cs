/*********************************************************
* 名    称：ReaderScoreService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：用户积分服务
* 更新历史：
*
* *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.AppCenter;
using SmartLibrary.ScoreCenter.Application.Dtos;
using SmartLibrary.ScoreCenter.Application.Services.Enum;
using SmartLibrary.ScoreCenter.Application.Services.Extensions;
using SmartLibrary.ScoreCenter.Application.Services.Interface;
using SmartLibrary.ScoreCenter.Application.ViewModels;
using SmartLibrary.ScoreCenter.Common.Services;
using SmartLibrary.ScoreCenter.Common.Utils;
using SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys;
using SmartLibrary.User.RpcService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.Application.Services.Impl
{
    /// <summary>
    /// 用户积分服务
    /// </summary>
    public class ReaderScoreService : IReaderScoreService, IScoped
    {
        private readonly IRepository<UserScore> _userScoreRepository;
        private readonly IRepository<UserEventScore> _userEventScoreRepository;
        private readonly IRepository<ScoreLevel> _scoreLevelRepository;
        private readonly IRepository<UserMedal> _userMedalRepository;
        private readonly IRepository<OrderRecord> _orderRepository;
        private readonly IRepository<ScoreObtainTask> _scoreTaskRepository;
        private readonly IRepository<BasicConfig> _basicConfigRepository;
        private readonly IRepository<MedalObtainTask> _medalObtainTaskRepository;
        private readonly IRepository<GoodsRecord> _goodsRepository;
        private readonly IGrpcClientResolver _grpcClientResolver;

        public ReaderScoreService(IRepository<UserScore> userScoreRepository
            , IRepository<UserEventScore> userEventScoreRepository
            , IRepository<ScoreLevel> scoreLevelRepository
            , IRepository<UserMedal> userMedalRepository
            , IRepository<OrderRecord> orderRepository
            , IRepository<ScoreObtainTask> scoreTaskRepository
            , IRepository<BasicConfig> basicConfigRepository
            , IRepository<MedalObtainTask> medalObtainTaskRepository
            , IRepository<GoodsRecord> goodsRepository
            , IGrpcClientResolver grpcClientResolver)
        {
            _userScoreRepository = userScoreRepository;
            _userEventScoreRepository = userEventScoreRepository;
            _scoreLevelRepository = scoreLevelRepository;
            _userMedalRepository = userMedalRepository;
            _orderRepository = orderRepository;
            _scoreTaskRepository = scoreTaskRepository;
            _basicConfigRepository = basicConfigRepository;
            _medalObtainTaskRepository = medalObtainTaskRepository;
            _goodsRepository = goodsRepository;
            _grpcClientResolver = grpcClientResolver;
        }

        /// <summary>
        /// 获取初始数据
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetInitData()
        {
            return await Task.FromResult(new
            {
                Status = EnumHelper.GetEnumDescDictionaryItems(typeof(EnumOrderStatus)),
                ObtainWay = EnumHelper.GetEnumDescDictionaryItems(typeof(EnumObtainWay)),
                ScoreType = EnumHelper.GetEnumDescDictionaryItems(typeof(EnumScoreType)),
                GoodsType = EnumHelper.GetEnumDescDictionaryItems(typeof(EnumGoodsType)),
            });
        }

        /// <summary>
        /// 获取读者积分信息
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        public async Task<ReaderScoreInfoDto> GetReaderScoreInfo(string userKey)
        {
            var readerScoreInfo = new ReaderScoreInfoDto();
            var scoreEntity = await _userScoreRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag && x.UserKey == userKey);
            var readerScore = scoreEntity?.AvailableScore ?? 0;
            var consumeScore = await _userEventScoreRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.UserKey == userKey && x.Type == (int)EnumScoreType.减积分).SumAsync(x => x.EventScore);
            consumeScore = Math.Abs(consumeScore);
            var readerLevel = 0;
            var allLevels = await _scoreLevelRepository.DetachedEntities.Where(x => !x.DeleteFlag).OrderBy(x => x.ArchiveScore).ProjectToType<ScoreLevelListItemDto>().ToListAsync();
            if (allLevels.Any())
            {
                var sort = 1;
                allLevels.ForEach(x =>
                {
                    x.Level = sort++;
                });
                var archiveLevelEntity = allLevels.Where(x => x.ArchiveScore <= readerScore).OrderByDescending(x => x.ArchiveScore).FirstOrDefault();
                readerLevel = archiveLevelEntity?.Level ?? 0;
            }
            var readerMedalCount = 0;
            var userMedals = await _userMedalRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.UserKey == userKey).ToListAsync();
            if (userMedals.Any())
            {
                readerMedalCount = userMedals.Select(x => x.MedalObtainTaskId).Distinct().Count();
            }
            var readerOrdersCount = await _orderRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.ExchangeUserKey == userKey).CountAsync();
            var readerGoodsCount = 0;
            if (readerOrdersCount > 0)
            {
                readerGoodsCount = await _orderRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.ExchangeUserKey == userKey).SumAsync(x => x.ExchangeCount);
            }
            readerScoreInfo.Score = readerScore;
            readerScoreInfo.ConsumeScore = consumeScore;
            readerScoreInfo.Level = readerLevel;
            readerScoreInfo.MedalCount = readerMedalCount;
            readerScoreInfo.OrderCount = readerOrdersCount;
            readerScoreInfo.GoodsCount = readerGoodsCount;
            return readerScoreInfo;
        }

        /// <summary>
        /// 获取用户推荐任务
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<ReaderScoreObtainTaskDto>> QueryScoreObtainTask(ReaderScoreTaskTableQuery queryFilter)
        {
            //所有激活任务
            var endPointType = queryFilter.EndPointType ?? 0;
            var matchExpression = GetMatchEndPointTask(endPointType);
            var allActiveTasks = await _scoreTaskRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.IsActive)
                .Where(matchExpression)
                .OrderBy(x => x.TriggerTerm).ToListAsync();
            //固定任务
            var onceTasks = allActiveTasks.Where(x => x.TriggerTerm == (int)EnumTriggerTerm.永久).ToList();
            var onceInsertTasks = await GetNeedCompleteTasks(onceTasks, queryFilter.UserKey, null, null, endPointType);
            //每天任务
            var dayTasks = allActiveTasks.Where(x => x.TriggerTerm == (int)EnumTriggerTerm.每天).ToList();
            var dayStartTime = DateTime.Now.Date;
            var dayEndTime = DateTime.Now.Date.AddDays(1);
            var dayInsertTasks = await GetNeedCompleteTasks(dayTasks, queryFilter.UserKey, dayStartTime, dayEndTime, endPointType);
            //每周任务
            var weekTasks = allActiveTasks.Where(x => x.TriggerTerm == (int)EnumTriggerTerm.每周).ToList();
            var weekStartTime = GetWeekFirstDayMon(DateTime.Now);
            var weekEndTime = GetWeekLastDaySun(DateTime.Now).AddDays(1);
            var weekInsertTasks = await GetNeedCompleteTasks(weekTasks, queryFilter.UserKey, weekStartTime, weekEndTime, endPointType);
            //每月任务
            var monthTasks = allActiveTasks.Where(x => x.TriggerTerm == (int)EnumTriggerTerm.每月).ToList();
            var monthStartTime = DateTime.Now.AddDays(1 - DateTime.Now.Day).Date;
            var monthEndTime = DateTime.Now.AddDays(1 - DateTime.Now.Day).Date.AddMonths(1);
            var monthInsertTasks = await GetNeedCompleteTasks(monthTasks, queryFilter.UserKey, monthStartTime, monthEndTime, endPointType);
            //每年任务
            var yearTasks = allActiveTasks.Where(x => x.TriggerTerm == (int)EnumTriggerTerm.每年).ToList();
            int year = DateTime.Now.Year;
            var yearStartTime = new DateTime(year, 1, 1);
            var yearEndTime = yearStartTime.AddYears(1);
            var yearInsertTasks = await GetNeedCompleteTasks(monthTasks, queryFilter.UserKey, yearStartTime, yearEndTime, queryFilter.EndPointType);
            var uncompletedTasks = new List<ReaderScoreObtainTaskDto>();
            uncompletedTasks.AddRange(dayInsertTasks);
            uncompletedTasks.AddRange(weekInsertTasks);
            uncompletedTasks.AddRange(monthInsertTasks);
            uncompletedTasks.AddRange(yearInsertTasks);
            uncompletedTasks.AddRange(onceInsertTasks);
            var pagedList = uncompletedTasks.OrderBy(x => x.HasCompleted).ThenBy(x => x.TriggerTerm).AsQueryable().ToPagedList(queryFilter.PageIndex, queryFilter.PageSize);
            return pagedList;
        }

        #region QueryScoreObtainTaskHelper
        private DateTime GetWeekFirstDayMon(DateTime datetime)
        {
            //星期一为第一天  
            int weeknow = Convert.ToInt32(datetime.DayOfWeek);

            //因为是以星期一为第一天，所以要判断weeknow等于0时，要向前推6天。  
            weeknow = (weeknow == 0 ? (7 - 1) : (weeknow - 1));
            int daydiff = (-1) * weeknow;

            //本周第一天  
            string FirstDay = datetime.AddDays(daydiff).ToString("yyyy-MM-dd");
            return Convert.ToDateTime(FirstDay);
        }

        private DateTime GetWeekLastDaySun(DateTime datetime)
        {
            //星期天为最后一天  
            int weeknow = Convert.ToInt32(datetime.DayOfWeek);
            weeknow = (weeknow == 0 ? 7 : weeknow);
            int daydiff = (7 - weeknow);

            //本周最后一天  
            string LastDay = datetime.AddDays(daydiff).ToString("yyyy-MM-dd");
            return Convert.ToDateTime(LastDay);
        }

        /// <summary>
        /// 获取待完成任务
        /// </summary>
        /// <param name="termTasks"></param>
        /// <param name="userKey"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        private async Task<List<ReaderScoreObtainTaskDto>> GetNeedCompleteTasks(List<ScoreObtainTask> termTasks, string userKey, DateTime? startTime, DateTime? endTime, int? endPointType)
        {
            var termTaskCodes = termTasks.Select(x => x.FullEventCode).ToList();
            //已完成固定任务
            var completeTaskEvents = await _userEventScoreRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.UserKey == userKey && termTaskCodes.Contains(x.FullEventCode))
                .Where(startTime.HasValue, x => x.TriggerTime >= startTime.Value)
                .Where(endTime.HasValue, x => x.TriggerTime < endTime.Value)
                .Select(x => x.FullEventCode).ToListAsync();
            var completeTaskDesc = completeTaskEvents.GroupBy(x => x).Select(x => new { FullEventCode = x.Key, Count = x.Count() }).ToList();

            var appRouteCodes = new List<AppRouteUrlDto>();
            try
            {
                var appCodes = termTasks.Select(x => x.AppCode).ToList();
                var appCenterClient = _grpcClientResolver.EnsureClient<AppCenterGrpcService.AppCenterGrpcServiceClient>();
                var request = new AppBaseUriBatchRequest();
                request.AppRouteCode.AddRange(appCodes);
                var reply = await appCenterClient.GetAppBaseUriBatchAsync(request);
                appRouteCodes = reply.AppBaseUriBatchReplys.Select(x => new AppRouteUrlDto { AppCode = x.RouteCode, FrontUrl = x.FrontUrl, BackUrl = x.BackUrl }).ToList();
            }
            catch
            { //不处理
            }

            //待完成固定任务
            var insertTasks = new List<ReaderScoreObtainTaskDto>();

            termTasks.ForEach(x =>
            {
                var matchEvent = completeTaskDesc.FirstOrDefault(t => t.FullEventCode == x.FullEventCode);
                insertTasks.Add(new ReaderScoreObtainTaskDto
                {
                    TaskID = x.Id,
                    TaskName = x.Name,
                    TaskDesc = x.Desc,
                    Score = x.ObtainScore,
                    EndDate = x.EndDate,
                    IntroPicUrl = (!string.IsNullOrWhiteSpace(x.IntroPicUrl) && !x.IntroPicUrl.StartsWith("/")) ? $"/{x.IntroPicUrl}" : x.IntroPicUrl,
                    Link = GetMatchLink(x, appRouteCodes, endPointType ?? 0),
                    TriggerTerm = x.TriggerTerm,
                    TermTriggerTime = x.TriggerTime,
                    TermHasTriggerTime = matchEvent?.Count ?? 0,
                });
            });
            return insertTasks;
        }

        private Expression<Func<ScoreObtainTask, bool>> GetMatchEndPointTask(int endPointType)
        {
            switch (endPointType)
            {
                case 0:
                    return x => x.ForPc;
                case 1:
                    return x => x.ForApp;
                case 2:
                    return x => x.ForMicroApp;
                case 3:
                    return x => x.ForH5;
            }
            return x => x.ForPc;
        }

        private string GetMatchLink(ScoreObtainTask task, List<AppRouteUrlDto> appRouteInfo, int endPointType)
        {
            var matchRoute = appRouteInfo.FirstOrDefault(x => x.AppCode == task.AppCode);
            var matchPreUrl = "";
            if (matchRoute != null)
            {
                matchPreUrl = matchRoute.FrontUrl;
            }
            var link = "";


            switch (endPointType)
            {
                case 0://目前只对PC段做特殊处理
                    link = task.PcLink;
                    if (string.IsNullOrWhiteSpace(link))
                    {
                        return "#";
                    }
                    if (Uri.IsWellFormedUriString(link, UriKind.Absolute))
                    {
                        return link;
                    }
                    if (!string.IsNullOrWhiteSpace(matchPreUrl))
                    {
                        if (link.StartsWith("/") || link.StartsWith("\\"))
                        {
                            link.Remove(0, 1);
                        }
                        link = $"{matchPreUrl}/{link}";
                    }
                    break;
                case 1:
                    link = task.AppLink;
                    if (string.IsNullOrWhiteSpace(link))
                    {
                        return "#";
                    }
                    if (Uri.IsWellFormedUriString(link, UriKind.Absolute))
                    {
                        return link;
                    }
                    break;
                case 2:
                    link = task.MicroAppLink;
                    if (string.IsNullOrWhiteSpace(link))
                    {
                        return "#";
                    }
                    if (Uri.IsWellFormedUriString(link, UriKind.Absolute))
                    {
                        return link;
                    }
                    break;
                case 3:
                    link = task.H5Link;
                    if (string.IsNullOrWhiteSpace(link))
                    {
                        return "#";
                    }
                    if (Uri.IsWellFormedUriString(link, UriKind.Absolute))
                    {
                        return link;
                    }
                    break;
                default:
                    link = task.PcLink;
                    if (string.IsNullOrWhiteSpace(link))
                    {
                        return "#";
                    }
                    if (Uri.IsWellFormedUriString(link, UriKind.Absolute))
                    {
                        return link;
                    }
                    if (!string.IsNullOrWhiteSpace(matchPreUrl))
                    {
                        if (link.StartsWith("/") || link.StartsWith("\\"))
                        {
                            link.Remove(0, 1);
                        }
                        link = $"{matchPreUrl}{link}";
                    }
                    break;
            }

            return link;
        }
        #endregion

        /// <summary>
        /// 获取积分排行
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<ScoreRankListItemDto>> QueryScoreRankData(ReaderScoreRankTableQuery queryFilter)
        {
            var scoreRankQuery = _userEventScoreRepository.DetachedEntities.Where(x => !x.DeleteFlag)
                .GroupBy(x => x.UserKey).Select(x => new ScoreRankListItemDto
                {
                    Sort = 0,
                    UserKey = x.Key,
                    UserName = "",
                    UserCollege = "",
                    UserCollegeDepart = "",
                    Score = x.Sum(g => g.EventScore)
                });
            var scoreRankList = await scoreRankQuery.OrderByDescending(x => x.Score).ToPagedListAsync(queryFilter.PageIndex, queryFilter.PageSize);
            var sort = queryFilter.PageSize * (queryFilter.PageIndex - 1);
            var userKeys = scoreRankList.Items.Select(x => x.UserKey ?? "").ToList();
            var userGrpcClient = _grpcClientResolver.EnsureClient<UserGrpcService.UserGrpcServiceClient>();
            var userIdListRequest = new UserIdList();
            userIdListRequest.Ids.AddRange(userKeys);
            var userListData = await userGrpcClient.GetUserListByIdsAsync(userIdListRequest);

            foreach (var item in scoreRankList.Items)
            {
                item.Sort = ++sort;
                if (userListData != null && userListData.Items != null)
                {
                    var mapUser = userListData.Items.FirstOrDefault(x => x.Key == item.UserKey);
                    if (mapUser != null)
                    {
                        item.UserName = mapUser.Name;
                        item.UserCollege = mapUser.CollegeName;
                        item.UserCollegeDepart = mapUser.CollegeDepartName;
                    }
                }
            }
            return scoreRankList;
        }

        /// <summary>
        /// 获取积分规则
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetScoreRule()
        {
            var config = await _basicConfigRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag);
            return config?.RuleContent ?? "";
        }

        /// <summary>
        /// 获取用户等级
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        public async Task<ReaderScoreLevelOutput> GetReaderLevelInfo(string userKey)
        {
            var readerLevelInfo = new ReaderScoreLevelOutput
            {
                Score = 0,
                Level = 0,
                LevelName = "",
                NextLevelScore = 0,
            };
            var scoreEntity = await _userScoreRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag && x.UserKey == userKey);
            var readerScore = scoreEntity?.AvailableScore ?? 0;
            var readerLevel = 0;
            var readerLevelName = "";
            var nextLevelScore = 0;
            var allLevels = await _scoreLevelRepository.DetachedEntities.Where(x => !x.DeleteFlag).OrderBy(x => x.ArchiveScore).ProjectToType<ScoreLevelListItemDto>().ToListAsync();
            if (allLevels.Any())
            {
                var sort = 1;
                allLevels.ForEach(x =>
                {
                    x.Level = sort++;
                });
                var archiveLevelEntity = allLevels.Where(x => x.ArchiveScore <= readerScore).OrderByDescending(x => x.ArchiveScore).FirstOrDefault();
                readerLevel = archiveLevelEntity?.Level ?? 0;
                readerLevelName = archiveLevelEntity?.Name ?? "";
                nextLevelScore = archiveLevelEntity?.ArchiveScore ?? 0;
                var nextLevelEntity = allLevels.Where(x => x.ArchiveScore > readerScore).OrderBy(x => x.ArchiveScore).FirstOrDefault();
                nextLevelScore = nextLevelEntity != null ? nextLevelEntity.ArchiveScore : nextLevelScore;
            }
            readerLevelInfo.Score = readerScore;
            readerLevelInfo.Level = readerLevel;
            readerLevelInfo.LevelName = readerLevelName;
            readerLevelInfo.NextLevelScore = nextLevelScore;
            readerLevelInfo.Levels = allLevels.Select(x => new ScoreLevelListItemOutput
            {
                ID = x.ID,
                Level = x.Level,
                Name = x.Name,
                ArchiveScore = x.ArchiveScore
            }).ToList();
            return readerLevelInfo;
        }

        /// <summary>
        /// 获取用户勋章
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        public async Task<ReaderMedalOutput> GetReaderMedalInfo(string userKey)
        {
            var userMedals = await _userMedalRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.UserKey == userKey).ToListAsync();
            var medalInfo = new ReaderMedalOutput();
            medalInfo.AllMedals = await _medalObtainTaskRepository.DetachedEntities.Where(x => !x.DeleteFlag).Select(x => new MedalInfoListItemOutput
            {
                ID = x.Id,
                Name = x.Name,
                Desc = x.Desc,
                IntroPicUrl = x.IntroPicUrl,
                Link = "",
                ReaderCount = _userMedalRepository.DetachedEntities.Where(u => !u.DeleteFlag && u.MedalObtainTaskId == x.Id).Select(x => x.UserKey).Distinct().Count()
            }).ToListAsync();
            foreach (var x in medalInfo.AllMedals)
            {
                x.IntroPicUrl = (!string.IsNullOrWhiteSpace(x.IntroPicUrl) && !x.IntroPicUrl.StartsWith("/")) ? $"/{x.IntroPicUrl}" : x.IntroPicUrl;
            }
            userMedals.ForEach(x =>
            {
                var matchMedal = medalInfo.AllMedals.FirstOrDefault(m => m.ID == x.MedalObtainTaskId);
                if (matchMedal != null)
                {
                    medalInfo.UserMedal.Add(matchMedal);
                }

            });
            return medalInfo;
        }


        /// <summary>
        /// 获取读者积分明细
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<UserScoreEventListItemDto>> QueryReaderEventScoreTableData(ReaderScoreEventTableQuery queryFilter)
        {
            var scoreEventQuery = from scoreEvent in _userEventScoreRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.UserKey == queryFilter.UserKey)
                                  .Where(!string.IsNullOrWhiteSpace(queryFilter.Keyword), x => x.EventName.Contains(queryFilter.Keyword))
                                  .Where(queryFilter.Type.HasValue, x => x.Type == queryFilter.Type)
                                  .Where(queryFilter.TriggerStartTime.HasValue, x => x.TriggerTime >= queryFilter.TriggerStartTime)
                                  .Where(queryFilter.TriggerEndTime.HasValue, x => x.TriggerTime < queryFilter.TriggerCompareEndTime)
                                  select new UserScoreEventListItemDto
                                  {
                                      Type = scoreEvent.Type,
                                      Score = scoreEvent.EventScore,
                                      TriggerTime = scoreEvent.TriggerTime,
                                      EventName = scoreEvent.EventName,
                                      AppCode = scoreEvent.AppCode
                                  };
            var scoreEventList = await scoreEventQuery.OrderByDescending(x => x.TriggerTime).ToPagedListAsync(queryFilter.PageIndex, queryFilter.PageSize);
            return scoreEventList;
        }

        /// <summary>
        /// 获取读者商品订单明细
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<OrderListItemDto>> QueryReaderGoodsOrderTableData(OrderManageTableQuery queryFilter)
        {
            var userOrderQuery = from order in _orderRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.ExchangeUserKey == queryFilter.UserKey)
                                 .Where(queryFilter.Status.HasValue, x => x.Status == queryFilter.Status)
                                 .Where(queryFilter.ObtainWay.HasValue, x => x.ObtainWay == queryFilter.ObtainWay)
                                 .Where(queryFilter.TriggerStartTime.HasValue, x => x.ExchangeTime >= queryFilter.TriggerStartTime)
                                 .Where(queryFilter.TriggerEndTime.HasValue, x => x.ExchangeTime < queryFilter.TriggerEndCompareTime)
                                 join goods in _goodsRepository.DetachedEntities
                                 .Where(!string.IsNullOrWhiteSpace(queryFilter.GoodsName), x => x.Name.Contains(queryFilter.GoodsName))
                                 on order.GoodsID equals goods.Id into goodsStore
                                 from goods in goodsStore
                                 select new OrderListItemDto
                                 {
                                     ID = order.Id,
                                     GoodsID = order.GoodsID,
                                     GoodsName = goods.Name,
                                     ExchangeCode = order.ExchangeCode,
                                     ExchangeCount = order.ExchangeCount,
                                     ExchangeUserKey = order.ExchangeUserKey,
                                     ExchangeName = order.ExchangeName,
                                     Status = order.Status,
                                     ObtainWay = order.ObtainWay,
                                     ObtainTime = goods.ObtainTime,
                                     ExchangeTime = order.ExchangeTime,
                                     CreateTime = order.CreateTime
                                 };
            var userOrderList = await userOrderQuery.OrderByDescending(x => x.CreateTime).ToPagedListAsync(queryFilter.PageIndex, queryFilter.PageSize);
            return userOrderList;

        }
        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OrderDetailDto> GetReaderGoodsOrderDetail(Guid id)
        {
            var orderEntity = await _orderRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == id);
            if (orderEntity == null)
            {
                throw Oops.Oh("未找到订单详情数据").BadRequest();
            }
            var goodsEntity = await _goodsRepository.DetachedEntities.FirstOrDefaultAsync(x => x.Id == orderEntity.GoodsID);
            var orderData = orderEntity.Adapt<OrderDetailDto>();
            orderData.GoodsName = goodsEntity?.Name;
            orderData.GoodsType = goodsEntity?.Type ?? 1;
            orderData.ExchangeScore = Math.Abs(orderData.ExchangeScore);
            orderData.GoodsIntroPicUrl = (goodsEntity != null && !string.IsNullOrWhiteSpace(goodsEntity.IntroPicUrl) && !goodsEntity.IntroPicUrl.StartsWith("/")) ? $"/{goodsEntity.IntroPicUrl}" : goodsEntity?.IntroPicUrl ?? "";
            return orderData;
        }

        /// <summary>
        /// 查询积分中心尝尽数据
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        public async Task<ScoreCenterSceneOutput> QueryScoreCenterSceneData(string userKey)
        {
            var appCenterClient = _grpcClientResolver.EnsureClient<AppCenterGrpcService.AppCenterGrpcServiceClient>();
            var linkRequest = new AppBaseUriRequest { AppRouteCode = "scorecenter" };
            var linkResponse = await appCenterClient.GetAppBaseUriAsync(linkRequest);
            var scoreEntity = await _userScoreRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag && x.UserKey == userKey);
            var readerScore = scoreEntity?.AvailableScore ?? 0;
            var readerMedalCount = 0;
            var userMedals = await _userMedalRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.UserKey == userKey).ToListAsync();
            if (userMedals.Any())
            {
                readerMedalCount = userMedals.Select(x => x.MedalObtainTaskId).Distinct().Count();
            }

            var tasks = await this.QueryScoreObtainTask(new ReaderScoreTaskTableQuery { UserKey = userKey, PageSize = 2 });


            var result = new ScoreCenterSceneOutput
            {
                Score = readerScore,
                MedalCount = readerMedalCount,
                LinkUrl = linkResponse.FrontUrl,
                ScoreTasks = tasks.Items.Select(x => new ReaderScoreObtainTask
                {
                    TaskID = x.TaskID,
                    TaskName = x.TaskName,
                    TaskDesc = x.TaskDesc,
                    Score = x.Score,
                    EndDate = x.EndDate,
                    IntroPicUrl = x.IntroPicUrl,
                    Link = x.Link,
                    TriggerTerm = x.TriggerTerm,
                    TermTriggerTime = x.TermTriggerTime,
                    TermHasTriggerTime = x.TermHasTriggerTime,
                    HasCompleted = x.HasCompleted
                }).ToList()
            };
            return await Task.FromResult(result);
        }
    }
}
