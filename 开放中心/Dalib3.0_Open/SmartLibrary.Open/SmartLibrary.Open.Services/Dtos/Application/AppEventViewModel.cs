/*********************************************************
 * 名    称：AppEventViewModel
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/11/10 14:26:40
 * 描    述：应用事件视图模型
 *
 * 更新历史：
 *
 * *******************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services.Dtos
{
    public class AppEventViewModel
    {
        /// <summary>
        /// 事件Code，用于订阅和发布
        /// 格式：{AppRouteCode}_{EventName}
        /// </summary>
        public string EventCode { get; set; }

        /// <summary>
        /// 事件名称
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// 事件类型：1 运行日志，2 操作日志 ，3 积分获取，4 积分消费，5 待办项，0 所有，多个用逗号分隔
        /// </summary>
        public string EventType { get; set; }
    }
}
