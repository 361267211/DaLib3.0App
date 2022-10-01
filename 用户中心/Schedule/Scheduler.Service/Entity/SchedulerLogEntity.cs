using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Service.Entity
{
    /// <summary>
    /// 任务执行情况日志
    /// </summary>
    [SqlSugar.SugarTable("SchedulerLogEntity")]
    public class SchedulerLogEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        [SugarColumn(Length = 100)]
        public string TenantId { get; set; }

        /// <summary>
        /// 任务ID
        /// </summary>
        public int JobId { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        [SugarColumn(Length = 200)]
        public string JobName { get; set; }

        /// <summary>
        /// 任务参数
        /// </summary>
        [SugarColumn(Length =2000, IsNullable = true)]
        public string Params { get; set; }

        /// <summary>
        /// 批处理ID,适用于同一任务分步骤记录
        /// </summary>
        [SugarColumn(Length =50)]
        public string BatchId { get; set; }

        /// <summary>
        /// 日志内容
        /// </summary>
        [SugarColumn(ColumnDataType = "text", IsNullable = true)]
        public string Context { get; set; }

        /// <summary>
        /// 运行开始时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 运行结束时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? EndTime { get; set; }


        /// <summary>
        /// 任务分析后的状态 -1失败 0执行中 1成功 2部分成功
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 下次执行时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? NextFireTime { get; set; }
    }
}
