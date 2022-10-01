/*********************************************************
* 名    称：ScoreConsumeTaskService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分消费任务
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
using SmartLibrary.ScoreCenter.Application.Services.Extensions;
using SmartLibrary.ScoreCenter.Application.Services.Interface;
using SmartLibrary.ScoreCenter.Application.ViewModels;
using SmartLibrary.ScoreCenter.Common.Services;
using SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.Application.Services.Impl
{
    /// <summary>
    /// 积分消费任务
    /// </summary>
    public class ScoreConsumeTaskService : IScoreConsumeTaskService, IScoped
    {
        private readonly IDistributedIDGenerator _idGenerator;
        private readonly IRepository<ScoreConsumeTask> _scoreConsumeTaskRepository;
        private readonly IRepository<ScoreObtainTask> _scoreObtainTaskRepository;
        private readonly IGrpcClientResolver _grpcClientResolver;

        public ScoreConsumeTaskService(IDistributedIDGenerator idGenerator
            , IRepository<ScoreConsumeTask> scoreConsumeTaskRepository
            , IRepository<ScoreObtainTask> scoreObtainTaskRepository
            , IGrpcClientResolver grpcClientResolver)
        {
            _idGenerator = idGenerator;
            _scoreConsumeTaskRepository = scoreConsumeTaskRepository;
            _scoreObtainTaskRepository = scoreObtainTaskRepository;
            _grpcClientResolver = grpcClientResolver;
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
                var appListData = await appCenterClient.GetAppByEventTypeAsync(new GetAppByEventTypeRequest { EventType = 4 });
                foreach (var item in appListData.GetAppByEventTypeList)
                {
                    AppItems.Add(item.AppName, item.AppCode);
                }
            }
            catch
            {
                //AppItems = new Dictionary<string, string>();
                AppItems.Add("通用检索", "articlesearch");
            }

            var initData = new
            {
                taskData = new ScoreConsumeTaskDto
                {
                    ID = _idGenerator.CreateGuid(),
                },
                AppItems
            };
            return await Task.FromResult(initData);
        }

        /// <summary>
        /// 通过应用编码消费积分事件
        /// </summary>
        public async Task<Dictionary<string, string>> GetConsumeEventsByAppCode(string appCode)
        {
            var EventItems = new Dictionary<string, string>();
            try
            {
                var appCenterClient = _grpcClientResolver.EnsureClient<AppCenterGrpcService.AppCenterGrpcServiceClient>();
                var appListData = await appCenterClient.GetAppEventByCodeAsync(new GetAppEventByCodeRequest { EventType = 4, AppCode = appCode });
                foreach (var item in appListData.GetAppEventByCodeList)
                {
                    EventItems.Add(item.EventName, item.EventCode);
                }
            }
            catch
            {
                //EventItems = new Dictionary<string, string>();
                EventItems.Add("文档下载", "ArticleDownload");
            }
            return EventItems;
        }

        /// <summary>
        /// 获取任务列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<ScoreConsumeListItemDto>> QueryListData()
        {
            var listQuery = _scoreConsumeTaskRepository.DetachedEntities.Where(x => !x.DeleteFlag).ProjectToType<ScoreConsumeListItemDto>();
            var dataList = await listQuery.OrderByDescending(x => x.IsActive).ThenByDescending(x => x.CreateTime).ToListAsync();
            return dataList;
        }

        /// <summary>
        /// 获取积分消费任务详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ScoreConsumeTaskDto> GetByID(Guid id)
        {
            var taskEntity = await _scoreConsumeTaskRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == id);
            if (taskEntity == null)
            {
                throw Oops.Oh("未找到积分消费任务数据").StatusCode(Consts.Consts.ExceptionStatus);
            }
            var taskData = taskEntity.Adapt<ScoreConsumeTaskDto>();
            return taskData;
        }

        /// <summary>
        /// 新增积分消费任务
        /// </summary>
        /// <param name="consumeTask"></param>
        /// <returns></returns>
        public async Task<Guid> Create(ScoreConsumeTaskDto consumeTask)
        {
            consumeTask.ID = _idGenerator.CreateGuid();
            var taskEntity = consumeTask.Adapt<ScoreConsumeTask>();
            //检查事件是否配置为积分获取事件
            if (_scoreObtainTaskRepository.DetachedEntities.Any(x => !x.DeleteFlag && x.AppCode == consumeTask.AppCode && x.EventCode == consumeTask.EventCode))
            {
                throw Oops.Oh("该应用事件已配置为积分获取事件，不能再配置为积分消费事件").BadRequest();
            }
            //检查是否重复配置积分消费事件
            if (_scoreConsumeTaskRepository.DetachedEntities.Any(x => !x.DeleteFlag && x.AppCode == consumeTask.AppCode && x.EventCode == consumeTask.EventCode && x.Id != consumeTask.ID))
            {
                throw Oops.Oh("该应用事件已配置积分消费事件，不能重复配置").BadRequest();
            }
            var entityEntry = await _scoreConsumeTaskRepository.InsertAsync(taskEntity);
            return entityEntry.Entity.Id;
        }

        /// <summary>
        /// 编辑积分消费任务
        /// </summary>
        /// <param name="consumeTask"></param>
        /// <returns></returns>
        public async Task<Guid> Update(ScoreConsumeTaskDto consumeTask)
        {
            var taskEntity = await _scoreConsumeTaskRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == consumeTask.ID);
            if (taskEntity == null)
            {
                throw Oops.Oh("未找到积分消耗任务数据").StatusCode(Consts.Consts.ExceptionStatus);
            }
            taskEntity = consumeTask.Adapt(taskEntity);
            taskEntity.UpdateTime = DateTime.Now;
            //检查事件是否配置为积分获取事件
            if (_scoreObtainTaskRepository.DetachedEntities.Any(x => !x.DeleteFlag && x.AppCode == consumeTask.AppCode && x.EventCode == consumeTask.EventCode))
            {
                throw Oops.Oh("该应用事件已配置为积分获取事件，不能再配置为积分消费事件").BadRequest();
            }
            //检查是否重复配置积分消费事件
            if (_scoreConsumeTaskRepository.DetachedEntities.Any(x => !x.DeleteFlag && x.AppCode == consumeTask.AppCode && x.EventCode == consumeTask.EventCode && x.Id != consumeTask.ID))
            {
                throw Oops.Oh("该应用事件已配置积分消费事件，不能重复配置").BadRequest();
            }
            var entityEntry = await _scoreConsumeTaskRepository.UpdateAsync(taskEntity);
            return entityEntry.Entity.Id;
        }

        /// <summary>
        /// 设置是否激活
        /// </summary>
        /// <param name="statusSet"></param>
        /// <returns></returns>
        public async Task<bool> SetActiveStatus(ScoreConsumeTaskStatus statusSet)
        {
            var taskEntity = await _scoreConsumeTaskRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == statusSet.ID);
            if (taskEntity == null)
            {
                throw Oops.Oh("未找到积分消耗任务数据").StatusCode(Consts.Consts.ExceptionStatus);
            }
            taskEntity.IsActive = statusSet.IsActive;
            taskEntity.UpdateTime = DateTime.Now;
            await _scoreConsumeTaskRepository.UpdateAsync(taskEntity);
            return true;
        }

        /// <summary>
        /// 删除积分任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(Guid id)
        {
            var taskEntity = await _scoreConsumeTaskRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (taskEntity == null)
            {
                throw Oops.Oh("未找到积分消费任务数据").StatusCode(Consts.Consts.ExceptionStatus);
            }
            taskEntity.DeleteFlag = true;
            taskEntity.UpdateTime = DateTime.Now;
            await _scoreConsumeTaskRepository.UpdateAsync(taskEntity);
            return true;
        }










    }
}
