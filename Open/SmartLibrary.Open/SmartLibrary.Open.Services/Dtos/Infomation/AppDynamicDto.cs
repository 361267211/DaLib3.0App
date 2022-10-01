/*********************************************************
* 名    称：AppDynamicDto.cs
* 作    者：刘启平
* 联系方式：电话[13627622058],邮件[83379242@qq.com]
* 创建时间：20210909
* 描    述：应用动态数据
* 更新历史：
*
* *******************************************************/
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.Open.Services.Dtos
{
    /// <summary>
    /// 应用动态数据模型
    /// </summary>
    public class AppDynamicDto
    {
        /// <summary>
        /// 动态ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 应用ID
        /// </summary>
        [Required(ErrorMessage = "请选择应用")]
        public Guid AppID { get; set; }

        /// <summary>
        /// 应用分支ID
        /// </summary>
        public Guid? AppBranchID { get; set; }
        /// <summary>
        /// 消息类型，0:版本变更，1：应用通知
        /// </summary>
        public int InfoType { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }
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

    }
}
