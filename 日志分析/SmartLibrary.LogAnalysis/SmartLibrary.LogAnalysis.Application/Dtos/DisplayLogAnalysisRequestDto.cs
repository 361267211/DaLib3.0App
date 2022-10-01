using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.LogAnalysis.Application.Dtos
{
    /// <summary>
    /// 首页的本馆数据接口入参
    /// </summary>
    public class DisplayLogAnalysisRequestDto
    {
        /// <summary>
        /// 类型 1服务数据 2资源数据
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 范围 1 昨天 3 最近三天 7本周 30本月 365本年 
        /// 如果指定了开始时间和结束时间，则该参数将被忽略
        /// </summary>
        public int Range { get; set; }
        /// <summary>
        /// 手动指定开始时间
        /// </summary>
        public DateTime? From { get; set; }
        /// <summary>
        /// 手动指定结束时间
        /// </summary>
        public DateTime? To { get; set; }
    }
}
