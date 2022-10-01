/*********************************************************
* 名    称：UserGroupAnalysisService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户组画像数据分析
* 更新历史：
*
* *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.User.Application.Dtos;
using SmartLibrary.User.Application.Dtos.UserGroup;
using SmartLibrary.User.Application.Services.Enum;
using SmartLibrary.User.Application.Services.Interface;
using SmartLibrary.User.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.Services.Impl
{
    /// <summary>
    /// 用户组画像数据分析
    /// </summary>
    public class UserGroupAnalysisService : IUserGroupAnalysisService, IScoped
    {
        /// <summary>
        /// 获取初始数据
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetInitData()
        {
            var initData = new
            {
                AnalysisTimeType = EnumHelper.GetEnumDictionaryItems(typeof(AnalysisTimeType)),
                IntervalType = EnumHelper.GetEnumDictionaryItems(typeof(IntervalType)),
                BasicAnalysisType = EnumHelper.GetEnumDictionaryItems(typeof(BasicAnalysisType)),
                ResourceRankType = EnumHelper.GetEnumDictionaryItems(typeof(ResourceRankType)),
                ReaderRankType = EnumHelper.GetEnumDictionaryItems(typeof(ReaderRankType)),
            };
            return await Task.FromResult(initData);
        }

        /// <summary>
        /// 获取访问量简要信息
        /// </summary>
        /// <returns></returns>
        public async Task<UserGroupVisitBriefDto> GetVisitBreifInfo()
        {
            var fakeData = new UserGroupVisitBriefDto
            {
                TodayTotalCount = 1241,
                PreTotalCount = 3630,
                TodayUserCount = 1241,
                PreUserCount = 3630,
                GroupAvgCount = 12,
                UserAvgCount = 36
            };
            return await Task.FromResult(fakeData);
        }

        /// <summary>
        /// 查询访问图表
        /// </summary>
        /// <param name="intervalType"></param>
        /// <returns></returns>
        public async Task<List<ChartDataItemDto>> QueryVisitChartData(int intervalType)
        {
            var nowDay = DateTime.Now.Date;
            var chartList = new List<ChartDataItemDto> {
                new ChartDataItemDto{GroupName="门户访问量",Label=$"{nowDay.AddDays(-1).ToString("M月dd日")}",Value=50,Sort=7 },
                new ChartDataItemDto{GroupName="门户访问量",Label=$"{nowDay.AddDays(-2).ToString("M月dd日")}",Value=58,Sort=6 },
                new ChartDataItemDto{GroupName="门户访问量",Label=$"{nowDay.AddDays(-3).ToString("M月dd日")}",Value=40,Sort=5 },
                new ChartDataItemDto{GroupName="门户访问量",Label=$"{nowDay.AddDays(-4).ToString("M月dd日")}",Value=64,Sort=4 },
                new ChartDataItemDto{GroupName="门户访问量",Label=$"{nowDay.AddDays(-5).ToString("M月dd日")}",Value=35,Sort=3 },
                new ChartDataItemDto{GroupName="门户访问量",Label=$"{nowDay.AddDays(-6).ToString("M月dd日")}",Value=60,Sort=2 },
                new ChartDataItemDto{GroupName="门户访问量",Label=$"{nowDay.AddDays(-7).ToString("M月dd日")}",Value=40,Sort=1 },
                new ChartDataItemDto{GroupName="app访问量",Label=$"{nowDay.AddDays(-1).ToString("M月dd日")}",Value=58,Sort=7 },
                new ChartDataItemDto{GroupName="app访问量",Label=$"{nowDay.AddDays(-2).ToString("M月dd日")}",Value=48,Sort=6 },
                new ChartDataItemDto{GroupName="app访问量",Label=$"{nowDay.AddDays(-3).ToString("M月dd日")}",Value=43,Sort=5 },
                new ChartDataItemDto{GroupName="app访问量",Label=$"{nowDay.AddDays(-4).ToString("M月dd日")}",Value=64,Sort=4 },
                new ChartDataItemDto{GroupName="app访问量",Label=$"{nowDay.AddDays(-5).ToString("M月dd日")}",Value=25,Sort=3 },
                new ChartDataItemDto{GroupName="app访问量",Label=$"{nowDay.AddDays(-6).ToString("M月dd日")}",Value=67,Sort=2 },
                new ChartDataItemDto{GroupName="app访问量",Label=$"{nowDay.AddDays(-7).ToString("M月dd日")}",Value=43,Sort=1 },
                new ChartDataItemDto{GroupName="小程序访问量",Label=$"{nowDay.AddDays(-1).ToString("M月dd日")}",Value=52,Sort=7 },
                new ChartDataItemDto{GroupName="小程序访问量",Label=$"{nowDay.AddDays(-2).ToString("M月dd日")}",Value=58,Sort=6 },
                new ChartDataItemDto{GroupName="小程序访问量",Label=$"{nowDay.AddDays(-3).ToString("M月dd日")}",Value=73,Sort=5 },
                new ChartDataItemDto{GroupName="小程序访问量",Label=$"{nowDay.AddDays(-4).ToString("M月dd日")}",Value=24,Sort=4 },
                new ChartDataItemDto{GroupName="小程序访问量",Label=$"{nowDay.AddDays(-5).ToString("M月dd日")}",Value=35,Sort=3 },
                new ChartDataItemDto{GroupName="小程序访问量",Label=$"{nowDay.AddDays(-6).ToString("M月dd日")}",Value=57,Sort=2 },
                new ChartDataItemDto{GroupName="小程序访问量",Label=$"{nowDay.AddDays(-7).ToString("M月dd日")}",Value=53,Sort=1 },
                new ChartDataItemDto{GroupName="其他系统访问量",Label=$"{nowDay.AddDays(-1).ToString("M月dd日")}",Value=65,Sort=7 },
                new ChartDataItemDto{GroupName="其他系统访问量",Label=$"{nowDay.AddDays(-2).ToString("M月dd日")}",Value=46,Sort=6 },
                new ChartDataItemDto{GroupName="其他系统访问量",Label=$"{nowDay.AddDays(-3).ToString("M月dd日")}",Value=33,Sort=5 },
                new ChartDataItemDto{GroupName="其他系统访问量",Label=$"{nowDay.AddDays(-4).ToString("M月dd日")}",Value=64,Sort=4 },
                new ChartDataItemDto{GroupName="其他系统访问量",Label=$"{nowDay.AddDays(-5).ToString("M月dd日")}",Value=37,Sort=3 },
                new ChartDataItemDto{GroupName="其他系统访问量",Label=$"{nowDay.AddDays(-6).ToString("M月dd日")}",Value=38,Sort=2 },
                new ChartDataItemDto{GroupName="其他系统访问量",Label=$"{nowDay.AddDays(-7).ToString("M月dd日")}",Value=49,Sort=1 },
            };
            return await Task.FromResult(chartList);
        }


        /// <summary>
        /// 获取热点事件数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<HotEventListItemDto>> QueryHotEventData(TableQueryBase queryFilter)
        {
            var allSourceData = new List<HotEventListItemDto> {
                new HotEventListItemDto {
                    EventName="读者入馆",
                    GroupEventCount=210021,
                    TotalEventCount=2010021,
                    EventPercent=0.1302m,
                    GroupUserCount=210021,
                    TotalUserCount=2010021,
                    UserPercent=0.1401m,
                    PercentDiff=0.5801m,
                },
                new HotEventListItemDto {
                    EventName="图书借阅",
                    GroupEventCount=560021,
                    TotalEventCount=2010021,
                    EventPercent=0.1302m,
                    GroupUserCount=560021,
                    TotalUserCount=2010021,
                    UserPercent=0.1401m,
                    PercentDiff=0.1001m,
                },
                new HotEventListItemDto {
                    EventName="资源点击",
                    GroupEventCount=1110021,
                    TotalEventCount=21010021,
                    EventPercent=0.1302m,
                    GroupUserCount=1110021,
                    TotalUserCount=21010021,
                    UserPercent=0.1401m,
                    PercentDiff=0.0801m,
                },
                new HotEventListItemDto {
                    EventName="资源下载",
                    GroupEventCount=60021,
                    TotalEventCount=21010021,
                    EventPercent=0.1302m,
                    GroupUserCount=60021,
                    TotalUserCount=21010021,
                    UserPercent=0.1401m,
                    PercentDiff=0.1801m,
                },
                new HotEventListItemDto {
                    EventName="资源收藏",
                    GroupEventCount=160021,
                    TotalEventCount=26010021,
                    EventPercent=0.1802m,
                    GroupUserCount=160021,
                    TotalUserCount=26010021,
                    UserPercent=0.1401m,
                    PercentDiff=-0.0801m,
                },
                new HotEventListItemDto {
                    EventName="读者登录",
                    GroupEventCount=60021,
                    TotalEventCount=26010021,
                    EventPercent=0.1802m,
                    GroupUserCount=60021,
                    TotalUserCount=26010021,
                    UserPercent=0.1401m,
                    PercentDiff=-0.1001m,
                },
                new HotEventListItemDto {
                    EventName="资源分享",
                    GroupEventCount=60021,
                    TotalEventCount=26010021,
                    EventPercent=0.1802m,
                    GroupUserCount=60021,
                    TotalUserCount=26010021,
                    UserPercent=0.1401m,
                    PercentDiff=-0.0101m,
                },
            };
            var targetList = await allSourceData.OrderByDescending(x => x.PercentDiff).AsQueryable().ToPagedListAsync(queryFilter.PageIndex, queryFilter.PageSize);
            return targetList;
        }

        /// <summary>
        /// 获取资源排行
        /// </summary>
        /// <returns></returns>
        public async Task<List<ResourceRankListItemDto>> QueryResourceRankData()
        {
            var sourceData = new List<ResourceRankListItemDto> {
                new ResourceRankListItemDto{Name="中国知网",Count=158 },
                new ResourceRankListItemDto{Name="万方中文",Count=58 },
                new ResourceRankListItemDto{Name="维普",Count=31 },
                new ResourceRankListItemDto{Name="springer",Count=25 },
                new ResourceRankListItemDto{Name="wiley",Count=18 },
                new ResourceRankListItemDto{Name="超新",Count=15 },
            };
            var targetList = await sourceData.OrderByDescending(x => x.Count).AsQueryable().ToListAsync();
            return targetList;
        }

        /// <summary>
        /// 获取读者排行
        /// </summary>
        /// <returns></returns>
        public async Task<List<ReaderRankListItemDto>> QueryReaderRankData()
        {
            var sourceData = new List<ReaderRankListItemDto> {
                new ReaderRankListItemDto{Name="李楠一",College="汽车工程学院",Count=158 },
                new ReaderRankListItemDto{Name="张诺丹",College="外国语学院",Count=58 },
                new ReaderRankListItemDto{Name="左洪",College="软件工程学院",Count=31 },
                new ReaderRankListItemDto{Name="和诚",College="外国语学院",Count=25 },
                new ReaderRankListItemDto{Name="李丹",College="动力工程学院",Count=18 },
                new ReaderRankListItemDto{Name="邓星",College="软件工程学院",Count=15 },
                new ReaderRankListItemDto{Name="李涛",College="软件工程学院",Count=14 },
                new ReaderRankListItemDto{Name="刘玉华",College="外国语学院",Count=10 },
                new ReaderRankListItemDto{Name="文涛",College="动力工程学院",Count=8 },
                new ReaderRankListItemDto{Name="欣华",College="软件工程学院",Count=5 },
            };
            var targetList = await sourceData.OrderByDescending(x => x.Count).AsQueryable().ToListAsync();
            return targetList;
        }


    }
}
