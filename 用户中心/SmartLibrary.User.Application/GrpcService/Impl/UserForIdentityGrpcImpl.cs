/*********************************************************
* 名    称：UserForIdentityGrpcImpl.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户认证服务
* 更新历史：
*
* *******************************************************/
using Furion.DependencyInjection;
using Grpc.Core;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using SmartLibrary.User.Application.Dtos.User;
using SmartLibrary.User.Application.GrpcService.Interface;
using SmartLibrary.User.Application.Services.Interface;
using System.Threading.Tasks;
using SmartLibrary.User.RpcService;

namespace SmartLibrary.User.Application.GrpcService.Impl
{
    [Authorize]
    public class UserForIdentityGrpcImpl : UserForIdentityGrpcService.UserForIdentityGrpcServiceBase, IIdentityGrpcService, IScoped
    {
        private readonly IUserService _userService;
        public UserForIdentityGrpcImpl(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 用户账号密码登录
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<LoginResult> LoginByAccountPwd(AccountInfo request, ServerCallContext context)
        {
            var accountDto = request.Adapt<AccountInfoDto>();
            var result = await _userService.LoginByAccountPwd(accountDto);
            var targetResult = result.Adapt<LoginResult>();
            return targetResult;
        }

        /// <summary>
        /// 身份证号密码登录
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<LoginResult> LoginByIdCard(IdCardInfo request, ServerCallContext context)
        {
            var idCardDto = request.Adapt<IdCardInfoDto>();
            var result = await _userService.LoginByIdCardPwd(idCardDto);
            var targetResult = result.Adapt<LoginResult>();
            return targetResult;
        }

        /// <summary>
        /// 手机号登录
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<LoginResult> LoginByPhone(PhoneInfo request, ServerCallContext context)
        {
            var phoneDto = request.Adapt<PhoneInfoDto>();
            var result = await _userService.LoginByPhone(phoneDto);
            var targetResult = result.Adapt<LoginResult>();
            return targetResult;
        }

        /// <summary>
        /// 通过卡号查询读者卡
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<CardSearchResult> SearchCardByNo(CardSearch request, ServerCallContext context)
        {
            var cardSearchDto = request.Adapt<CardSearchDto>();
            var result = await _userService.SearchCardByNo(cardSearchDto);
            var targetResult = result.Adapt<CardSearchResult>();
            return targetResult;
        }

        /// <summary>
        /// 修改读者卡密码
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<SimpleResult> ChangeCardPwd(CardTokenInfo request, ServerCallContext context)
        {
            var cardTokenInfo = request.Adapt<CardTokenInfoDto>();
            var result = await _userService.ChangeCardPwd(cardTokenInfo);
            var targetResult = result.Adapt<SimpleResult>();
            return targetResult;
        }

        /// <summary>
        /// 修改卡密码，需要验证旧密码
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<SimpleResult> ChangeCardPwdEx(CardChangePwdInfo request, ServerCallContext context)
        {
            var cardTokenInfo = request.Adapt<CardChangePwdDto>();
            var result = await _userService.ChangeCardPwdEx(cardTokenInfo);
            var targetResult = result.Adapt<SimpleResult>();
            return targetResult;
        }

        /// <summary>
        /// 根据userkey获取卡列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<CardReply> GetCardList(CardRequest request, ServerCallContext context)
        {
            var result = new CardReply();
            var cardList = await _userService.GetCardListByUserKey(request.UserKey);

            cardList.ForEach(c =>
            {
                result.CardList.Add(new CardSingle
                {
                    CardId = c.CardId,
                    CardNo = c.CardNo,
                    IsPrincipal = c.IsPrincipal
                });
            });

            return result;
        }

        /// <summary>
        /// 检查手机号是否已被使用
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<SimpleResult> CheckUniquePhone(PhoneInfo request, ServerCallContext context)
        {
            var phoneDto = request.Adapt<PhoneInfoDto>();
            var result = await _userService.CheckUniquePhone(phoneDto);
            var targetResult = result.Adapt<SimpleResult>();
            return targetResult;
        }

        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<RegisterResult> RegisterUser(RegisterUserInfo request, ServerCallContext context)
        {
            var registerUserDto = request.Adapt<RegisterUserInfoDto>();
            var result = await _userService.RegisterUser(registerUserDto);
            var targetResult = result.Adapt<RegisterResult>();
            return targetResult;
        }
    }
}
