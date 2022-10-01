using Furion.DataValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services.Dtos
{
    /// <summary>
    /// 积分事件模型
    /// </summary>
    public class PointsEventDto
    {
        /// <summary>
        /// 积分事件ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 应用ID
        /// </summary>
        [Required(ErrorMessage = "应用不能为空")]
        public Guid AppID { get; set; }
        /// <summary>
        /// 事件名称
        /// </summary>
        [Required(ErrorMessage = "请输入事件名称")]
        [MaxLength(20, ErrorMessage = "事件名称最多输入20个字符")]
        public string EventName { get; set; }
        /// <summary>
        /// 事件描述
        /// </summary>
        [Required(ErrorMessage = "请输入事件描述")]
        [MaxLength(100, ErrorMessage = "事件描述最多输入100个字符")]
        public string EventDesc { get; set; }
        /// <summary>
        /// 积分类型，增或减
        /// </summary>
        public int CalcType { get; set; }
        /// <summary>
        /// 分值
        /// </summary>
        [DataValidation(ValidationTypes.PositiveNumber, ErrorMessage = "请输入正数分值")]
        public decimal Point { get; set; }

    }
}
