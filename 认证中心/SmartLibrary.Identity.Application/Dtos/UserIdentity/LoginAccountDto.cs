using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application.Dtos.UserIdentity
{
    public class LoginAccountDto
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 验证码Key
        /// </summary>
        public string ValidateKey { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string ValidateCode { get; set; }
        /// <summary>
        /// 是否记住登录凭据
        /// </summary>
        public bool RememberMe { get; set; }
    }

    public class LoginIdCardDto
    {
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdCard { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 验证码Key
        /// </summary>
        public string ValidateKey { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string ValidateCode { get; set; }
        /// <summary>
        /// 是否记住登录凭据
        /// </summary>
        public bool RememberMe { get; set; }
    }

    public class LoginPhoneDto
    {
        /// <summary>
        /// 验证码Key
        /// </summary>
        public string VerifyKey { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string VerifyCode { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 是否记住
        /// </summary>
        public bool RememberMe { get; set; }
    }

    public class LoginResultDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }

    public class CasLoginResultDto
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

    public class CasLoginDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        /// <example>superAdmin</example>
        public string LoginName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        /// <example>123456</example>
        public string Password { get; set; }
    }
}
