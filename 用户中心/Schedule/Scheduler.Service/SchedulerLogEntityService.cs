using Microsoft.Extensions.Configuration;
using Scheduler.Service.Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Service
{
    /// <summary>
    /// SchedulerLogEntityService
    /// </summary>
    public class SchedulerLogEntityService
    {
        private SqlSugarClient _dbClient;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public SchedulerLogEntityService(IConfigurationRoot config)
        {
            _dbClient = DataHelper.GetInstance(config);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="schedulerLogEntity"></param>
        /// <returns></returns>
        public int AddSchedulerLogEntity(SchedulerLogEntity schedulerLogEntity)
        {
            var id = _dbClient.Insertable(schedulerLogEntity).ExecuteReturnIdentity();
            return id;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="schedulerLogEntity"></param>
        /// <returns></returns>
        public int UpdateSchedulerLogEntity(SchedulerLogEntity schedulerLogEntity)
        {
            _dbClient.Updateable(schedulerLogEntity).ExecuteCommand();
            return schedulerLogEntity.Id;
        }

        /// <summary>
        /// 通过Id获取日志
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SchedulerLogEntity GetSchedulerLogEntity(int id)
        {
            var entity = _dbClient.Queryable<SchedulerLogEntity>().Where(x => x.Id == id).First();
            return entity;
        }


        /// <summary>
        /// 通过jobID获取SchedulerLogEntity
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public SchedulerLogEntity GetSchedulerLogEntityByJobId(int jobId)
        {
            return _dbClient.Queryable<SchedulerLogEntity>().Where(c => c.JobId == jobId && c.Status == 1)
                .OrderBy(c => c.CreateTime, OrderByType.Desc).First();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobName"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public bool CheckLogExpired(string jobName, TimeSpan timeSpan)
        {
            var expiredTime = DateTime.Now.AddSeconds(-timeSpan.TotalSeconds);
            _dbClient.Updateable<SchedulerLogEntity>()
                .SetColumns(it => it.Context == it.Context + "\n执行任务未正常完成")
                .SetColumns(it => it.Status == -1)
                .Where(x => x.Status == 0 && x.JobName == jobName && expiredTime > x.StartTime).ExecuteCommand();
            return true;
        }

        /// <summary>
        /// 多条新增
        /// </summary>
        /// <param name="schedulerLogEntityList"></param>
        /// <returns></returns>
        public int AddSchedulerLogEntityList(List<SchedulerLogEntity> schedulerLogEntityList)
        {
            return _dbClient.Insertable(schedulerLogEntityList).ExecuteCommand();
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<SchedulerLogEntity> GetJobLogPage(int pageIndex, int pageSize, out int total)
        {
            total = 0;
            var result = _dbClient.Queryable<SchedulerLogEntity>().ToPageList(pageIndex, pageSize, ref total);
            return result;
        }


        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<SchedulerLogEntity> GetJobLogPage(int pageIndex, int pageSize, out int total, string beginTime = null, string endTime = null, int jobState = -1, int jobID = -1, string jobName = null)
        {
            total = 0;
            var result = _dbClient.Queryable<SchedulerLogEntity>()
                .WhereIF(jobID != -1, c => c.JobId == jobID)
                .WhereIF(jobName != null, c => c.JobName.Contains(jobName))
                .WhereIF(beginTime != null, c => c.CreateTime >= Convert.ToDateTime(beginTime))
                .WhereIF(endTime != null, c => c.CreateTime <= Convert.ToDateTime(endTime))
                .OrderBy(c => c.CreateTime, OrderByType.Desc)
                .OrderBy(c => c.Id, OrderByType.Desc)
                .ToPageList(pageIndex, pageSize, ref total);
            return result;
        }
    }
}
