/*********************************************************
* 名    称：ScoreObtainTaskService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分获取任务
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
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.Application.Services.Impl
{
    /// <summary>
    /// 积分获取任务
    /// </summary>
    public class ScoreObtainTaskService : IScoreObtainTaskService, IScoped
    {
        private readonly IDistributedIDGenerator _idGenerator;
        private readonly IRepository<ScoreObtainTask> _scoreObtainTaskRepository;
        private readonly IRepository<ScoreConsumeTask> _scoreConsumeTaskRepository;
        private readonly IGrpcClientResolver _grpcClientResolver;

        public ScoreObtainTaskService(IDistributedIDGenerator idGenerator
            , IRepository<ScoreObtainTask> scoreObtainTaskRepository
            , IRepository<ScoreConsumeTask> scoreConsumeTaskRepository
            , IGrpcClientResolver grpcClientResolver)
        {
            _idGenerator = idGenerator;
            _scoreObtainTaskRepository = scoreObtainTaskRepository;
            _grpcClientResolver = grpcClientResolver;
            _scoreConsumeTaskRepository = scoreConsumeTaskRepository;
        }

        /// <summary>
        /// 初始数据
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
                taskData = new ScoreObtainTaskDto
                {
                    ID = _idGenerator.CreateGuid(),
                },
                AppItems = AppItems,
                ValidTermItems = EnumHelper.GetEnumDescDictionaryItems(typeof(EnumValidTerm)),
                TriggerTermItems = EnumHelper.GetEnumDescDictionaryItems(typeof(EnumTriggerTerm))
            };
            return await Task.FromResult(initData);
        }

        /// <summary>
        /// 通过应用编码获取积分事件
        /// </summary>
        public async Task<Dictionary<string, string>> GetScoreEventsByAppCode(string appCode)
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
        /// 获取积分任务列表信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<ScoreObtainListItemDto>> QueryListData()
        {
            var listQuery = _scoreObtainTaskRepository.DetachedEntities.Where(x => !x.DeleteFlag).ProjectToType<ScoreObtainListItemDto>();
            var dataList = await listQuery.OrderByDescending(x => x.IsActive).ThenByDescending(x => x.CreateTime).ToListAsync();
            return dataList;
        }

        /// <summary>
        /// 获取积分任务数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<ScoreObtainTaskDto> GetByID(Guid Id)
        {
            var taskEntity = await _scoreObtainTaskRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == Id);
            if (taskEntity == null)
            {
                throw Oops.Oh("未找到积分任务数据").StatusCode(Consts.Consts.ExceptionStatus);
            }
            var taskData = taskEntity.Adapt<ScoreObtainTaskDto>();
            return taskData;
        }

        /// <summary>
        /// 新增积分任务
        /// </summary>
        /// <param name="scoreTask"></param>
        /// <returns></returns>
        public async Task<Guid> Create(ScoreObtainTaskDto scoreTask)
        {
            scoreTask.ID = _idGenerator.CreateGuid();
            var taskEntity = scoreTask.Adapt<ScoreObtainTask>();
            taskEntity.FullEventCode = $"{taskEntity.AppCode}:{taskEntity.EventCode}";
            //检查事件是否配置为积分消费事件
            if (_scoreConsumeTaskRepository.DetachedEntities.Any(x => !x.DeleteFlag && x.AppCode == scoreTask.AppCode && x.EventCode == scoreTask.EventCode))
            {
                throw Oops.Oh("该应用事件已配置为积分消费事件，不能再配置为积分获取事件").BadRequest();
            }
            //检查是否重复配置积分事件
            if (_scoreObtainTaskRepository.DetachedEntities.Any(x => !x.DeleteFlag && x.AppCode == scoreTask.AppCode && x.EventCode == scoreTask.EventCode && x.Id != scoreTask.ID))
            {
                throw Oops.Oh("该应用事件已配置积分获取事件，不能重复配置").BadRequest();
            }
            var entityEntry = await _scoreObtainTaskRepository.InsertAsync(taskEntity);
            return entityEntry.Entity.Id;
        }

        /// <summary>
        /// 编辑积分任务
        /// </summary>
        /// <param name="scoreTask"></param>
        /// <returns></returns>
        public async Task<Guid> Update(ScoreObtainTaskDto scoreTask)
        {
            var taskEntity = await _scoreObtainTaskRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == scoreTask.ID);
            if (taskEntity == null)
            {
                throw Oops.Oh("未找到积分任务数据").StatusCode(Consts.Consts.ExceptionStatus);
            }
            taskEntity = scoreTask.Adapt(taskEntity);
            taskEntity.FullEventCode = $"{taskEntity.AppCode}:{taskEntity.EventCode}";
            taskEntity.UpdateTime = DateTime.Now;
            //检查事件是否配置为积分消费事件
            if (_scoreConsumeTaskRepository.DetachedEntities.Any(x => !x.DeleteFlag && x.AppCode == scoreTask.AppCode && x.EventCode == scoreTask.EventCode))
            {
                throw Oops.Oh("该应用事件已配置为积分消费事件，不能再配置为积分获取事件").BadRequest();
            }
            //检查是否重复配置积分事件
            if (_scoreObtainTaskRepository.DetachedEntities.Any(x => !x.DeleteFlag && x.AppCode == scoreTask.AppCode && x.EventCode == scoreTask.EventCode && x.Id != scoreTask.ID))
            {
                throw Oops.Oh("该应用事件已配置积分获取事件，不能重复配置").BadRequest();
            }
            var entityEntry = await _scoreObtainTaskRepository.UpdateAsync(taskEntity);
            return entityEntry.Entity.Id;
        }

        /// <summary>
        /// 删除积分任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(Guid id)
        {
            var taskEntity = await _scoreObtainTaskRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (taskEntity == null)
            {
                throw Oops.Oh("未找到积分任务数据").StatusCode(Consts.Consts.ExceptionStatus);
            }
            taskEntity.DeleteFlag = true;
            taskEntity.UpdateTime = DateTime.Now;
            await _scoreObtainTaskRepository.UpdateAsync(taskEntity);
            return true;
        }

        /// <summary>
        /// 设置任务激活状态
        /// </summary>
        /// <param name="statusSet"></param>
        /// <returns></returns>
        public async Task<bool> SetActiveStatus(ScoreObtainTaskStatusSet statusSet)
        {
            var taskEntity = await _scoreObtainTaskRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == statusSet.ID);
            if (taskEntity == null)
            {
                throw Oops.Oh("未找到积分任务数据").StatusCode(Consts.Consts.ExceptionStatus);
            }
            taskEntity.IsActive = statusSet.IsActive;
            taskEntity.UpdateTime = DateTime.Now;
            await _scoreObtainTaskRepository.UpdateAsync(taskEntity);
            return true;
        }


    }
}
