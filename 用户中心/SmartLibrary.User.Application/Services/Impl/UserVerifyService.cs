/*********************************************************
* 名    称：UserVerifyService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户认证服务
* 更新历史：
*
* *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DataValidation;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SmartLibrary.MsgCenter.RpcService;
using SmartLibrary.User.Application.Dtos.User;
using SmartLibrary.User.Application.Services.Interface;
using SmartLibrary.User.Common.Extensions;
using SmartLibrary.User.Common.Services;
using SmartLibrary.User.Common.Utils;
using System;
using System.Threading.Tasks;
using static SmartLibrary.MsgCenter.RpcService.MsgCenterGrpcService;

namespace SmartLibrary.User.Application.Services.Impl
{
    /// <summary>
    /// 用户认证服务
    /// </summary>
    public class UserVerifyService : IUserVerifyService, IScoped
    {
        private readonly ITenantDistributedCache _distributeCache;
        private readonly IGrpcClientResolver _grpcClientResolver;
        private readonly IRepository<EntityFramework.Core.Entitys.User> _userRepository;
        private readonly Base64Crypt _baseEncrypt;
        public UserVerifyService(ITenantDistributedCache distributedCache
            , IRepository<EntityFramework.Core.Entitys.User> userRepository
            , IGrpcClientResolver grpcClientResolver)
        {
            var codeTable = FileHelper.ReadFileToText("EncodeTable.cert");
            _baseEncrypt = new Base64Crypt(codeTable);
            _distributeCache = distributedCache;
            _userRepository = userRepository;
            _grpcClientResolver = grpcClientResolver;
        }
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        private string GetCode()
        {
            string checkCode = String.Empty;
            int iSeed = DateTime.Now.Millisecond;
            System.Random random = new Random(iSeed);
            for (int i = 0; i < 6; i++)
            {
                int number = random.Next(10);
                checkCode += number.ToString();
            }
            return checkCode;
        }

        /// <summary>
        /// 发送手机验证码，返回对应key
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public async Task<string> SendMobileVerifyCode(string mobile)
        {
            var validateResult = mobile.TryValidate(ValidationTypes.PhoneNumber);
            if (!validateResult.IsValid)
            {
                throw Oops.Oh("请输入正确格式的手机号码").StatusCode(Consts.Consts.ExceptionStatus);
            }
            var verifyCode = GetCode();
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
            await _distributeCache.SetAsync<string>(verifyKey, verifyCode, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
            return verifyKey;
        }

        /// <summary>
        /// 检查手机号是否重复并发送手机验证码，返回对应key
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public async Task<string> CheckAndSendMobileVerifyCode(Guid userId, string mobile)
        {
            var validateResult = mobile.TryValidate(ValidationTypes.PhoneNumber);
            if (!validateResult.IsValid)
            {
                throw Oops.Oh("请输入正确格式的手机号码").StatusCode(Consts.Consts.ExceptionStatus);
            }
            var encodePhone = _baseEncrypt.Encode(mobile);
            var existPhone = await _userRepository.DetachedEntities.AnyAsync(x => !x.DeleteFlag && x.Id != userId && x.Phone == encodePhone);
            if (existPhone)
            {
                throw Oops.Oh("系统已有用户使用当前手机号，请联系管理员处理").StatusCode(Consts.Consts.ExceptionStatus);
            }
            var verifyCode = GetCode();
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
            await _distributeCache.SetAsync<string>(verifyKey, verifyCode, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
            return verifyKey;
        }

        /// <summary>
        ///  绑定手机
        /// </summary>
        /// <param name="mobileBind"></param>
        /// <returns></returns>
        public async Task<bool> BindMobile(BindMobileDto mobileBind)
        {
            var validateResult = mobileBind.Mobile.TryValidate(ValidationTypes.PhoneNumber);
            if (!validateResult.IsValid)
            {
                throw Oops.Oh("手机号格式错误").StatusCode(Consts.Consts.ExceptionStatus);
            }
            var checkCode = await _distributeCache.GetAsync<string>(mobileBind.Key);
            if (checkCode.IsNullOrWhiteSpace())
            {
                throw Oops.Oh("验证码已过期").StatusCode(Consts.Consts.ExceptionStatus);
            }
            if (checkCode != mobileBind.Code)
            {
                throw Oops.Oh("验证码错误").StatusCode(Consts.Consts.ExceptionStatus);
            }
            var userEntity = await _userRepository.FirstOrDefaultAsync(x => x.Id == mobileBind.UserId);
            if (userEntity == null)
            {
                throw Oops.Oh("未找到读者信息").StatusCode(Consts.Consts.ExceptionStatus);
            }
            var encodePhone = _baseEncrypt.Encode(mobileBind.Mobile);
            var existPhone = await _userRepository.DetachedEntities.AnyAsync(x => !x.DeleteFlag && x.Id != mobileBind.UserId && x.Phone == encodePhone);
            if (existPhone)
            {
                throw Oops.Oh("系统已有用户使用当前手机号，请联系管理员处理").StatusCode(Consts.Consts.ExceptionStatus);
            }
            userEntity.Phone = _baseEncrypt.Encode(mobileBind.Mobile);
            userEntity.MobileIdentity = true;
            userEntity.UpdateTime = DateTime.Now;
            await _userRepository.UpdateAsync(userEntity);
            return true;
        }



        /// <summary>
        /// 发送邮箱验证码
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<string> SendEmailVerifyCode(string email)
        {
            var validateResult = email.TryValidate(ValidationTypes.EmailAddress);
            if (!validateResult.IsValid)
            {
                throw Oops.Oh("请输入正确格式的邮箱地址");
            }
            var verifyCode = GetCode();
            var msgCenterGrpcClient = _grpcClientResolver.EnsureClient<MsgCenterGrpcServiceClient>();
            var request = new EmailVerifyCodeInput { Address = email, Code = verifyCode };
            var grpcResult = await FriendlyGrpc<MessageResult>.WrapResultAsync(async () =>
            {
                var innerResult = await msgCenterGrpcClient.SendEmailVerifyCodeMessageAsync(request);
                return innerResult;
            });
            if (!grpcResult.Succ)
            {
                throw Oops.Oh(grpcResult.Exception?.Message);
            }
            var verifyKey = Guid.NewGuid().ToString();
            //发送邮件逻辑
            await _distributeCache.SetAsync<string>(verifyKey, verifyCode, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
            return verifyKey;
        }

        public async Task<string> CheckAndSendEmailVerifyCode(Guid userId, string email)
        {
            var validateResult = email.TryValidate(ValidationTypes.EmailAddress);
            if (!validateResult.IsValid)
            {
                throw Oops.Oh("请输入正确格式的邮箱地址");
            }
            var encodeEmail = _baseEncrypt.Encode(email);
            var existEmail = await _userRepository.DetachedEntities.AnyAsync(x => !x.DeleteFlag && x.Id != userId && x.Email == encodeEmail);
            if (existEmail)
            {
                throw Oops.Oh("系统已有用户使用当前邮箱地址，请联系管理员处理").StatusCode(Consts.Consts.ExceptionStatus);
            }
            var verifyCode = GetCode();
            var msgCenterGrpcClient = _grpcClientResolver.EnsureClient<MsgCenterGrpcServiceClient>();
            var request = new EmailVerifyCodeInput { Address = email, Code = verifyCode };
            var grpcResult = await FriendlyGrpc<MessageResult>.WrapResultAsync(async () =>
            {
                var innerResult = await msgCenterGrpcClient.SendEmailVerifyCodeMessageAsync(request);
                return innerResult;
            });
            if (!grpcResult.Succ)
            {
                throw Oops.Oh(grpcResult.Exception?.Message);
            }
            var verifyKey = Guid.NewGuid().ToString();
            //发送邮件逻辑
            await _distributeCache.SetAsync<string>(verifyKey, verifyCode, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
            return verifyKey;
        }


        /// <summary>
        /// 绑定邮件
        /// </summary>
        /// <param name="bindData"></param>
        /// <returns></returns>
        public async Task<bool> BindEmail(BindEmailDto bindData)
        {
            var validateResult = bindData.Email.TryValidate(ValidationTypes.EmailAddress);
            if (!validateResult.IsValid)
            {
                throw Oops.Oh("邮箱地址格式错误").StatusCode(Consts.Consts.ExceptionStatus);
            }
            var checkCode = await _distributeCache.GetAsync<string>(bindData.Key);
            if (checkCode.IsNullOrWhiteSpace())
            {
                throw Oops.Oh("验证码已过期").StatusCode(Consts.Consts.ExceptionStatus);
            }
            if (checkCode != bindData.Code)
            {
                throw Oops.Oh("验证码错误").StatusCode(Consts.Consts.ExceptionStatus);
            }
            var userEntity = await _userRepository.FirstOrDefaultAsync(x => x.Id == bindData.UserId);
            if (userEntity == null)
            {
                throw Oops.Oh("未找到读者信息").StatusCode(Consts.Consts.ExceptionStatus);
            }
            var encodeEmail = _baseEncrypt.Encode(bindData.Email);
            var existEmail = await _userRepository.DetachedEntities.AnyAsync(x => !x.DeleteFlag && x.Id != bindData.UserId && x.Email == encodeEmail);
            if (existEmail)
            {
                throw Oops.Oh("系统已有用户使用当前邮箱地址，请联系管理员处理").StatusCode(Consts.Consts.ExceptionStatus);
            }
            userEntity.Email = _baseEncrypt.Encode(bindData.Email);
            userEntity.EmailIdentity = true;
            userEntity.UpdateTime = DateTime.Now;
            await _userRepository.UpdateAsync(userEntity);
            return true;
        }

        /// <summary>
        /// 修改读者身份证号
        /// </summary>
        /// <param name="bindIdCard"></param>
        /// <returns></returns>
        public async Task<bool> SetUserIdCard(BindIdCardDto bindIdCard)
        {
            if (string.IsNullOrWhiteSpace(bindIdCard.IdCard))
            {
                throw Oops.Oh("身份证号不能为空").StatusCode(Consts.Consts.ExceptionStatus);
            }
            var userEntity = await _userRepository.FirstOrDefaultAsync(x => x.Id == bindIdCard.UserId);
            if (userEntity == null)
            {
                throw Oops.Oh("未找到读者信息").StatusCode(Consts.Consts.ExceptionStatus);
            }
            var encodeIdCard = _baseEncrypt.Encode(bindIdCard.IdCard);
            if (await _userRepository.AnyAsync(x => !x.DeleteFlag && x.Id != bindIdCard.UserId && x.IdCard == encodeIdCard))
            {
                throw Oops.Oh("身份证号已存在").StatusCode(Consts.Consts.ExceptionStatus);
            }
            userEntity.IdCard = _baseEncrypt.Encode(bindIdCard.IdCard); ;
            //userEntity.IdCardIdentity = true;
            userEntity.UpdateTime = DateTime.Now;
            await _userRepository.UpdateAsync(userEntity);
            return true;
        }

    }
}
