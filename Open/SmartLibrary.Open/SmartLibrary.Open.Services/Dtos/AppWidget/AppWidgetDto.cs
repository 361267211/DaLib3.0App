/*********************************************************
* 名    称：AppWidgetDto.cs
* 作    者：刘启平
* 联系方式：电话[13627622058],邮件[83379242@qq.com]
* 创建时间：20210913
* 描    述：应用组件数据模型
* 更新历史：
*
* *******************************************************/
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.Open.Services.Dtos
{
    /// <summary>
    /// 应用组件数据模型
    /// </summary>
    public class AppWidgetDto
    {

        /// <summary>
        /// 应用组件ID
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// 应用ID
        /// </summary>
        [Required(ErrorMessage = "请选择所属应用")]
        public Guid AppID { get; set; }
        /// <summary>
        /// 组件名称
        /// </summary>
        [Required(ErrorMessage = "请填写组件名称")]
        [MaxLength(50, ErrorMessage = "组件名称最多输入50个字符")]
        public string Name { get; set; }
        /// <summary>
        /// 组件描述信息
        /// </summary>
        [MaxLength(2000, ErrorMessage = "描述信息最多输入2000个字符")]
        public string Desc { get; set; }
        /// <summary>
        /// 组件内容地址
        /// </summary>
        [Required(ErrorMessage = "请填写组件地址")]
        [MaxLength(50, ErrorMessage = "组件地址最多输入100个字符")]
        public string Target { get; set; }
    }
}
