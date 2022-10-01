using SqlSugar;
using System;

namespace Scheduler.Service.Entity
{
    /// <summary>
    /// 任务调度实体
    /// </summary>
    [SqlSugar.SugarTable("SchedulerEntity")]
    public class SchedulerEntity
    {

        public SchedulerEntity()
        {
            CreatedTime = DateTime.Now;
        }

        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 租户ID，相同任务不同租户，需要创建多条任务记录
        /// </summary>
        [SugarColumn(Length = 100)]
        public string TenantId { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        [SugarColumn(Length = 100)]
        public string JobName { get; set; }

        /// <summary>
        /// 任务分组
        /// </summary>
        [SugarColumn(Length = 100)]
        public string JobGroup { get; set; }

        /// <summary>
        /// 运行频率设置
        /// </summary>
        [SugarColumn(Length = 200, IsNullable = true)]
        public string Cron { get; set; }

        /// <summary>
        /// 任务所在DLL对应的程序集名称
        /// </summary>
        [SugarColumn(Length = 200, IsNullable = true)]
        public string AssemblyFullName { get; set; }

        /// <summary>
        /// 任务所在类
        /// </summary>
        [SugarColumn(Length = 200, IsNullable = true)]
        public string ClassFullName { get; set; }

        /// <summary>
        /// 任务执行参数
        /// </summary>
        [SugarColumn(Length = 2000, IsNullable = true)]
        public string TaskParam { get; set; }

        /// <summary>
        /// 适配器程序集
        /// </summary>
        [SugarColumn(Length = 200, IsNullable = true)]
        public string AdapterAssemblyFullName { get; set; }

        /// <summary>
        /// 适配器类
        /// </summary>
        [SugarColumn(Length = 200, IsNullable = true)]
        public string AdapterClassFullName { get; set; }

        /// <summary>
        /// 适配器参数JSON格式
        /// </summary>
        [SugarColumn(Length = 2000, IsNullable = true)]
        public string AdapterParm { get; set; }


        /// <summary>
        /// 任务状态
        /// </summary>
        public JobStatus JobStatus { get; set; }

        /// <summary>
        /// 任务创建时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? CreatedTime { get; set; }

        /// <summary>
        /// 下次执行时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? NextTime { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? BeginTime { get; set; }


        /// <summary>
        /// 停止时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? EndTime { get; set; }



        /// <summary>
        /// 执行间隔时间, 秒为单位
        /// </summary>
        public int IntervalSecond { get; set; }

        /// <summary>
        /// 是否重复执行
        /// </summary>
        public bool IsRepeat { get; set; }

        /// <summary>
        /// 执行次数
        /// </summary>
        public int RunTimes { get; set; }

        /// <summary>
        /// 重试次数
        /// </summary>
        public int RetryTimes { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public int IsDelete { get; set; }


        /// <summary>
        /// 任务备注
        /// </summary>
        [SugarColumn(Length = 200, IsNullable = true)]
        public string Remark { get; set; }

        /// <summary>
        /// 任务运频率中文说明
        /// </summary>
        [SugarColumn(Length = 1000, IsNullable = true)]
        public string CronRemark { get; set; }


    }
}
