using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Dtos.SceneUse
{
    /// <summary>
    /// 应用中心-前台请求参数
    /// </summary>
    public class SceneUseRequsetParameter
    {
        /// <summary>
        /// 栏目ID
        /// </summary>
        public string ColumnId { get; set; }

        /// <summary>
        /// 显示条数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 排序规则 
        /// </summary>
        public string OrderRule { get; set; }
    }
}
