using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application.ViewModels
{
    public class CardSearchInput
    {
        /// <summary>
        /// 卡号
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 验证Key
        /// </summary>
        public string ValidateKey { get; set; }
        /// <summary>
        /// 验证Code
        /// </summary>
        public string ValidateCode { get; set; }

    }

    public class PhoneCodeInput
    {
        public string OperateKey { get; set; }
        public string Phone { get; set; }
        public string VerifyKey { get; set; }
        public string VerifyCode { get; set; }
    }

    public class CardChangeInput
    {
        public string OperateKey { get; set; }
        public string Password { get; set; }
    }

    public class CardSearchResult
    {
        /// <summary>
        /// 结果编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrMsg { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 卡标识
        /// </summary>
        public string CardId { get; set; }
    }

    /// <summary>
    /// 注册手机验证
    /// </summary>
    public class RegisterPhoneInput
    {
        public string VerifyKey { get; set; }
        public string Phone { get; set; }
    }

    /// <summary>
    /// 用户注册信息
    /// </summary>
    public class RegisterUserInput
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string NickName { get; set; }
        public string Unit { get; set; }
        public string Edi { get; set; }
        public string Title { get; set; }
        public string Depart { get; set; }
        public string College { get; set; }
        public string Major { get; set; }
        public string Grade { get; set; }
        public string Class { get; set; }
        public string Type { get; set; }
        public string IdCard { get; set; }
        public string Email { get; set; }
        public DateTime? Birthday { get; set; }
        public string Gender { get; set; }
        public string Addr { get; set; }
        public string AddrDetail { get; set; }
        public string Photo { get; set; }
        public DateTime? LeaveTime { get; set; }
    }


}
