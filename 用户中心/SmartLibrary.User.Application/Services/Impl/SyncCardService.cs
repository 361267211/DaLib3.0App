/*********************************************************
* 名    称：SyncCardService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：读者卡同步服务
* 更新历史：
*
* *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.User.Application.Dtos.Card;
using SmartLibrary.User.Application.Services.Enum;
using SmartLibrary.User.Application.Services.Interface;
using SmartLibrary.User.Application.ViewModels;
using SmartLibrary.User.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.Services.Impl
{
    /// <summary>
    /// 同步读者卡任务
    /// </summary>
    public class SyncCardService : ISyncCardService, IScoped
    {
        public const string JobName = "DefaultSyncCardRepeatJob";
        public const string TempJobName = "DefaultSyncCardOnceJob";
        public const string JobGroupName = "SyncCardJobGroup";
        private readonly IRepository<SchedulerEntity> _schedulerEntityRepository;
        private readonly IRepository<SchedulerLogEntity> _schedulerLogEntityRepository;

        public SyncCardService(IRepository<SchedulerEntity> schedulerEntityRepository,
            IRepository<SchedulerLogEntity> schedulerLogEntityRepository)
        {
            _schedulerEntityRepository = schedulerEntityRepository;
            _schedulerLogEntityRepository = schedulerLogEntityRepository;
        }

        /// <summary>
        /// 获取同步读者卡配置
        /// </summary>
        /// <param name="tenant"></param>
        /// <returns></returns>
        public async Task<SyncCardConfigItemDto> GetSyncCardConfig(string tenant)
        {
            var schedulerEntity = await _schedulerEntityRepository.DetachedEntities.FirstOrDefaultAsync(x => x.TenantId == tenant && x.IsDelete == 0 && x.JobName == JobName);
            if (schedulerEntity == null)
            {
                return new SyncCardConfigItemDto();
            }
            var configData = schedulerEntity.Adapt<SyncCardConfigItemDto>();
            return configData;
        }

        /// <summary>
        /// 设置读者卡同步配置
        /// </summary>
        /// <param name="tenant"></param>
        /// <param name="syncConfig"></param>
        /// <returns></returns>
        public async Task<bool> SetSyncCardConfig(string tenant, SyncCardConfigItemDto syncConfig)
        {

            var schedulerEntity = await _schedulerEntityRepository.DetachedEntities.FirstOrDefaultAsync(x => x.TenantId == tenant && x.IsDelete == 0 && x.JobName == JobName);
            if (schedulerEntity == null)
            {
                //不存在则新建
                schedulerEntity = new SchedulerEntity
                {
                    TenantId = tenant,
                    JobName = JobName,
                    JobGroup = JobGroupName,
                    Cron = syncConfig.Cron,
                    CronRemark = syncConfig.CronRemark,
                    AssemblyFullName = syncConfig.AssemblyFullName,
                    ClassFullName = syncConfig.ClassFullName,
                    TaskParam = syncConfig.TaskParam,
                    AdapterAssemblyFullName = syncConfig.AdapterAssemblyFullName,
                    AdapterClassFullName = syncConfig.AdapterClassFullName,
                    AdapterParm = syncConfig.AdapterParm,
                    JobStatus = (int)EnumSyncCardStatus.运行,
                    CreatedTime = DateTime.Now,
                    BeginTime = DateTime.Now,
                    IsRepeat = true,
                    Remark = "读者卡同步系统任务"
                };
                await _schedulerEntityRepository.InsertAsync(schedulerEntity);
            }
            else
            {
                schedulerEntity.Cron = syncConfig.Cron;
                schedulerEntity.CronRemark = syncConfig.CronRemark;
                schedulerEntity.AssemblyFullName = syncConfig.AssemblyFullName;
                schedulerEntity.ClassFullName = syncConfig.ClassFullName;
                schedulerEntity.TaskParam = syncConfig.TaskParam;
                schedulerEntity.AdapterAssemblyFullName = syncConfig.AdapterAssemblyFullName;
                schedulerEntity.AdapterClassFullName = syncConfig.AdapterClassFullName;
                schedulerEntity.AdapterParm = syncConfig.AdapterParm;
                schedulerEntity.JobStatus = (int)EnumSyncCardStatus.运行;
                schedulerEntity.BeginTime = DateTime.Now;
                await _schedulerEntityRepository.UpdateAsync(schedulerEntity);
            }
            return true;
        }

        /// <summary>
        /// 添加读者卡临时同步任务
        /// </summary>
        /// <param name="tenant"></param>
        /// <returns></returns>
        public async Task<bool> SetSyncCardTask(string tenant)
        {
            var schedulerEntity = await _schedulerEntityRepository.DetachedEntities.FirstOrDefaultAsync(x => x.TenantId == tenant && x.IsDelete == 0 && x.JobName == JobName);
            if (schedulerEntity == null)
            {
                throw Oops.Oh("同步任务未正确配置");
            }
            //if (await _schedulerEntityRepository.AnyAsync(x => x.IsDelete == 0 && x.JobName == TempJobName && x.JobStatus == (int)EnumSyncCardStatus.运行))
            //{
            //    throw Oops.Oh("当前存在执行中的读者卡同步任务，不能添加");
            //}
            //不存在则新建
            var tempSchedulerEntity = new SchedulerEntity
            {
                TenantId = tenant,
                JobName = TempJobName,
                JobGroup = JobGroupName,
                Cron = "",
                CronRemark = "",
                AssemblyFullName = schedulerEntity.AssemblyFullName,
                ClassFullName = schedulerEntity.ClassFullName,
                TaskParam = schedulerEntity.TaskParam,
                AdapterAssemblyFullName = schedulerEntity.AdapterAssemblyFullName,
                AdapterClassFullName = schedulerEntity.AdapterClassFullName,
                AdapterParm = schedulerEntity.AdapterParm,
                JobStatus = (int)EnumSyncCardStatus.运行,
                CreatedTime = DateTime.Now,
                BeginTime = DateTime.Now.AddSeconds(10),
                RunTimes = 1,//执行一次
                IntervalSecond = 600,
                IsRepeat = false,
                Remark = "读者卡临时同步系统任务"
            };
            await _schedulerEntityRepository.InsertAsync(tempSchedulerEntity);
            return true;
        }

        /// <summary>
        /// 获取同步读者卡日志
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <param name="tenant"></param>
        /// <returns></returns>
        public async Task<PagedList<SyncCardLogListItemDto>> GetSyncCardLogTableData(string tenant, SyncCardLogTableQuery queryFilter)
        {
            var filterJobNames = new[] { JobName, TempJobName };
            var taskLogQuery = _schedulerLogEntityRepository.DetachedEntities.Where(x => x.TenantId == tenant && filterJobNames.Contains(x.JobName)).OrderByDescending(x => x.CreateTime)
                .Select(x => new SyncCardLogListItemDto
                {
                    Id = x.Id,
                    SyncStartTime = x.StartTime,
                    SyncEndTime = x.EndTime,
                    SyncType = (int)EnumSyncCardType.增量同步,
                    Status = x.Status,
                    Context = x.Context
                });

            return await taskLogQuery.ToPagedListAsync(queryFilter.PageIndex, queryFilter.PageSize);

        }

    }
}
