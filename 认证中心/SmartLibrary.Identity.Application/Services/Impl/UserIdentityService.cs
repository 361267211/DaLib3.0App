using Furion.DatabaseAccessor;
using Furion.DataValidation;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Furion.RemoteRequest.Extensions;
using Google.Protobuf.WellKnownTypes;
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using SmartLibrary.Identity.Application.Dtos.UserIdentity;
using SmartLibrary.Identity.Application.Services.Enum;
using SmartLibrary.Identity.Application.Services.Interface;
using SmartLibrary.Identity.Application.ViewModels;
using SmartLibrary.Identity.Common.Const;
using SmartLibrary.Identity.Common.Dtos;
using SmartLibrary.Identity.Common.Extensions;
using SmartLibrary.Identity.Common.Services;
using SmartLibrary.Identity.EntityFramework.Core.Entitys;
using SmartLibrary.MsgCenter.RpcService;
using SmartLibrary.User.RpcService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static SmartLibrary.MsgCenter.RpcService.MsgCenterGrpcService;

namespace SmartLibrary.Identity.Application.Services.Impl
{
    public class UserIdentityService : IUserIdentityService, IScoped
    {
        private readonly ITenantDistributedCache _distributedCache;
        private readonly IGrpcClientResolver _grpcClientResolver;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Lazy<ICaptchaService> _captchaService;
        private readonly TenantInfo _tenantInfo;
        private readonly IRepository<RegisterConfigSet> _registerConfigRepository;

        public UserIdentityService(IGrpcClientResolver grpcClientResolver
            , IHttpContextAccessor httpContextAccessor
            , ITenantDistributedCache distributedCache
            , Lazy<ICaptchaService> captchaService
            , TenantInfo tenantInfo
            , IRepository<RegisterConfigSet> registerConfigRepository)
        {
            _grpcClientResolver = grpcClientResolver;
            _distributedCache = distributedCache;
            _httpContextAccessor = httpContextAccessor;
            _captchaService = captchaService;
            _tenantInfo = tenantInfo;
            _registerConfigRepository = registerConfigRepository;
        }

        /// <summary>
        /// 通过账号密码验证
        /// </summary>
        /// <param name="loginAccount"></param>
        /// <returns></returns>
        public async Task<CasLoginResultDto> LoginCasByAccount(CasLoginDto loginAccount)
        {
            var identityGrpcClient = _grpcClientResolver.EnsureClient<UserForIdentityGrpcService.UserForIdentityGrpcServiceClient>();
            var userGrpcClient = _grpcClientResolver.EnsureClient<UserGrpcService.UserGrpcServiceClient>();

            var accountInfo = new AccountInfo { Account = loginAccount.LoginName, Password = loginAccount.Password };
            var grpcResult = await FriendlyGrpc<User.RpcService.LoginResult>.WrapResultAsync(async () =>
            {
                var innerResult = await identityGrpcClient.LoginByAccountPwdAsync(accountInfo);
                return innerResult;
            });
            if (!grpcResult.Succ)
            {
                throw Oops.Oh(grpcResult.Exception?.Message);
            }
            var rData = grpcResult.Data;
            if (rData.Code != "200")
            {
                throw Oops.Oh($"{rData.ErrMsg}");
            }
            var userInfoResult = await FriendlyGrpc<User.RpcService.UserData>.WrapResultAsync(async () =>
            {
                var innerResult = await userGrpcClient.GetUserByKeyAsync(new StringValue { Value = rData.UserKey });
                return innerResult;
            });
            if (!userInfoResult.Succ)
            {
                throw Oops.Oh(userInfoResult.Exception?.Message);
            }
            var userData = userInfoResult.Data;
            var readerInfo = new LoginReaderInfoDto
            {
                UserKey = userData.Key,
                user_key = userData.Key,
                reader_name = userData.Name,
                login_name = userData.CardNo,
                reader_type = userData.TypeName,
                reader_title = userData.Title,
                department_name = userData.CollegeName,
                specialty_name = userData.Major,
                reader_gender = userData.Gender,
                edu_background = userData.Edu,
                username = userData.Name,
                reader_iccode = userData.CardNo,
                reader_id = userData.AsyncReaderId,
            };
            var tokenResult = await $"{ SiteGlobalConfig.TokenAddress}/api/Auth/AccessToken".SetBody(new TokenRequestInput
            {
                OrgId = "cqu",
                OrgSecret = "cqu123",
                OrgCode = _tenantInfo?.Name ?? "",
                UserKey = rData.UserKey
            }, "application/json", Encoding.UTF8).PostAsAsync<TokenResultOutput>();
            var tokenInfo = tokenResult.Data;
            var ticket = Guid.NewGuid().ToString();
            readerInfo.access_token = tokenInfo.Token;
            var claimsIdentity = new System.Security.Claims.ClaimsIdentity();
            claimsIdentity.AddClaim(new System.Security.Claims.Claim(ClaimConst.Claim_UserKey, readerInfo.UserKey));
            _httpContextAccessor.HttpContext.User.AddIdentity(claimsIdentity);
            return new CasLoginResultDto { ReaderInfo = readerInfo, Status = 200, Msg = "登录成功" };
        }

