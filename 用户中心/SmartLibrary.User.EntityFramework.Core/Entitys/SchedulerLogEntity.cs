using Furion.DatabaseAccessor;
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.User.EntityFramework.Core.Entitys
{
    public class SchedulerLogEntity : IPrivateEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        [StringLength(100)]
        public string TenantId { get; set; }

        /// <summary>
        /// 任务ID
        /// </summary>
        public int JobId { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        [StringLength(200)]
        public string JobName { get; set; }

        /// <summary>
        /// 任务参数
        /// </summary>
        [StringLength(2000)]
        public string Params { get; set; }

        /// <summary>
        /// 批处理ID,适用于同一任务分步骤记录
        /// </summary>
        [StringLength(50)]
        public string BatchId { get; set; }

        /// <summary>
        /// 日志内容
        /// </summary>
        public string Context { get; set; }

        /// <summary>
        /// 运行开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 运行结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }


        /// <summary>
        /// 任务分析后的状态 -1未执行 0成功 1失败 2警告
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 下次执行时间
        /// </summary>
        public DateTime? NextFireTime { get; set; }
    }
}
