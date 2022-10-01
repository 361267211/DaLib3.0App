using SmartLibrary.Core.Cap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application.Dtos.Cap
{
    /// <summary>
    /// 名    称：NewsServiceSubscribeEvent
    /// 作    者：张泽军
    /// 创建时间：2021/9/7 19:45:34
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NewsServiceSubscribeEvent: SmartCapSubscribeEventBase
    {/// <summary>
     /// 事件名称
     /// </summary>
        [SmartCapEventBind(typeof(NewNewsColumnMsg))]
        [Description("新闻服务发布新增新闻栏目消息")]
        public const string NewNewsColumn = "VipSmart.NewsService.NewNewsColumn";//公有云.服务名.事件名
        /// <summary>
        /// 消息接收类型
        /// </summary>
        public class NewNewsColumnMsg : SmartCapEventMsgBase
        {
            [Description("新闻栏目Id")]
            public int ColumnID { get; set; }
        }
    }
}