        /// <summary>
        /// 通过账号密码验证
        /// </summary>
        /// <param name="loginAccount"></param>
        /// <returns></returns>
        public async Task<LoginResultDto> LoginByAccount(LoginAccountDto loginAccount)
        {
            var checkCode = await _distributedCache.GetAsync<string>(loginAccount.ValidateKey);
            if (checkCode.IsNullOrWhiteSpace())
            {
                throw Oops.Oh("验证码已过期");
            }
            if (checkCode != loginAccount.ValidateCode)
            {
                throw Oops.Oh("验证码错误");
            }
            var identityGrpcClient = _grpcClientResolver.EnsureClient<UserForIdentityGrpcService.UserForIdentityGrpcServiceClient>();
            var accountInfo = new AccountInfo { Account = loginAccount.Account, Password = loginAccount.Password };
            var grpcResult = await FriendlyGrpc<User.RpcService.LoginResult>.WrapResultAsync(async () =>
             {
                 var innerResult = await identityGrpcClient.LoginByAccountPwdAsync(accountInfo);
                 return innerResult;
             });
            if (!grpcResult.Succ)
            {
                throw Oops.Oh(grpcResult.Exception?.Message);
            }
            var rData = grpcResult.Data;
            if (rData.Code != "200")
            {
                throw Oops.Oh($"{rData.ErrMsg}");
            }
            var tokenResult = await $"{ SiteGlobalConfig.TokenAddress}/api/Auth/AccessToken".SetBody(new TokenRequestInput
            {
                OrgId = "Test",
                OrgSecret = "Test",
                OrgCode = _tenantInfo?.Name ?? "",
                UserKey = rData.UserKey
            }, "application/json", Encoding.UTF8).PostAsAsync<TokenResultOutput>();
            var tokenInfo = tokenResult.Data;
            return new LoginResultDto { AccessToken = tokenInfo.Token, RefreshToken = loginAccount.RememberMe ? tokenInfo.RefreshToken : "" };
        }

