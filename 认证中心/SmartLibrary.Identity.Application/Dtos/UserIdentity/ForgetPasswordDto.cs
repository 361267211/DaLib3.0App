using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application.Dtos.UserIdentity
{
    public class CardSearchDto
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
        /// 验证码
        /// </summary>
        public string ValidateCode { get; set; }

    }

    public class PhoneCodeDto
    {
        public string OperateKey { get; set; }
        public string Phone { get; set; }
        public string VerifyKey { get; set; }
        public string VerifyCode { get; set; }
    }

    public class CardChangeDto
    {
        public string OperateKey { get; set; }
        public string Password { get; set; }
    }

    public class CardSearchResultDto
    {
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
    public class RegisterPhoneDto
    {
        public string VerifyKey { get; set; }
        public string VerifyCode { get; set; }
        public string Phone { get; set; }
    }

    /// <summary>
    /// 注册手机数据
    /// </summary>
    public class RegisterPhoneData
    {
        public string Phone { get; set; }
    }

    /// <summary>
    /// 用户注册信息
    /// </summary>
    public class RegisterUserDto
    {
        public string OperateKey { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string NickName { get; set; }
        public string Unit { get; set; }
        public string Edu { get; set; }
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
