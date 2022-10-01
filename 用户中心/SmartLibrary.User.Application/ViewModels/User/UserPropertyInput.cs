/*********************************************************
* 名    称：UserPropertyInput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户属性设置
* 更新历史：
*
* *******************************************************/
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 用户属性设置
    /// </summary>
    public class UserPropertyInput
    {
        /// <summary>
        /// 属性ID
        /// </summary>
        [Required]
        public Guid PropertyID { get; set; }
        /// <summary>
        /// 属性值
        /// </summary>
        [StringLength(300,ErrorMessage ="属性值超长")]
        public string PropertyValue { get; set; }
    }
}
