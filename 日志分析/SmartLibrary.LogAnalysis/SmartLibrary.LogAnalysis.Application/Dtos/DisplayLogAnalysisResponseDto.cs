using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.LogAnalysis.Application.Dtos
{
    /// <summary>
    /// 首页本官数据的结果项
    /// </summary>
    public class DisplayLogAnalysisResponseDto
    {
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 显示的名称
        /// </summary>
        public string DisplayTitle { get; set; }
    }
}
