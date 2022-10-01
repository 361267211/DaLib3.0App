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
    public class SchedulerLogEntityService
    {
        private SqlSugarClient _dbClient;

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
            return _dbClient.Insertable(schedulerLogEntity).ExecuteCommand();
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
            try
            {
                var result = _dbClient.Queryable<SchedulerLogEntity>().ToPageList(pageIndex, pageSize, ref total);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

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
            try
            {
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
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
