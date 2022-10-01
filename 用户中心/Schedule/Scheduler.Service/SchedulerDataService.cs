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
    public class SchedulerDataService
    {
        private SqlSugarClient db;

        private IConfigurationRoot _config;
        public SchedulerDataService(IConfigurationRoot config)
        {
            _config = config;
            db = DataHelper.GetInstance(config);
        }

        /// <summary>
        /// 获取所有的计划
        /// </summary>
        /// <returns></returns>
        public List<SchedulerEntity> GetSchedulerEntityList(bool exceptIsDelete = false)
        {
            if (exceptIsDelete)
                return db.Queryable<SchedulerEntity>().Where(c => c.IsDelete == 0).ToList();
            else
                return db.Queryable<SchedulerEntity>().ToList();
        }


        /// <summary>
        /// 根据类名,租户Id，获取业务
        /// </summary>
        /// <param name="className"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public SchedulerEntity GetSchedulerEntity(string className, string tenantId)
        {
            return db.Queryable<SchedulerEntity>().Where(c => c.ClassFullName == className && c.TenantId == tenantId).First();
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="id">任务id</param>
        /// <returns></returns>
        public bool DeleteSchedulerEntity(int id)
        {
            var schedulerEntity = db.Queryable<SchedulerEntity>().First(c => c.Id == id);
            if (schedulerEntity == null || schedulerEntity.IsDelete == 1)
            {
                return true;
            }
            schedulerEntity.IsDelete = 1;
            schedulerEntity.EndTime = DateTime.Now;
            db.Updateable(schedulerEntity).ExecuteCommand();
            return true;
        }
    }
}
