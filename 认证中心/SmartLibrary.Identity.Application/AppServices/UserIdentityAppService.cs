using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.Identity.Application.Dtos.Common;
using SmartLibrary.Identity.Application.Dtos.UserIdentity;
using SmartLibrary.Identity.Application.Filter;
using SmartLibrary.Identity.Application.Services.Interface;
using SmartLibrary.Identity.Application.ViewModels;
using SmartLibrary.Identity.Application.ViewModels.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application.AppServices
{
    /// <summary>
    /// 用户认证服务
    /// </summary>

    [AllowAnonymous]
    public class UserIdentityAppService : BaseAppService
    {
        private readonly IUserIdentityService _userIdentityService;

        public UserIdentityAppService(IUserIdentityService userIdentityService)
        {
            _userIdentityService = userIdentityService;
        }

        /// <summary>
        /// 用户Cas登录Api
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        [CapActionEvent("UserIdentify_Reader_Login", "读者登录")]
        public async Task<CasLoginResult> CasLogin([FromQuery] CasLoginInput input)
        {
            //调用用户服务校验账号密码
            var loginAccount = input.Adapt<CasLoginDto>();
            var result = await _userIdentityService.LoginCasByAccount(loginAccount);
            var targetResult = result.Adapt<CasLoginResult>();
            return targetResult;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<LoginResult> LoginByAccount([FromBody] LoginAccountInput input)
        {
            //调用用户服务校验账号密码
            var loginAccount = input.Adapt<LoginAccountDto>();
            var result = await _userIdentityService.LoginByAccount(loginAccount);
            var targetResult = result.Adapt<LoginResult>();
            return targetResult;
        }
        /// <summary>
        /// 通过手机号登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<LoginResult> LoginByPhone([FromBody] LoginPhoneInput input)
        {
            //调用用户服务手机校验
            var loginPhone = input.Adapt<LoginPhoneDto>();
            var result = await _userIdentityService.LoginByPhone(loginPhone);
            var targetResult = result.Adapt<LoginResult>();
            return targetResult;
        }

        /// <summary>
        /// 通过身份证号登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<LoginResult> LoginByIdcard([FromBody] LoginIdCardInput input)
        {
            var loginIdCard = input.Adapt<LoginIdCardDto>();
            var result = await _userIdentityService.LoginByIdCard(loginIdCard);
            var targetResult = result.Adapt<LoginResult>();
            return targetResult;
        }

        /// <summary>
        /// 通过卡号查询读者卡
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<string> SearchCardByNo([FromBody] CardSearchInput input)
        {
            var searchDto = input.Adapt<CardSearchDto>();
            var result = await _userIdentityService.SearchCardByNo(searchDto);
            return result;
        }

        /// <summary>
        /// 发送手机验证码
        /// </summary>
        /// <param name="phoneVerifyInfo"></param>
        /// <returns></returns>
        public async Task<string> SendMobileVerifyCode([FromBody] PhoneVerifyInput phoneVerifyInfo)
        {
            var phoneVerifyDto = phoneVerifyInfo.Adapt<PhoneVerifyDto>();
            var verifyKey = await _userIdentityService.SendMobileVerifyCode(phoneVerifyDto);
            return verifyKey;
        }

        /// <summary>
        /// 发送手机验证码（忘记密码）
        /// </summary>
        /// <param name="phoneVerifyInfo"></param>
        /// <returns></returns>
        public async Task<string> SendMobileVerifyCodeForget([FromBody] PhoneVerifyForgetInput phoneVerifyInfo)
        {
            var phoneVerifyDto = phoneVerifyInfo.Adapt<PhoneVerifyForgetDto>();
            var verifyKey = await _userIdentityService.SendMobileVerifyCodeForget(phoneVerifyDto);
            return verifyKey;
        }

        /// <summary>
        /// 获取验证码信息
        /// </summary>
        /// <returns></returns>
        public async Task<ValidateCodeInfoOutput> GetValidateCode()
        {
            var validateInfoDto = await _userIdentityService.GetValidateCode();
            var validateInfo = validateInfoDto.Adapt<ValidateCodeInfoOutput>();
            return validateInfo;
        }

        /// <summary>
        /// 验证手机验证码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> VerifyPhoneCode([FromBody] PhoneCodeInput input)
        {
            var phoneCodeDto = input.Adapt<PhoneCodeDto>();
            var result = await _userIdentityService.VerifyPhoneCode(phoneCodeDto);
            return result;
        }

        /// <summary>
        /// 修改读者卡密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> ChangeCardPassword([FromBody] CardChangeInput input)
        {
            var changeDto = input.Adapt<CardChangeDto>();
            var result = await _userIdentityService.ChangeCardPassword(changeDto);
            return result;
        }

        /// <summary>
        /// 手机验证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<string> CheckUniquePhone([FromBody] RegisterPhoneInput input)
        {
            var registerPhone = input.Adapt<RegisterPhoneDto>();
            var result = await _userIdentityService.CheckUniquePhone(registerPhone);
            return result;
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<string> RegisterUser([FromBody] RegisterUserInput input)
        {
            var registerUser = input.Adapt<RegisterUserDto>();
            var result = await _userIdentityService.RegisterUser(registerUser);
            return result;
        }
    }
}
