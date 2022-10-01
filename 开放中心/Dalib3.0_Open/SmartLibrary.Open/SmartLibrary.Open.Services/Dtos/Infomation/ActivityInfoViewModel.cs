using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services.Dtos.Infomation
{
    /// <summary>
    /// 活动消息视图模型
    /// </summary>
    public class ActivityInfoViewModel
    {
        /// <summary>
        /// 活动消息ID
        /// </summary>
        public Guid ID { get; set; }
       /// <summary>
       /// 序号
       /// </summary>
        public int SortNo { get; set; }
        public Guid InfoID { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 发布范围
        /// </summary>
        public List<string> SpecificCustomerNames { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime PublishDate { get; set; }
        /// <summary>
        /// 是否公开消息
        /// </summary>
        public bool IsPublic { get; set; }
        /// <summary>
        /// 消息状态，0：正常，1：置顶
        /// </summary>
        public int Status { get; set; }

    }
}