        /// <summary>
        /// 通过身份证号登录
        /// </summary>
        /// <param name="loginIdCard"></param>
        /// <returns></returns>
        public async Task<LoginResultDto> LoginByIdCard(LoginIdCardDto loginIdCard)
        {
            var checkCode = await _distributedCache.GetAsync<string>(loginIdCard.ValidateKey);
            if (checkCode.IsNullOrWhiteSpace())
            {
                throw Oops.Oh("验证码已过期");
            }
            if (checkCode != loginIdCard.ValidateCode)
            {
                throw Oops.Oh("验证码错误");
            }
            var identityGrpcClient = _grpcClientResolver.EnsureClient<UserForIdentityGrpcService.UserForIdentityGrpcServiceClient>();
            var idCardInfo = new IdCardInfo { IdCard = loginIdCard.IdCard, Password = loginIdCard.Password };
            var grpcResult = await FriendlyGrpc<User.RpcService.LoginResult>.WrapResultAsync(async () =>
            {
                var innerResult = await identityGrpcClient.LoginByIdCardAsync(idCardInfo);
                return innerResult;
            });
            if (!grpcResult.Succ)
            {
                throw Oops.Oh(grpcResult.Exception?.Message);
            }
            var rData = grpcResult.Data;
            if (rData.Code != "200")
            {
                throw Oops.Oh($"{rData.ErrMsg}");
            }
            var tokenResult = await "http://192.168.21.71:8077/api/Auth/AccessToken".SetBody(new TokenRequestInput
            {
                OrgId = "Test",
                OrgSecret = "Test",
                OrgCode = _tenantInfo?.Name ?? "",
                UserKey = rData.UserKey
            }, "application/json", Encoding.UTF8).PostAsAsync<TokenResultOutput>();
            var tokenInfo = tokenResult.Data;
            return new LoginResultDto { AccessToken = tokenInfo.Token, RefreshToken = loginIdCard.RememberMe ? tokenInfo.RefreshToken : "" };
        }

        /// <summary>
        /// 通过手机号登录
        /// </summary>
        /// <param name="loginPhone"></param>
        /// <returns></returns>
        public async Task<LoginResultDto> LoginByPhone(LoginPhoneDto loginPhone)
        {
            var checkCode = await _distributedCache.GetAsync<string>(loginPhone.VerifyKey);
            if (checkCode.IsNullOrWhiteSpace())
            {
                throw Oops.Oh("验证码已过期");
            }
            if (checkCode != loginPhone.VerifyCode)
            {
                throw Oops.Oh("验证码错误");
            }

            var identityGrpcClient = _grpcClientResolver.EnsureClient<UserForIdentityGrpcService.UserForIdentityGrpcServiceClient>();
            var phoneInfo = new PhoneInfo { Phone = loginPhone.Phone };
            var grpcResult = await FriendlyGrpc<User.RpcService.LoginResult>.WrapResultAsync(async () =>
            {
                var innerResult = await identityGrpcClient.LoginByPhoneAsync(phoneInfo);
                return innerResult;
            });
            if (!grpcResult.Succ)
            {
                throw Oops.Oh(grpcResult.Exception?.Message);
            }
            var rData = grpcResult.Data;
            if (rData.Code != "200")
            {
                throw Oops.Oh($"{rData.ErrMsg}");
            }
            //var gatewayUrl = "";
            var tokenResult = await $"{ SiteGlobalConfig.TokenAddress}/api/Auth/AccessToken".SetBody(new TokenRequestInput
            {
                OrgId = "Test",
                OrgSecret = "Test",
                OrgCode = _tenantInfo?.Name ?? "",
                UserKey = rData.UserKey
            }, "application/json", Encoding.UTF8).PostAsAsync<TokenResultOutput>();
            var tokenInfo = tokenResult.Data;
            return new LoginResultDto { AccessToken = tokenInfo.Token, RefreshToken = loginPhone.RememberMe ? tokenInfo.RefreshToken : "" };
        }

