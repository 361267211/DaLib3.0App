/*********************************************************
* 名    称：MedalObtainTaskService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：勋章获取任务服务
* 更新历史：
*
* *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DistributedIDGenerator;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.Application.Services.Impl
{
    /// <summary>
    /// 勋章获取任务
    /// </summary>
    public class MedalObtainTaskService : IMedalObtainTaskService, IScoped
    {
        private readonly IDistributedIDGenerator _idGenerator;
        private readonly IRepository<MedalObtainTask> _medalObtainTaskRepository;
        private readonly IGrpcClientResolver _grpcClientResolver;

        public MedalObtainTaskService(IDistributedIDGenerator idGenerator
            , IRepository<MedalObtainTask> medalObtainTaskRepository
            , IGrpcClientResolver grpcClientResolver)
        {
            _idGenerator = idGenerator;
            _medalObtainTaskRepository = medalObtainTaskRepository;
            _grpcClientResolver = grpcClientResolver;
        }


        /// <summary>
        /// 获取初始数据
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetInitData()
        {
            var AppItems = new Dictionary<string, string>();
            try
            {
                var appCenterClient = _grpcClientResolver.EnsureClient<AppCenterGrpcService.AppCenterGrpcServiceClient>();
                var appListData = await appCenterClient.GetAppByEventTypeAsync(new GetAppByEventTypeRequest { EventType = 3 });
                foreach (var item in appListData.GetAppByEventTypeList)
                {
                    AppItems.Add(item.AppName, item.AppCode);
                }
            }
            catch
            {
                AppItems = new Dictionary<string, string>();
            }
            //获取所有应用
            var initData = new
            {
                taskData = new MedalObtainTaskDto
                {
                    ID = _idGenerator.CreateGuid(),
                },
                AppItems = AppItems,
                TriggerWayItems = EnumHelper.GetEnumDescDictionaryItems(typeof(EnumMedalTriggerWay))
            };
            return await Task.FromResult(initData);
        }

        /// <summary>
        /// 通过应用编码获取积分事件
        /// </summary>
        public async Task<Dictionary<string, string>> GetMedalEventsByAppCode(string appCode)
        {
            var EventItems = new Dictionary<string, string>();
            try
            {
                var appCenterClient = _grpcClientResolver.EnsureClient<AppCenterGrpcService.AppCenterGrpcServiceClient>();
                var appListData = await appCenterClient.GetAppEventByCodeAsync(new GetAppEventByCodeRequest { EventType = 3, AppCode = appCode });
                foreach (var item in appListData.GetAppEventByCodeList)
                {
                    EventItems.Add(item.EventName, item.EventCode);
                }
            }
            catch
            {
                EventItems = new Dictionary<string, string>();
            }
            return EventItems;
        }

        /// <summary>
        /// 获取勋章任务列表信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<MedalObtainListItemDto>> QueryListData()
        {
            var listQuery = _medalObtainTaskRepository.DetachedEntities.Where(x => !x.DeleteFlag).ProjectToType<MedalObtainListItemDto>();
            var dataList = await listQuery.OrderByDescending(x => x.IsActive).ThenByDescending(x => x.CreateTime).ToListAsync();
            foreach (var x in dataList)
            {
                x.IntroPicUrl = (!string.IsNullOrWhiteSpace(x.IntroPicUrl) && !x.IntroPicUrl.StartsWith("/")) ? $"/{x.IntroPicUrl}" : x.IntroPicUrl;
            }
            return dataList;
        }

        /// <summary>
        /// 获取任务详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MedalObtainTaskDto> GetByID(Guid id)
        {
            var taskEntity = await _medalObtainTaskRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == id);
            if (taskEntity == null)
            {
                throw Oops.Oh("未找到勋章任务数据").StatusCode(Consts.Consts.ExceptionStatus);
            }
            var taskData = taskEntity.Adapt<MedalObtainTaskDto>();
            taskData.IntroPicUrl = (!string.IsNullOrWhiteSpace(taskData.IntroPicUrl) && !taskData.IntroPicUrl.StartsWith("/")) ? $"/{taskData.IntroPicUrl}" : taskData.IntroPicUrl;
            return taskData;
        }

        /// <summary>
        /// 创建任务
        /// </summary>
        /// <param name="medalTask"></param>
        /// <returns></returns>
        public async Task<Guid> Create(MedalObtainTaskDto medalTask)
        {
            medalTask.ID = _idGenerator.CreateGuid();
            var taskEntity = medalTask.Adapt<MedalObtainTask>();
            //检查是否有重复配置
            if (_medalObtainTaskRepository.DetachedEntities.Any(x => !x.DeleteFlag && x.AppCode == medalTask.AppCode && x.EventCode == medalTask.EventCode && x.Id != medalTask.ID))
            {
                throw Oops.Oh("该应用事件已配置勋章获取事件，不能重复配置").BadRequest();
            }
            var entityEntry = await _medalObtainTaskRepository.InsertAsync(taskEntity);
            return entityEntry.Entity.Id;
        }

        /// <summary>
        /// 编辑任务
        /// </summary>
        /// <param name="medalTask"></param>
        /// <returns></returns>
        public async Task<Guid> Update(MedalObtainTaskDto medalTask)
        {
            var taskEntity = await _medalObtainTaskRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == medalTask.ID);
            if (taskEntity == null)
            {
                throw Oops.Oh("未找到勋章任务数据").StatusCode(Consts.Consts.ExceptionStatus);
            }
            taskEntity = medalTask.Adapt(taskEntity);
            taskEntity.UpdateTime = DateTime.Now;
            //检查是否有重复配置
            if (_medalObtainTaskRepository.DetachedEntities.Any(x => !x.DeleteFlag && x.AppCode == medalTask.AppCode && x.EventCode == medalTask.EventCode && x.Id != medalTask.ID))
            {
                throw Oops.Oh("该应用事件已配置勋章获取事件，不能重复配置").BadRequest();
            }
            var entityEntry = await _medalObtainTaskRepository.UpdateAsync(taskEntity);
            return entityEntry.Entity.Id;
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(Guid id)
        {
            var taskEntity = await _medalObtainTaskRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (taskEntity == null)
            {
                throw Oops.Oh("未找到勋章任务数据").StatusCode(Consts.Consts.ExceptionStatus);
            }
            taskEntity.DeleteFlag = true;
            taskEntity.UpdateTime = DateTime.Now;
            await _medalObtainTaskRepository.UpdateAsync(taskEntity);
            return true;
        }

        /// <summary>
        /// 设置任务是否激活状态
        /// </summary>
        /// <param name="statusSet"></param>
        /// <returns></returns>
        public async Task<bool> SetActiveStatus(MedalObtainTaskStatusSet statusSet)
        {
            var taskEntity = await _medalObtainTaskRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == statusSet.ID);
            if (taskEntity == null)
            {
                throw Oops.Oh("未找到勋章任务数据").StatusCode(Consts.Consts.ExceptionStatus);
            }
            taskEntity.IsActive = statusSet.IsActive;
            taskEntity.UpdateTime = DateTime.Now;
            await _medalObtainTaskRepository.UpdateAsync(taskEntity);
            return true;
        }


    }
}
