using SmartLibrary.Identity.Application.Dtos.UserIdentity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application.Services.Interface
{
    public interface IUserIdentityService
    {
        /// <summary>
        /// 通过账号密码登录
        /// </summary>
        /// <param name="loginAccount"></param>
        /// <returns></returns>
        public Task<CasLoginResultDto> LoginCasByAccount(CasLoginDto loginAccount);
        /// <summary>
        /// 通过账号密码登录
        /// </summary>
        /// <param name="loginAccount"></param>
        /// <returns></returns>
        public Task<LoginResultDto> LoginByAccount(LoginAccountDto loginAccount);
        /// <summary>
        /// 通过手机号码登录
        /// </summary>
        /// <param name="loginPhone"></param>
        /// <returns></returns>
        public Task<LoginResultDto> LoginByPhone(LoginPhoneDto loginPhone);
        /// <summary>
        /// 通过身份证号码登录
        /// </summary>
        /// <param name="loginIdCard"></param>
        /// <returns></returns>
        public Task<LoginResultDto> LoginByIdCard(LoginIdCardDto loginIdCard);
        /// <summary>
        /// 通过卡号查询读者卡
        /// </summary>
        /// <param name="cardSearch"></param>
        /// <returns></returns>
        public Task<string> SearchCardByNo(CardSearchDto cardSearch);

        /// <summary>
        /// 生成验证码信息
        /// </summary>
        /// <returns></returns>
        public Task<ValidateCodeInfoDto> GetValidateCode();

        /// <summary>
        /// 手机号发送验证码
        /// </summary>
        /// <param name="phoneVerifyData"></param>
        /// <returns></returns>
        Task<string> SendMobileVerifyCode(PhoneVerifyDto phoneVerifyData);
        /// <summary>
        /// 手机号发送验证码
        /// </summary>
        /// <param name="phoneVerifyData"></param>
        /// <returns></returns>
        Task<string> SendMobileVerifyCodeForget(PhoneVerifyForgetDto phoneVerifyData);

        /// <summary>
        /// 验证手机验证码
        /// </summary>
        /// <param name="phoneInfo"></param>
        /// <returns></returns>
        public Task<bool> VerifyPhoneCode(PhoneCodeDto phoneInfo);

        /// <summary>
        /// 修改读者卡密码
        /// </summary>
        /// <param name="changeInfo"></param>
        /// <returns></returns>
        public Task<bool> ChangeCardPassword(CardChangeDto changeInfo);

        /// <summary>
        /// 检查手机号是否唯一
        /// </summary>
        /// <param name="registerPhone"></param>
        /// <returns></returns>
        public Task<string> CheckUniquePhone(RegisterPhoneDto registerPhone);
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        public Task<string> RegisterUser(RegisterUserDto userData);

    }
}
