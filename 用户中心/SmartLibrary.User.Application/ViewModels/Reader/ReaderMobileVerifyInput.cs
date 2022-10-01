/*********************************************************
* 名    称：ReaderMobileVerifyInput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：手机号验证
* 更新历史：
*
* *******************************************************/
using Furion.DataValidation;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 手机号验证
    /// </summary>
    public class ReaderMobileVerifyInput
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        [DataValidation(ValidationTypes.PhoneNumber, ErrorMessage = "手机号格式错误")]
        public string Mobile { get; set; }
        /// <summary>
        /// 认证码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 认证码Key
        /// </summary>
        public string Key { get; set; }
    }
}
