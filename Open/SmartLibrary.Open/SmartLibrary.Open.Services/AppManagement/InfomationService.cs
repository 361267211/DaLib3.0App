/*********************************************************
* 名    称：InfomationService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20211109
* 描    述：消息服务
* 更新历史：
*
* *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DistributedIDGenerator;
using Furion.FriendlyException;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.Open.Common.Enums;
using SmartLibrary.Open.Common.Extensions;
using SmartLibrary.Open.EntityFramework.Core.Entitys;
using SmartLibrary.Open.Services.Dtos;
using SmartLibrary.Open.Services.Dtos.Infomation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services
{
    public class InfomationService : IInfomationService
    {
        private readonly IDistributedIDGenerator _idGenerator;
        private readonly IRepository<Information> _infoRepository;
        private readonly IRepository<ActivityInfo> _activityRepository;
        private readonly IRepository<InfoSpecificCustomer> _infoSpecificCustomerRepository;
        private readonly IRepository<AppDynamic> _appDynamicRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<MicroApplication> _appRepository;

        /// <summary>
        /// 服务初始化
        /// </summary>
        /// <param name="idGenerator"></param>
        public InfomationService(IDistributedIDGenerator idGenerator
            , IRepository<Information> infoRepository
            , IRepository<ActivityInfo> activityRepository
            , IRepository<InfoSpecificCustomer> infoSpecificCustomerRepository
            , IRepository<AppDynamic> appDynamicRepository
            , IRepository<Customer> customerRepository
            , IRepository<MicroApplication> appRepository)
        {
            _idGenerator = idGenerator;
            _infoRepository = infoRepository;
            _activityRepository = activityRepository;
            _infoSpecificCustomerRepository = infoSpecificCustomerRepository;
            _appDynamicRepository = appDynamicRepository;
            _customerRepository = customerRepository;
            _appRepository = appRepository;
        }
        /// <summary>
        /// 创建活动消息
        /// </summary>
        /// <param name="activityInfoDto"></param>
        /// <returns></returns>
        public async Task<Guid> CreateActivityInfo(ActivityInfoDto activityInfoDto)
        {
            var informationEntity = new Information
            {
                Id = _idGenerator.CreateGuid(),
                Title = activityInfoDto.Title,
                Content = activityInfoDto.Content,
                PublishDate = activityInfoDto.PublishDate,
                Status = activityInfoDto.Status,
            };
            var activityEntity = new ActivityInfo
            {
                Id = _idGenerator.CreateGuid(),
                IsPublic = activityInfoDto.IsPublic,
                InfoId = informationEntity.Id.ToString()
            };
            var specificCustomers = activityInfoDto.SpecificCustomerIds.Select(x => new InfoSpecificCustomer
            {
                Id = _idGenerator.CreateGuid(),
                ActInfoId = activityEntity.Id.ToString(),
                CustomerId = x.ToString()
            }).ToList();
            var infoEntry = await _infoRepository.InsertAsync(informationEntity);
            var activityEntry = await _activityRepository.InsertAsync(activityEntity);
            await _infoSpecificCustomerRepository.InsertAsync(specificCustomers);
            return activityEntity.Id;
        }

        /// <summary>
        /// 创建应用动态
        /// </summary>
        /// <param name="dynamicInfoDto"></param>
        /// <returns></returns>
        public async Task<Guid> CreateAppDynamicInfo(AppDynamicDto dynamicInfoDto)
        {
            var informationEntity = new Information
            {
                Id = _idGenerator.CreateGuid(),
                Title = dynamicInfoDto.Title,
                Content = dynamicInfoDto.Content,
                PublishDate = dynamicInfoDto.PublishDate,
                Status = dynamicInfoDto.Status,
            };
            var dynamicEntity = new AppDynamic
            {
                Id = _idGenerator.CreateGuid(),
                AppId = dynamicInfoDto.AppID.ToString(),
                AppBranchId = dynamicInfoDto.AppBranchID.ToString(),
                InfoType = dynamicInfoDto.InfoType,
                Version = dynamicInfoDto.Version,
                InfoId = informationEntity.Id.ToString(),
            };
            var infoEntry = await _infoRepository.InsertAsync(informationEntity);
            var dynamicEntry = await _appDynamicRepository.InsertAsync(dynamicEntity);
            return dynamicEntity.Id;
        }

        /// <summary>
        /// 修改活动消息
        /// </summary>
        /// <param name="activityInfoDto"></param>
        /// <returns></returns>
        public async Task<Guid> UpdateActivityInfo(ActivityInfoDto activityInfoDto)
        {
            var activityEntity = await _activityRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == activityInfoDto.ID);
            if (activityEntity == null)
            {
                throw Oops.Oh("为获取到活动数据");
            }
            var infoId = new Guid(activityEntity.InfoId);
            var infoEntity = await _infoRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == infoId);
            if (infoEntity == null)
            {
                throw Oops.Oh("未找到活动消息");
            }
            var delCustomers = await _infoSpecificCustomerRepository.Where(x => !x.DeleteFlag && x.ActInfoId == activityEntity.Id.ToString()).ToListAsync();
            delCustomers.ForEach(x => x.DeleteFlag = true);
            var specificCustomers = activityInfoDto.SpecificCustomerIds.Select(x => new InfoSpecificCustomer
            {
                Id = _idGenerator.CreateGuid(),
                ActInfoId = activityEntity.Id.ToString(),
                CustomerId = x.ToString()
            }).ToList();
            activityEntity.IsPublic = activityInfoDto.IsPublic;
            infoEntity.Title = activityInfoDto.Title;
            infoEntity.Content = activityInfoDto.Content;
            await _activityRepository.UpdateAsync(activityEntity);
            await _infoRepository.UpdateAsync(infoEntity);
            await _infoSpecificCustomerRepository.UpdateAsync(delCustomers);
            await _infoSpecificCustomerRepository.InsertAsync(specificCustomers);
            return activityEntity.Id;
        }

        /// <summary>
        /// 修改应用动态
        /// </summary>
        /// <param name="dynamicInfoDto"></param>
        /// <returns></returns>
        public async Task<Guid> UpdateAppDynamicInfo(AppDynamicDto dynamicInfoDto)
        {
            var dynamicEntity = await _appDynamicRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == dynamicInfoDto.ID);
            if (dynamicEntity == null)
            {
                throw Oops.Oh("为获取到应用动态数据");
            }
            var infoId = new Guid(dynamicEntity.InfoId);
            var infoEntity = await _infoRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == infoId);
            if (infoEntity == null)
            {
                throw Oops.Oh("未找到动态消息");
            }
            infoEntity.Title = infoEntity.Title;
            infoEntity.Content = infoEntity.Content;
            await _infoRepository.UpdateAsync(infoEntity);
            return dynamicEntity.Id;
        }

        /// <summary>
        /// 删除活动消息
        /// </summary>
        /// <param name="delIds"></param>
        /// <returns></returns>
        public async Task<bool> DeleteActivityInfo(List<Guid> delIds)
        {
            var updateBuilder = _activityRepository.Context.BatchUpdate<ActivityInfo>();
            await updateBuilder
                .Set(s => s.DeleteFlag, s => true)
                .Set(s => s.UpdatedTime, s => DateTime.Now)
                .Where(x => !x.DeleteFlag && delIds.Contains(x.Id))
                .ExecuteAsync();
            return true;
        }

        /// <summary>
        /// 删除应用动态
        /// </summary>
        /// <param name="delIds"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAppDynamicInfo(List<Guid> delIds)
        {
            var updateBuilder = _activityRepository.Context.BatchUpdate<AppDynamic>();
            await updateBuilder
                .Set(s => s.DeleteFlag, s => true)
                .Set(s => s.UpdatedTime, s => DateTime.Now)
                .Where(x => !x.DeleteFlag && delIds.Contains(x.Id))
                .ExecuteAsync();
            return true;
        }

        /// <summary>
        /// 获取活动消息
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public async Task<ActivityInfoViewModel> GetActivityInfoById(Guid activityId)
        {
            var activityInfoEntity = await _activityRepository.DetachedEntities.FirstOrDefaultAsync(x => x.Id == activityId);
            if (activityInfoEntity == null)
            {
                throw Oops.Oh("未找到活动信息");
            }
            var infoId = new Guid(activityInfoEntity.InfoId);
            var infoEntity = await _infoRepository.DetachedEntities.FirstOrDefaultAsync(x => x.Id == infoId);
            if (infoEntity == null)
            {
                throw Oops.Oh("未找到活动消息");
            }
            var specificCustomerIds = await _infoSpecificCustomerRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.ActInfoId == activityId.ToString()).Select(x => x.CustomerId).ToListAsync();
            var customerIds = specificCustomerIds.Select(x => new Guid(x)).ToList();
            var customerNames = await _customerRepository.DetachedEntities.Where(x => customerIds.Contains(x.Id)).Select(x => x.Name).ToListAsync();
            var activityInfo = new ActivityInfoViewModel
            {
                ID = activityInfoEntity.Id,
                InfoID = new Guid(activityInfoEntity.InfoId),
                Title = infoEntity.Title,
                Content = infoEntity.Content,
                PublishDate = infoEntity.PublishDate,
                IsPublic = activityInfoEntity.IsPublic,
                Status = infoEntity.Status,
                SpecificCustomerNames = customerNames
            };
            return activityInfo;
        }

        /// <summary>
        /// 获取应用动态
        /// </summary>
        /// <param name="appDynamicId"></param>
        /// <returns></returns>
        public async Task<AppDynamicInfoViewModel> GetAppDynamicById(Guid appDynamicId)
        {
            var appDynamicEntity = await _appDynamicRepository.DetachedEntities.FirstOrDefaultAsync(x => x.Id == appDynamicId);
            if (appDynamicEntity == null)
            {
                throw Oops.Oh("未找到应用动态");
            }
            var infoId = new Guid(appDynamicEntity.InfoId);
            var infoEntity = await _infoRepository.DetachedEntities.FirstOrDefaultAsync(x => x.Id == infoId);
            if (infoEntity == null)
            {
                throw Oops.Oh("未找到动态消息");
            }
            var appId = new Guid(appDynamicEntity.AppId);
            var appEntity = await _appRepository.DetachedEntities.FirstOrDefaultAsync(x => x.Id == appId);
            var dynamicInfo = new AppDynamicInfoViewModel
            {
                ID = appDynamicEntity.Id,
                AppID = appEntity.Id,
                AppName = appEntity.Name,
                AppIcon = appEntity.Icon,
                AppBranchID = appDynamicEntity.AppBranchId,
                InfoType = appDynamicEntity.InfoType,
                InfoID = new Guid(appDynamicEntity.InfoId),
                Title = infoEntity.Title,
                Content = infoEntity.Content,
                PublishDate = infoEntity.PublishDate,
                Status = infoEntity.Status,
                Version = appDynamicEntity.Version,
            };
            return dynamicInfo;
        }

        /// <summary>
        /// 获取活动消息数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<ActivityInfoViewModel>> QueryActivityInfoTableData(AppInfoTableQuery queryFilter)
        {
            var infoQuery = from activity in _activityRepository.DetachedEntities.Where(x => !x.DeleteFlag)
                            join info in _infoRepository.DetachedEntities.Where(x => !x.DeleteFlag) on activity.InfoId equals info.Id.ToString() into infos
                            from info in infos
                            select new ActivityInfoViewModel
                            {
                                ID = activity.Id,
                                InfoID = new Guid(activity.InfoId),
                                Title = info.Title,
                                Content = info.Content,
                                PublishDate = info.PublishDate,
                                IsPublic = activity.IsPublic,
                                Status = info.Status,
                            };
            var pageList = await infoQuery.OrderByDescending(x => x.Status).ThenByDescending(x => x.PublishDate).ToPagedListAsync(queryFilter.PageIndex, queryFilter.PageSize);
            return pageList;
        }

        /// <summary>
        /// 查询应用动态
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<AppDynamicInfoViewModel>> QueryAppDynamicTableData(AppInfoTableQuery queryFilter)
        {
            var appDynamicQuery = from appDynamic in _appDynamicRepository.DetachedEntities.Where(x => !x.DeleteFlag)
                                  join info in _infoRepository.DetachedEntities.Where(x => !x.DeleteFlag) on appDynamic.InfoId equals info.Id.ToString() into infos
                                  from info in infos
                                  join app in _appRepository.DetachedEntities.Where(x => !x.DeleteFlag) on appDynamic.AppId equals app.Id.ToString() into apps
                                  from app in apps
                                  select new AppDynamicInfoViewModel
                                  {
                                      ID = appDynamic.Id,
                                      AppID = app.Id,
                                      AppName = app.Name,
                                      AppIcon = app.Icon,
                                      AppBranchID = appDynamic.AppBranchId,
                                      InfoType = appDynamic.InfoType,
                                      InfoID = new Guid(appDynamic.InfoId),
                                      Title = info.Title,
                                      Content = info.Content,
                                      PublishDate = info.PublishDate,
                                      Status = info.Status,
                                      Version = appDynamic.Version,
                                  };
            var pageList = await appDynamicQuery.OrderByDescending(x => x.Status).ThenByDescending(x => x.PublishDate).ToPagedListAsync(queryFilter.PageIndex, queryFilter.PageSize);
            return pageList;
        }

        /// <summary>
        /// 置顶活动
        /// </summary>
        /// <param name="topSetIds"></param>
        /// <returns></returns>
        public async Task<bool> TopSetActivityInfo(List<Guid> topSetIds)
        {
            var activityInfoIds = await _activityRepository.DetachedEntities.Where(x => !x.DeleteFlag && topSetIds.Contains(x.Id)).Select(x => x.InfoId).ToListAsync();
            var infoIds = activityInfoIds.Select(x => new Guid(x)).ToList();
            var updateBuilder = _activityRepository.Context.BatchUpdate<Information>();
            await updateBuilder.Set(s => s.Status, s => (int)EnumInformationStatus.置顶)
                .Set(s => s.UpdatedTime, s => DateTime.Now)
                .Where(s => !s.DeleteFlag && infoIds.Contains(s.Id))
                .ExecuteAsync();
            return true;
        }

        /// <summary>
        /// 置顶应用动态
        /// </summary>
        /// <param name="topSetIds"></param>
        /// <returns></returns>
        public async Task<bool> TopSetAppDynamicInfo(List<Guid> topSetIds)
        {
            var dynamicInfoIds = await _appDynamicRepository.DetachedEntities.Where(x => !x.DeleteFlag && topSetIds.Contains(x.Id)).Select(x => x.InfoId).ToListAsync();
            var infoIds = dynamicInfoIds.Select(x => new Guid(x)).ToList();
            var updateBuilder = _activityRepository.Context.BatchUpdate<Information>();
            await updateBuilder.Set(s => s.Status, s => (int)EnumInformationStatus.置顶)
                .Set(s => s.UpdatedTime, s => DateTime.Now)
                .Where(s => !s.DeleteFlag && infoIds.Contains(s.Id))
                .ExecuteAsync();
            return true;
        }

        /// <summary>
        /// 取消置顶消息
        /// </summary>
        /// <param name="activityIds"></param>
        /// <returns></returns>
        public async Task<bool> UntopSetActivityInfo(List<Guid> activityIds)
        {
            var activityInfoIds = await _activityRepository.DetachedEntities.Where(x => !x.DeleteFlag && activityIds.Contains(x.Id)).Select(x => x.InfoId).ToListAsync();
            var infoIds = activityInfoIds.Select(x => new Guid(x)).ToList();
            var updateBuilder = _activityRepository.Context.BatchUpdate<Information>();
            await updateBuilder.Set(s => s.Status, s => (int)EnumInformationStatus.正常)
                .Set(s => s.UpdatedTime, s => DateTime.Now)
                .Where(s => !s.DeleteFlag && infoIds.Contains(s.Id))
                .ExecuteAsync();
            return true;
        }

        /// <summary>
        /// 取消置顶应用动态
        /// </summary>
        /// <param name="dynamicIds"></param>
        /// <returns></returns>
        public async Task<bool> UntopSetAppDynamicInfo(List<Guid> dynamicIds)
        {
            var activityInfoIds = await _activityRepository.DetachedEntities.Where(x => !x.DeleteFlag && dynamicIds.Contains(x.Id)).Select(x => x.InfoId).ToListAsync();
            var infoIds = activityInfoIds.Select(x => new Guid(x)).ToList();
            var updateBuilder = _activityRepository.Context.BatchUpdate<Information>();
            await updateBuilder.Set(s => s.Status, s => (int)EnumInformationStatus.正常)
                .Set(s => s.UpdatedTime, s => DateTime.Now)
                .Where(s => !s.DeleteFlag && infoIds.Contains(s.Id))
                .ExecuteAsync();
            return true;
        }


    }
}
