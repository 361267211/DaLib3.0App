/*********************************************************
 * 名    称：AppEvent
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/11/9 21:51:11
 * 描    述：
 *
 * 更新历史：
 *
 * *******************************************************/
using Furion.DatabaseAccessor;
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.Open.EntityFramework.Core.Entitys
{
    public class AppEvent:Entity<Guid>
    {
        /// <summary>
        /// 应用标识
        /// </summary>
        [StringLength(48), Required]
        public string AppId { get; set; }

        /// <summary>
        /// 事件Code，用于订阅和发布
        /// 格式：{AppRouteCode}_{EventName}
        /// </summary>
        [StringLength(50), Required]
        public string EventCode { get; set; }

        /// <summary>
        /// 事件名称
        /// </summary>
        [StringLength(20), Required]
        public string EventName { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        [StringLength(100), Required]
        public string EventDesc { get; set; }

        /// <summary>
        /// 事件类型：1 运行日志，2 操作日志 ，3 积分获取，4 积分消费，5 待办项，0 所有，多个用逗号分隔
        /// </summary>
        [StringLength(20)]
        public string EventType { get; set; } 

        /// <summary>
        /// 删除标识
        /// </summary>
        [Required]
        public bool DeleteFlag { get; set; }

    }
}
