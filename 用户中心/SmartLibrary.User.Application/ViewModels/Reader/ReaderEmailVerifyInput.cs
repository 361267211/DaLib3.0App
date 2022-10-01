/*********************************************************
* 名    称：ReaderEmailVerifyInput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：读者邮箱验证
* 更新历史：
*
* *******************************************************/
using Furion.DataValidation;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 读者邮箱验证
    /// </summary>
    public class ReaderEmailVerifyInput
    {
        /// <summary>
        /// 邮箱地址
        /// </summary>
        [DataValidation(ValidationTypes.EmailAddress)]
        public string Email { get; set; }
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
