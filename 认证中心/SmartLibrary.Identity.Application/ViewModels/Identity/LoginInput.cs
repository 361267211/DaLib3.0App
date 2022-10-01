using SmartLibrary.Identity.Application.Dtos.UserIdentity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application.ViewModels
{
    public class LoginAccountInput
    {
        /// <summary>
        /// 用户名
        /// </summary>
        /// <example>superAdmin</example>
        [Required(ErrorMessage = "用户名不能为空")]
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        /// <example>123456</example>
        [Required(ErrorMessage = "密码不能为空")]
        public string Password { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        /// <example>code</example>
        [Required(ErrorMessage = "验证码不能为空")]
        public string ValidateCode { get; set; }

        /// <summary>
        /// 验证Key
        /// </summary>
        /// <example>key</example>
        [Required(ErrorMessage = "验证key不能为空")]
        public string ValidateKey { get; set; }
        /// <summary>
        /// 记住账号
        /// </summary>
        public bool RememberMe { get; set; }
    }

    public class LoginIdCardInput
    {
        /// <summary>
        /// 用户名
        /// </summary>
        /// <example>superAdmin</example>
        [Required(ErrorMessage = "身份证号不能为空")]
        public string IdCard { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        /// <example>123456</example>
        [Required(ErrorMessage = "密码不能为空")]
        public string Password { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        /// <example>code</example>
        [Required(ErrorMessage = "验证码不能为空")]
        public string ValidateCode { get; set; }

        /// <summary>
        /// 验证Key
        /// </summary>
        /// <example>key</example>
        [Required(ErrorMessage = "验证key不能为空")]
        public string ValidateKey { get; set; }
        /// <summary>
        /// 记住账号
        /// </summary>
        public bool RememberMe { get; set; }
    }

    public class LoginPhoneInput
    {
        /// <summary>
        /// 手机号
        /// </summary>
        /// <example>superAdmin</example>
        [Required(ErrorMessage = "手机号不能为空")]
        public string Phone { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        /// <example>code</example>
        [Required(ErrorMessage = "验证码不能为空")]
        public string VerifyCode { get; set; }

        /// <summary>
        /// 验证Key
        /// </summary>
        /// <example>key</example>
        [Required(ErrorMessage = "验证key不能为空")]
        public string VerifyKey { get; set; }

        /// <summary>
        /// 记住账号
        /// </summary>
        public bool RememberMe { get; set; }
    }

    public class LoginResult
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }

    public class CasLoginResult
    {
        /// <summary>
        /// 登录信息
        /// </summary>
        public LoginReaderInfoDto ReaderInfo { get; set; }

        /// <summary>
        /// 验证状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 原始密码
        /// </summary>
        public string VirginPassword { get; set; }
    }

    public class PhoneVerifyInput
    {
        public string Phone { get; set; }
        public string validateKey { get; set; }
        public string validateCode { get; set; }
    }

    public class PhoneVerifyForgetInput
    {
        public string Phone { get; set; }
        public string OperateKey { get; set; }

    }
}
