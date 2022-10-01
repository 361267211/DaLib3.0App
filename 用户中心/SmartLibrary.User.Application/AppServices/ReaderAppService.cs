/*********************************************************
* 名    称：ReaderAppService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220301
* 描    述：读者操作Api，对应用户中心读者前台数据获取及操作
* 更新历史：
*
* *******************************************************/
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.User.Application.Dtos.Common;
using SmartLibrary.User.Application.Dtos.DataApprove;
using SmartLibrary.User.Application.Dtos.User;
using SmartLibrary.User.Application.Filter;
using SmartLibrary.User.Application.Services.Consts;
using SmartLibrary.User.Application.Services.Interface;
using SmartLibrary.User.Application.ViewModels;
using SmartLibrary.User.Application.ViewModels.Reader;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.AppServices
{
    /// <summary>
    /// 读者服务
    /// </summary>
    [Authorize(Policy = PolicyKey.ReaderAuth)]
    public class ReaderAppService : BaseAppService
    {

        private readonly IUserService _userService;
        private readonly ICardService _cardService;
        private readonly IDataApproveService _dataApproveService;
        private readonly IUserVerifyService _userVerifyService;
        private readonly IRegionService _regionService;

        /// <summary>
        /// 服务初始化
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="cardService"></param>
        /// <param name="dataApproveService"></param>
        /// <param name="userVerifyService"></param>
        /// <param name="regionService"></param>
        public ReaderAppService(IUserService userService,
                                ICardService cardService,
                                IDataApproveService dataApproveService,
                                IUserVerifyService userVerifyService,
                                IRegionService regionService)
        {
            _userService = userService;
            _cardService = cardService;
            _dataApproveService = dataApproveService;
            _userVerifyService = userVerifyService;
            _regionService = regionService;
        }

        /// <summary>
        /// 获取读者InitData
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetInitData()
        {
            return await _userService.GetReaderInitData();
        }

        /// <summary>
        /// 获取读者自己详细信息
        /// </summary>
        /// <returns></returns>
        public async Task<ReaderAndCardInfoOutput> GetReaderSelfDetailInfo()
        {
            if (CurrentUser == null)
            {
                throw Oops.Oh("未获取到读者信息");
            }
            var userInfo = await _userService.GetByID(CurrentUser.UserID);
            var readerInfo = userInfo.Adapt<ReaderAndCardInfoOutput>();
            return readerInfo;
        }

        /// <summary>
        /// 获取修改读者信息权限
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<bool> CheckModifyReaderPermit()
        {
            if (CurrentUser == null)
            {
                throw Oops.Oh("未获取到读者信息");
            }
            var updateReaderInfo = await _userPermissionService.CheckModifyReaderInfo(CurrentUser.UserKey);
            return updateReaderInfo;
        }

        /// <summary>
        /// 获取读者领卡权限
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<bool> CheckCardClaimPermit()
        {
            if (CurrentUser == null)
            {
                throw Oops.Oh("未获取到读者信息");
            }
            var cardClaim = await _userPermissionService.CheckCardClaimPermit(CurrentUser.UserKey);
            return cardClaim;
        }


        /// <summary>
        /// 读者获取自己的读者信息
        /// </summary>
        /// <returns></returns>
        public async Task<ReaderDetailOutput> GetReaderInfo()
        {
            if (CurrentUser == null)
            {
                throw Oops.Oh("未获取到读者信息");
            }
            var userInfo = await _userService.GetByID(CurrentUser.UserID);
            var readerInfo = userInfo.Adapt<ReaderDetailOutput>();
            return readerInfo;
        }

        /// <summary>
        /// 编辑读者自己的属性信息
        /// </summary>
        /// <param name="readerData">用户信息</param>
        /// <returns></returns>
        public async Task<Guid> UpdateReaderInfo([FromBody] ReaderEditInput readerData)
        {
            if (CurrentUser == null)
            {
                throw Oops.Oh("未获取到读者信息");
            }
            var updateReaderInfo = await _userPermissionService.CheckModifyReaderInfo(CurrentUser.UserKey);
            if (!updateReaderInfo)
            {
                throw Oops.Oh("当前读者无修改信息权限").StatusCode(Consts.ExceptionStatus);
            }
            var readerDto = readerData.Adapt<ReaderEditDto>();
            var result = await _userService.UpdateReaderInfo(readerDto, CurrentUser.UserID);
            return CurrentUser.UserID;
        }

        /// <summary>
        /// 发送手机验证码
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]/{mobile}")]
        public async Task<string> SendMobileVerfiyCode(string mobile)
        {
            if (CurrentUser == null)
            {
                throw Oops.Oh("未获取到读者信息").StatusCode(Consts.ExceptionStatus);
            }
            var verifyKey = await _userVerifyService.CheckAndSendMobileVerifyCode(CurrentUser.UserID, mobile);
            return verifyKey;
        }

        /// <summary>
        /// 绑定手机号码
        /// </summary>
        /// <param name="verifyData"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        [CapActionEvent("UserManage_Bind_Mobile", "绑定手机")]
        public async Task<bool> BindMobileAndCode([FromBody] ReaderMobileVerifyInput verifyData)
        {
            if (CurrentUser == null)
            {
                throw Oops.Oh("未获取到读者信息");
            }
            var bindMobile = new BindMobileDto
            {
                Key = verifyData.Key,
                Code = verifyData.Code,
                UserId = CurrentUser.UserID,
                Mobile = verifyData.Mobile
            };
            var bindResult = await _userVerifyService.BindMobile(bindMobile);
            return bindResult;

        }



        /// <summary>
        /// 发送邮箱验证码
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]/{email}")]
        public async Task<string> SendEmailVerifyCode(string email)
        {
            if (CurrentUser == null)
            {
                throw Oops.Oh("未获取到读者信息").StatusCode(Consts.ExceptionStatus);
            }
            var verifyKey = await _userVerifyService.CheckAndSendEmailVerifyCode(CurrentUser.UserID, email);
            return verifyKey;
        }

        /// <summary>
        /// 绑定邮箱
        /// </summary>
        /// <param name="verifyData"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        [CapActionEvent("UserManage_Bind_Email", "绑定邮箱")]
        public async Task<bool> BindEmailAndCode([FromBody] ReaderEmailVerifyInput verifyData)
        {
            if (CurrentUser == null)
            {
                throw Oops.Oh("未获取到读者信息");
            }
            var bindEmail = new BindEmailDto
            {
                Key = verifyData.Key,
                Code = verifyData.Code,
                UserId = CurrentUser.UserID,
                Email = verifyData.Email
            };
            var bindResult = await _userVerifyService.BindEmail(bindEmail);
            return bindResult;
        }

        /// <summary>
        /// 读者修改身份证号
        /// </summary>
        /// <param name="verifyData"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<bool> BindReaderIdCard([FromBody] ReaderIdCardVerifyInput verifyData)
        {
            if (CurrentUser == null)
            {
                throw Oops.Oh("未获取到读者信息");
            }
            var bindIdCard = new BindIdCardDto
            {
                UserId = CurrentUser.UserID,
                IdCard = verifyData.IdCard
            };
            var bindResult = await _userVerifyService.SetUserIdCard(bindIdCard);
            return bindResult;
        }

        /// <summary>
        /// 查询读者卡信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<CardListItemOutput>> QueryReaderCardListData()
        {
            if (CurrentUser == null)
            {
                throw Oops.Oh("未获取到读者信息");
            }
            var cardList = await _cardService.QueryUserCardListData(CurrentUser.UserID);
            var targetList = cardList.Adapt<List<CardListItemOutput>>();
            return targetList;
        }

        /// <summary>
        /// 查询读者卡申请信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<CardListItemOutput>> QueryReaderCardApplyListData()
        {
            if (CurrentUser == null)
            {
                throw Oops.Oh("未获取到读者信息");
            }
            var cardList = await _cardService.QueryUserCardApplyListData(CurrentUser.UserID);
            var targetList = cardList.Adapt<List<CardListItemOutput>>();
            return targetList;
        }

        /// <summary>
        /// 查询读者卡数据
        /// </summary>
        /// <param name="cardId">读者卡ID</param>
        /// <returns></returns>
        public async Task<CardDetailOutput> GetReaderCardData(Guid cardId)
        {
            if (CurrentUser == null)
            {
                throw Oops.Oh("未获取到读者信息");
            }
            var card = await _cardService.GetByID(cardId);
            return card;
        }

        /// <summary>
        /// 修改读者卡密码
        /// </summary>
        /// <param name="pwdChangeData">密码数据</param>
        /// <returns></returns>
        public async Task<bool> ChangeCardPassword([FromBody] ReaderCardPwdChangeInput pwdChangeData)
        {
            if (CurrentUser == null)
            {
                throw Oops.Oh("未获取到读者信息");
            }
            var result = await _cardService.ChangeCardPwd(pwdChangeData);
            return result;
        }

        /// <summary>
        /// 设置读者卡为主卡
        /// </summary>
        /// <param name="cardId">读者卡ID</param>
        /// <returns></returns>
        [Route("[action]/{cardId}")]
        public async Task<bool> SetPrincipalCard(Guid cardId)
        {
            if (CurrentUser == null)
            {
                throw Oops.Oh("未获取到读者信息");
            }
            var result = await _cardService.SetPrincipalCard(cardId, CurrentUser.UserID);
            return result;
        }

        /// <summary>
        /// 取消审核
        /// </summary>
        /// <param name="claimId">记录Id</param>
        /// <returns></returns>
        [Route("[action]/{claimId}")]
        public async Task<bool> CancelCardConfirm(Guid claimId)
        {
            if (CurrentUser == null)
            {
                throw Oops.Oh("未获取到读者信息");
            }
            var result = await _dataApproveService.CancelCardConfirm(claimId);
            return result;
        }

        /// <summary>
        /// 删除个人读者卡
        /// </summary>
        /// <param name="cardId">卡ID</param>
        /// <returns></returns>
        public async Task<bool> DeleteCard(Guid cardId)
        {
            if (CurrentUser == null)
            {
                throw Oops.Oh("未获取到读者信息");
            }
            var result = await _cardService.Delete(cardId);
            return result;
        }

        /// <summary>
        /// 删除领卡申请
        /// </summary>
        /// <param name="claimId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteCardClaim(Guid claimId)
        {
            if (CurrentUser == null)
            {
                throw Oops.Oh("未获取到读者信息");
            }
            var result = await _dataApproveService.DeleteCardClaim(claimId);
            return result;
        }

        /// <summary>
        /// 查询卡信息
        /// </summary>
        /// <param name="cardSearch"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<CardDetailOutput> SearchCardData([FromQuery] ReaderCardSearchInput cardSearch)
        {
            if (CurrentUser == null)
            {
                throw Oops.Oh("未获取到读者信息");
            }
            var cardInfo = await _cardService.GetByNoPwd(cardSearch.No, cardSearch.Pwd);
            return cardInfo;
        }

        /// <summary>
        /// 读者卡领取
        /// </summary>
        /// <param name="claimData"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<int> ClaimReaderCard([FromBody] ReaderCardClaimInput claimData)
        {
            if (CurrentUser == null)
            {
                throw Oops.Oh("未获取到读者信息");
            }
            var cardClaimPermit = await _userPermissionService.CheckCardClaimPermit(CurrentUser.UserKey);
            if (!cardClaimPermit)
            {
                throw Oops.Oh("当前读者无领卡权限").StatusCode(Consts.ExceptionStatus);
            }
            var cardInfo = await _cardService.GetByID(claimData.CardID);
            var sameUserIdentity = false;
            if (!string.IsNullOrWhiteSpace(cardInfo.UserIdCard) && cardInfo.UserIdCard == CurrentUser.UserIdCard)
            {
                sameUserIdentity = true;
            }
            if (!string.IsNullOrWhiteSpace(cardInfo.UserPhone) && cardInfo.UserPhone == CurrentUser.UserPhone)
            {
                sameUserIdentity = true;
            }
            if (!string.IsNullOrWhiteSpace(cardInfo.UserEmail) && cardInfo.UserEmail == CurrentUser.UserEmail)
            {
                sameUserIdentity = true;
            }
            var claimDto = new CardCliamEditDto
            {
                UserID = CurrentUser.UserID,
                CardID = cardInfo.ID,
                NeedConfirm = !sameUserIdentity
            };
            var result = await _dataApproveService.ClaimReaderCard(claimDto);
            return result;

        }



        /// <summary>
        /// 获取地区
        /// </summary>
        /// <returns></returns>
        public async Task<List<RegionOutput>> GetRegionList()
        {
            var orgList = await _regionService.GetRegionTree();
            return orgList;
        }
    }
}
