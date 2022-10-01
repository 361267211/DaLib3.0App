using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services.Dtos
{
    /// <summary>
    /// 积分事件视图模型
    /// </summary>
    public class PointsEventViewModel
    {
        /// <summary>
        /// 积分事件ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 应用ID
        /// </summary>
        public Guid AppID { get; set; }
        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; }
        /// <summary>
        /// 事件名称
        /// </summary>
        public string EventName { get; set; }
        /// <summary>
        /// 事件描述
        /// </summary>
        public string EventDesc { get; set; }
        /// <summary>
        /// 积分类型，增或减
        /// </summary>
        public int CalcType { get; set; }
        /// <summary>
        /// 分值
        /// </summary>
        public decimal Point { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 事件识别码
        /// </summary>
        public Guid? Token { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

    }
}
