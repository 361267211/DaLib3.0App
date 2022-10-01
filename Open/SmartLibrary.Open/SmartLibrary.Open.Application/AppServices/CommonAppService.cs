/*********************************************************
* 名    称：CommonAppService.cs
* 作    者：刘启平
* 联系方式：电话[13627622058],邮件[83379242@qq.com]
* 创建时间：2021/10/2 14:15:27
* 描    述：开放平台通用功能设置
* 更新历史：
*
* *******************************************************/
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.Open.Common.AssemblyBase;
using SmartLibrary.Open.Common.Dtos;
using SmartLibrary.Open.EntityFramework.Core.DbContexts;
using SmartLibrary.Open.EntityFramework.Core.Entitys;
using SmartLibrary.Open.Services;
using SmartLibrary.Open.Services.Dtos;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Application.AppServices
{
    /// <summary>
    /// 开放平台通用功能设置
    /// </summary>
    public class CommonAppService : BaseAppService
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        private readonly OpenDbContext _openDbContext;

        private IApplicationService _applicationService;
        private ICommonService _commonService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="openDbContext"></param>
        /// <param name="applicationService"></param>
        /// <param name="commonService"></param>
        public CommonAppService(OpenDbContext openDbContext,
                                IApplicationService applicationService,
                                ICommonService commonService)
        {
            _openDbContext = openDbContext;
            _applicationService = applicationService;
            _commonService = commonService;
        }


        /// <summary>
        /// 获取服务类型
        /// </summary>
        /// <returns></returns>
        public async Task<AppInitModel> GetDictionary()
        {
            return await _applicationService.GetDictionary();
        }

        /// <summary>
        /// 添加服务类型
        /// </summary>
        /// <param name="dictDto">字典数据</param>
        /// <returns></returns>
        public async Task<Guid> CreateServiceType([FromBody] AppDictionaryDto dictDto)
        {
            var result = await _commonService.CreateServiceType(dictDto);
            return result;
        }

        /// <summary>
        /// 添加服务包
        /// </summary>
        /// <param name="dictDto"></param>
        /// <returns></returns>
        public async Task<Guid> CreateServicePack([FromBody] AppDictionaryDto dictDto)
        {
            var result = await _commonService.CreateServicePack(dictDto);
            return result;
        }

        /// <summary>
        /// 获取字典数据详情
        /// </summary>
        /// <param name="dictId"></param>
        /// <returns></returns>
        [Route("[action]/{dictId}")]
        public async Task<SysDictModel> GetSysDictInfo(Guid dictId)
        {
            var result = await _commonService.GetSysDictInfo(dictId);
            return result;
        }

        /// <summary>
        /// 编辑字典数据
        /// </summary>
        /// <param name="dictDto"></param>
        /// <returns></returns>
        public async Task<bool> UpdateSysDict([FromBody] AppDictionaryDto dictDto)
        {
            var result = await _commonService.UpdateSysDict(dictDto);
            return result;
        }

        /// <summary>
        /// 删除字典数据
        /// </summary>
        /// <param name="dictId"></param>
        /// <returns></returns>
        [Route("[action]/{dictId}")]
        public async Task<bool> DeleteSysDict(Guid dictId)
        {
            var result = await _commonService.DeleteSysDict(dictId);
            return result;
        }


        /// <summary>
        /// 升级数据库结构
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<bool> UpdateDatabaseSchema()
        {
            _openDbContext.Database.Migrate();
            return Task.FromResult(true);
        }


        /// <summary>
        /// 添加测试数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<bool> AddTestData()
        {
            _applicationService.AddTestData();
            return Task.FromResult(true);
        }

        /// <summary>
        /// 添加机构测试数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<bool> AddCustomerTestData()
        {
            _applicationService.AddCustomerAndAppUse();
            return Task.FromResult(true);
        }


        /// <summary>
        /// 添加应用测试数据——积分中心
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<bool> AddAppTestData_ScoreCenter()
        {
            _applicationService.AddCustomerAndAppUse();
            return Task.FromResult(true);
        }


        /// <summary>
        /// 添加应用测试数据——应用入口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<bool> AddAppTestData_AppEntrance()
        {
            _applicationService.AddAppEntrance();
            return Task.FromResult(true);
        }

        /// <summary>
        /// 添加应用测试数据——应用模板
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<bool> AddAppWidget()
        {
            _applicationService.AddAppWidget();
            return Task.FromResult(true);
        }


        /// <summary>
        /// 添加应用测试数据-重大需要
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<bool> AddAppTestData()
        {
            _applicationService.AddAppTestData();
            return Task.FromResult(true);
        }

        /// <summary>
        /// 添加重大需要的应用,全部跳转到2.2版本
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<bool> AddTestAppsForCqu()
        {
            _applicationService.AddTestAppsForCqu();
            return Task.FromResult(true);
        }

        /// <summary>
        /// 添加快应用中心跳转到2.2应用
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<bool> AddQuickAppForCqu()
        {
            _applicationService.AddQuickAppForCqu();
            return Task.FromResult(true);
        }

        /// <summary>
        /// 添加应用更新日志
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<bool> AddTestAppLogs()
        {
            _applicationService.AddTestAppLogs();
            return Task.FromResult(true);
        }

        /// <summary>
        /// 添加业务类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task AddBusinessType()
        {
            _openDbContext.BulkInsert(new[] {
                new AppDictioanry{DictType="BusinessType",Name="内容发布",Value="1", Desc="内容发布",DeleteFlag=false, Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.Now,UpdatedTime=DateTimeOffset.Now },
                new AppDictioanry{DictType="BusinessType",Name="学术服务",Value="2", Desc="学术服务",DeleteFlag=false, Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.Now,UpdatedTime=DateTimeOffset.Now },
                new AppDictioanry{DictType="BusinessType",Name="数据管理",Value="3", Desc="数据管理",DeleteFlag=false, Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.Now,UpdatedTime=DateTimeOffset.Now },
                new AppDictioanry{DictType="BusinessType",Name="统计分析",Value="4", Desc="统计分析",DeleteFlag=false, Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.Now,UpdatedTime=DateTimeOffset.Now },
                new AppDictioanry{DictType="BusinessType",Name="其他",Value="5", Desc="其他",DeleteFlag=false, Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.Now,UpdatedTime=DateTimeOffset.Now },
            });

            return Task.CompletedTask;
        }


        /// <summary>
        /// 添加测试数据，完成后可以删除
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task AddTestAppInfo()
        {
            //开发者标识
            var devId = "26857b56-e438-4ad5-afb4-9b9bc80aed30";
            //部署环境标识
            var deployeeId = "0e6558f8-da1a-4144-8877-decf9b07abd8";
            //客户标识
            var customerId = "1fc3b47e-76a8-4ab0-bc60-c43fd9a9a2a9";

            var appId1 = Guid.NewGuid();
            var appId2 = Guid.NewGuid();
            // 插入MicroApplication表
            _openDbContext.BulkInsert(
          new[] {new MicroApplication { Id=appId1,Desc="测试购买应用1",Name= "测试购买应用1",Intro="测试购买应用1", AdvisePrice=500,DevId= devId, FreeTry=false,Icon="/app_icons/icon-donation.svg", PriceType=2,RecommendScore=10,
                    RouteCode="testapp1",ServiceType="2",Status=1,Terminal="1",UseScene="3",CreatedTime=DateTimeOffset.Now,UpdatedTime=DateTimeOffset.Now}
                ,new MicroApplication { Id=appId2,Desc="测试购买应用2",Name= "测试购买应用2",Intro="测试购买应用2", AdvisePrice=899,DevId= devId, FreeTry=false,Icon="/app_icons/icon-donation.svg",PriceType=2,RecommendScore=10,
                    RouteCode="testapp1",ServiceType="2",Status=1,Terminal="1",UseScene="3",CreatedTime=DateTimeOffset.Now,UpdatedTime=DateTimeOffset.Now}
                    });
            //分支数据
            var appBrId1 = Guid.NewGuid();
            var appBrId2 = Guid.NewGuid();
            _openDbContext.BulkInsert(
         new[] { new AppBranch {Id=appBrId1, AppId=appId1.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deployeeId,Icon="/app_icons/icon-donation.svg",IsMaster=false, Name="测试购买应用1", Remark="测试购买应用1",Version="3.0.0"}
                ,new AppBranch {Id=appBrId2, AppId=appId2.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deployeeId,Icon="/app_icons/icon-donation.svg",IsMaster=false, Name="测试购买应用2", Remark="测试购买应用2",Version="3.0.0"}
                });

            return Task.CompletedTask;
        }
    }
}