        /// <summary>
        /// 发送手机验证码，返回对应key
        /// </summary>
        /// <param name="phoneVerifyData"></param>
        /// <returns></returns>
        public async Task<string> SendMobileVerifyCode(PhoneVerifyDto phoneVerifyData)
        {
            var mobile = phoneVerifyData.Phone;
            var validateResult = mobile.TryValidate(ValidationTypes.PhoneNumber);
            if (!validateResult.IsValid)
            {
                throw Oops.Oh("请输入正确格式的手机号码");
            }
            var checkCode = await _distributedCache.GetAsync<string>(phoneVerifyData.ValidateKey);
            if (checkCode.IsNullOrWhiteSpace())
            {
                throw Oops.Oh("验证码已过期");
            }
            if (checkCode != phoneVerifyData.ValidateCode)
            {
                throw Oops.Oh("验证码错误");
            }
            var verifyCode = _captchaService.Value.GenerateRandomNum(6);
            var msgCenterGrpcClient = _grpcClientResolver.EnsureClient<MsgCenterGrpcServiceClient>();
            var request = new PhoneVerifyCodeInput { Phone = mobile, Code = verifyCode };
            var grpcResult = await FriendlyGrpc<MessageResult>.WrapResultAsync(async () =>
            {
                var innerResult = await msgCenterGrpcClient.SendPhoneVerifyCodeMessageAsync(request);
                return innerResult;
            });
            if (!grpcResult.Succ)
            {
                throw Oops.Oh(grpcResult.Exception?.Message);
            }
            var verifyKey = Guid.NewGuid().ToString();
            //发送短信逻辑
            await _distributedCache.SetAsync<string>(verifyKey, verifyCode, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
            return verifyKey;
        }

        /// <summary>
        /// 发送手机验证码，返回对应key
        /// </summary>
        /// <param name="phoneVerifyData"></param>
        /// <returns></returns>
        public async Task<string> SendMobileVerifyCodeForget(PhoneVerifyForgetDto phoneVerifyData)
        {
            var mobile = phoneVerifyData.Phone;
            var validateResult = mobile.TryValidate(ValidationTypes.PhoneNumber);
            if (!validateResult.IsValid)
            {
                throw Oops.Oh("请输入正确格式的手机号码");
            }
            var cardSearchResult = await _distributedCache.GetAsync<CardSearchResultDto>(phoneVerifyData.OperateKey);
            if (cardSearchResult == null || cardSearchResult.CardId.IsNullOrWhiteSpace())
            {
                throw Oops.Oh("卡号验证已过期");
            }
            if (cardSearchResult.Phone.ToLower() != phoneVerifyData.Phone.ToLower())
            {
                throw Oops.Oh("输入手机号与用户手机号不一致");
            }
            var verifyCode = _captchaService.Value.GenerateRandomNum(6);
            var msgCenterGrpcClient = _grpcClientResolver.EnsureClient<MsgCenterGrpcServiceClient>();
            var request = new PhoneVerifyCodeInput { Phone = mobile, Code = verifyCode };
            var grpcResult = await FriendlyGrpc<MessageResult>.WrapResultAsync(async () =>
            {
                var innerResult = await msgCenterGrpcClient.SendPhoneVerifyCodeMessageAsync(request);
                return innerResult;
            });
            if (!grpcResult.Succ)
            {
                throw Oops.Oh(grpcResult.Exception?.Message);
            }
            var verifyKey = Guid.NewGuid().ToString();
            //发送短信逻辑
            await _distributedCache.SetAsync<string>(verifyKey, verifyCode, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
            return verifyKey;
        }

        /// <summary>
        /// 生成验证码信息
        /// </summary>
        /// <returns></returns>
        public async Task<ValidateCodeInfoDto> GetValidateCode()
        {
            var verifyCodeResult = _captchaService.Value.GenerateRandomNumImg(4);
            var verifyCode = verifyCodeResult.Code;
            var verifyKey = Guid.NewGuid().ToString();
            //发送短信逻辑
            await _distributedCache.SetAsync<string>(verifyKey, verifyCode, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
            return new ValidateCodeInfoDto
            {
                ValidateKey = verifyKey,
                ImgFile = verifyCodeResult.ImageBase64
            };
        }

        /// <summary>
        /// 验证手机号是否匹配
        /// </summary>
        /// <param name="phoneInfo"></param>
        /// <returns></returns>
        public async Task<bool> VerifyPhoneCode(PhoneCodeDto phoneInfo)
        {
            var mobile = phoneInfo.Phone;
            var validateResult = mobile.TryValidate(ValidationTypes.PhoneNumber);
            if (!validateResult.IsValid)
            {
                throw Oops.Oh("请输入正确格式的手机号码");
            }
            //暂不处理手机号验证
            var verifyCode = await _distributedCache.GetAsync<string>(phoneInfo.VerifyKey);
            if (verifyCode == null)
            {
                throw Oops.Oh("验证码已过期");
            }
            if (verifyCode != phoneInfo.VerifyCode)
            {
                throw Oops.Oh("验证码错误");
            }
            var cardInfo = await _distributedCache.GetAsync<CardSearchResultDto>(phoneInfo.OperateKey);
            if (cardInfo == null || (cardInfo.Phone.ToLower() != phoneInfo.Phone.ToLower()))
            {
                throw Oops.Oh("读者卡与手机号不匹配");
            }
            return true;
        }

        /// <summary>
        /// 通过卡号查询读者卡
        /// </summary>
        /// <param name="cardSearch"></param>
        /// <returns></returns>
        public async Task<string> SearchCardByNo(CardSearchDto cardSearch)
        {
            var checkCode = await _distributedCache.GetAsync<string>(cardSearch.ValidateKey);
            if (checkCode.IsNullOrWhiteSpace())
            {
                throw Oops.Oh("验证码已过期");
            }
            if (checkCode != cardSearch.ValidateCode)
            {
                throw Oops.Oh("验证码错误");
            }
            var identityGrpcClient = _grpcClientResolver.EnsureClient<UserForIdentityGrpcService.UserForIdentityGrpcServiceClient>();
            var cardInfo = new CardSearch { No = cardSearch.No };
            var grpcResult = await FriendlyGrpc<User.RpcService.CardSearchResult>.WrapResultAsync(async () =>
            {
                var innerResult = await identityGrpcClient.SearchCardByNoAsync(cardInfo);
                return innerResult;
            });
            if (!grpcResult.Succ)
            {
                throw Oops.Oh(grpcResult.Exception?.Message);
            }
            var rData = grpcResult.Data;
            if (rData.Code != "200")
            {
                throw Oops.Oh($"{rData.ErrMsg}");
            }
            var operateKey = Guid.NewGuid().ToString();
            await _distributedCache.SetAsync<CardSearchResultDto>(operateKey, new CardSearchResultDto { Phone = rData.Phone, CardId = rData.CardId }, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            });
            return operateKey;
        }

        /// <summary>
        /// 修改读者卡密码
        /// </summary>
        /// <param name="changeInfo"></param>
        /// <returns></returns>
        public async Task<bool> ChangeCardPassword(CardChangeDto changeInfo)
        {
            var cardInfo = await _distributedCache.GetAsync<CardSearchResultDto>(changeInfo.OperateKey);
            if (cardInfo == null)
            {
                throw Oops.Oh("未找到读者卡数据").StatusCode(Consts.Consts.ExceptionStatus);
            }
            var identityGrpcClient = _grpcClientResolver.EnsureClient<UserForIdentityGrpcService.UserForIdentityGrpcServiceClient>();
            var cardToken = new CardTokenInfo { CardId = cardInfo.CardId, Password = changeInfo.Password };
            var grpcResult = await FriendlyGrpc<SimpleResult>.WrapResultAsync(async () =>
            {
                var innerResult = await identityGrpcClient.ChangeCardPwdAsync(cardToken);
                return innerResult;
            });
            if (!grpcResult.Succ)
            {
                throw Oops.Oh(grpcResult.Exception?.Message);
            }
            var rData = grpcResult.Data;
            if (rData.Code != "200")
            {
                throw Oops.Oh($"{rData.ErrMsg}").StatusCode(Consts.Consts.ExceptionStatus);
            }
            return true;
        }

        /// <summary>
        /// 检查手机号是否唯一
        /// </summary>
        /// <param name="registerPhone"></param>
        /// <returns></returns>
        public async Task<string> CheckUniquePhone(RegisterPhoneDto registerPhone)
        {
            var checkCode = await _distributedCache.GetAsync<string>(registerPhone.VerifyKey);
            if (checkCode.IsNullOrWhiteSpace())
            {
                throw Oops.Oh("验证码已过期").StatusCode(Consts.Consts.ExceptionStatus);
            }
            if (checkCode != registerPhone.VerifyCode)
            {
                throw Oops.Oh("验证码错误").StatusCode(Consts.Consts.ExceptionStatus);
            }
            var identityGrpcClient = _grpcClientResolver.EnsureClient<UserForIdentityGrpcService.UserForIdentityGrpcServiceClient>();
            var phoneInfo = new PhoneInfo { Phone = registerPhone.Phone };
            var grpcResult = await FriendlyGrpc<SimpleResult>.WrapResultAsync(async () =>
            {
                var innerResult = await identityGrpcClient.CheckUniquePhoneAsync(phoneInfo);
                return innerResult;
            });
            if (!grpcResult.Succ)
            {
                throw Oops.Oh(grpcResult.Exception?.Message);
            }
            var rData = grpcResult.Data;
            if (rData.Code != "200")
            {
                throw Oops.Oh($"{rData.ErrMsg}").StatusCode(Consts.Consts.ExceptionStatus);
            }
            var operateKey = Guid.NewGuid().ToString();
            await _distributedCache.SetAsync<RegisterPhoneData>(operateKey, new RegisterPhoneData { Phone = registerPhone.Phone }, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            });
            return operateKey;
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        public async Task<string> RegisterUser(RegisterUserDto userData)
        {
            var phoneInfo = await _distributedCache.GetAsync<RegisterPhoneData>(userData.OperateKey);
            if (phoneInfo == null)
            {
                throw Oops.Oh("操作未经过验证");
            }
            userData.Phone = phoneInfo.Phone.Trim();
            var config = await _registerConfigRepository.FirstOrDefaultAsync(x => !x.DeleteFlag);
            var needConfirm = config.RegisteFlow == (int)EnumRegisterFlow.馆员审核;
            var identityGrpcClient = _grpcClientResolver.EnsureClient<UserForIdentityGrpcService.UserForIdentityGrpcServiceClient>();
            var registerInfo = new RegisterUserInfo
            {
                NeedConfirm = needConfirm,
                UserData = new RegisterUserInfo.Types.UserInfo
                {
                    Name = userData.Name,
                    Phone = userData.Phone,
                    NickName = userData.NickName,
                    Unit = userData.Unit,
                    Edu = userData.Edu,
                    Title = userData.Title,
                    Depart = userData.Depart,
                    College = userData.College,
                    Major = userData.Major,
                    Grade = userData.Grade,
                    Class = userData.Class,
                    Type = userData.Type,
                    IdCard = userData.IdCard,
                    Email = userData.Email,
                    Birthday = userData.Birthday.HasValue ? Timestamp.FromDateTime(userData.Birthday.Value) : null,
                    Gender = userData.Gender,
                    Addr = userData.Addr,
                    AddrDetail = userData.AddrDetail,
                    Photo = userData.Photo,
                    LeaveTime = userData.LeaveTime.HasValue ? Timestamp.FromDateTime(userData.LeaveTime.Value) : null,
                }
            };
            var grpcResult = await FriendlyGrpc<RegisterResult>.WrapResultAsync(async () =>
            {
                var innerResult = await identityGrpcClient.RegisterUserAsync(registerInfo);
                return innerResult;
            });
            if (!grpcResult.Succ)
            {
                throw Oops.Oh(grpcResult.Exception?.Message);
            }
            var rData = grpcResult.Data;
            if (rData.Code != "200")
            {
                throw Oops.Oh($"{rData.ErrMsg}");
            }
            return rData.CardNo;
        }
    }
}
