/*********************************************************
* 名    称：ActivityInfoDto.cs
* 作    者：刘启平
* 联系方式：电话[13627622058],邮件[83379242@qq.com]
* 创建时间：20210910
* 描    述：活动消息数据模型
* 更新历史：
*
* *******************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.Open.Services.Dtos.Infomation
{
    /// <summary>
    /// 活动消息数据模型
    /// </summary>
    public class ActivityInfoDto
    {
        /// <summary>
        /// 活动消息ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 消息ID
        /// </summary>

        public Guid InfoID { get; set; }
        /// <summary>
        /// 消息标题
        /// </summary>
        [MinLength(2, ErrorMessage = "标题输入2-100个字符")]
        [MaxLength(100, ErrorMessage = "标题输入2-100个字符")]
        public string Title { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        /// <summary>
        /// 消息内容
        /// </summary>
        [Required(ErrorMessage = "消息内容不能为空")]
        [MaxLength(4000, ErrorMessage = "消息内容最多输入4000个字符")]
        public string Content { get; set; }
        /// <summary>
        /// 发布日期
        /// </summary>
        public DateTime PublishDate { get; set; }
        /// <summary>
        /// 状态0：正常，1：置顶
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 发布范围客户ID集合
        /// </summary>
        public List<Guid> SpecificCustomerIds { get; set; }
        /// <summary>
        /// 是否公开消息
        /// </summary>
        public bool IsPublic { get; set; }
    }
}
