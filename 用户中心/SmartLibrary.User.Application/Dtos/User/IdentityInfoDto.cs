/*********************************************************
* 名    称：IdentityInfoDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户认证信息
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.Dtos.User
{
    /// <summary>
    /// 账号登录信息
    /// </summary>
    public class AccountInfoDto
    {
        /// <summary>
        /// 卡号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
    /// <summary>
    /// 身份证登录信息
    /// </summary>
    public class IdCardInfoDto
    {
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdCard { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
    /// <summary>
    /// 手机号信息
    /// </summary>
    public class PhoneInfoDto
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
    }
    /// <summary>
    /// 读者卡查询
    /// </summary>
    public class CardSearchDto
    {
        /// <summary>
        /// 卡号
        /// </summary>
        public string No { get; set; }
    }
    /// <summary>
    /// 读者卡查询结果
    /// </summary>
    public class CardSearchResultDto
    {
        /// <summary>
        /// 结果编码
        /// </summary>
        public string Code { get; set; } = "";
        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrMsg { get; set; } = "";
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; } = "";
        /// <summary>
        /// 卡标识
        /// </summary>
        public string CardId { get; set; } = "";
    }
    /// <summary>
    /// 登录结果
    /// </summary>
    public class LoginResultDto
    {
        /// <summary>
        /// 结果编码
        /// </summary>
        public string Code { get; set; } = "";
        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrMsg { get; set; } = "";
        /// <summary>
        /// 用户识别码
        /// </summary>
        public string UserKey { get; set; } = "";
        /// <summary>
        /// 是否馆员
        /// </summary>
        public bool IsStaff { get; set; }

    }
    /// <summary>
    /// 卡密码修改
    /// </summary>
    public class CardTokenInfoDto
    {
        /// <summary>
        /// 卡Id
        /// </summary>
        public string CardId { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }

    /// <summary>
    /// 馆员工作台修改密码
    /// </summary>
    public class CardChangePwdDto : CardTokenInfoDto
    {
        /// <summary>
        /// 旧密码
        /// </summary>
        public string OldPwd { get; set; }
    }

    /// <summary>
    /// 卡信息
    /// </summary>
    public class CardSingleInfo
    {
        /// <summary>
        /// 卡ID
        /// </summary>
        public string CardId { get; set; }

        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNo { get; set; }

        /// <summary>
        /// 是否主卡
        /// </summary>
        public bool IsPrincipal { get; set; }
    }

    public class SimpleResultDto
    {
        /// <summary>
        /// 结果编码
        /// </summary>
        public string Code { get; set; } = "";
        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrMsg { get; set; } = "";
    }

    public class RegisterUserInfoDto
    {
        public RegisterUserInfoDto()
        {
            UserData = new UserInfoDto();
        }
        /// <summary>
        /// 是否需要审批
        /// </summary>
        public bool NeedConfirm { get; set; }
        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfoDto UserData { get; set; }
    }
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfoDto
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
    /// <summary>
    /// 注册结果
    /// </summary>
    public class RegisterResultDto
    {
        /// <summary>
        /// 结果编码
        /// </summary>
        public string Code { get; set; } = "";
        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrMsg { get; set; } = "";
        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNo { get; set; } = "";
    }
}
