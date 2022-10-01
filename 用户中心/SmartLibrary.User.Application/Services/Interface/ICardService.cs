/*********************************************************
* 名    称：ICardService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：读者卡数据服务
* 更新历史：
*
* *******************************************************/
using Furion.DependencyInjection;
using SmartLibrary.User.Application.Dtos.Card;
using SmartLibrary.User.Application.Dtos.PropertyGroup;
using SmartLibrary.User.Application.ViewModels;
using SmartLibrary.User.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.Services.Interface
{
    /// <summary>
    /// 卡数据服务
    /// </summary>
    public interface ICardService : IScoped
    {
        /// <summary>
        /// 读者卡校验
        /// </summary>
        /// <param name="cardData"></param>
        /// <param name="isAdd"></param>
        /// <returns></returns>
        Task<Tuple<List<CardProperty>, List<CardProperty>>> CardEditValidate(CardDto cardData, bool isAdd);
        /// <summary>
        /// 映射编码名称
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        Task<CardDto> MapPropertyName(CardDto userData);
        /// <summary>
        /// 映射编码名称
        /// </summary>
        /// <param name="cardData"></param>
        /// <returns></returns>
        Task<CardBatchEditInput> MapBatchPropertyName(CardBatchEditInput cardData);
        /// <summary>
        /// 获取卡初始数据
        /// </summary>
        /// <returns></returns>
        Task<object> GetCardInitData();
        /// <summary>
        /// 获取属性组可选项
        /// </summary>
        /// <returns></returns>
        Task<List<PropertyGroupSelectDto>> GetGroupSelectItem();
        /// <summary>
        /// 卡列表数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        Task<PagedList<CardListItemDto>> QueryTableData(CardTableQuery queryFilter);
        /// <summary>
        /// 获取卡详情
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        Task<CardDetailOutput> GetByID(Guid cardId);
        /// <summary>
        /// 获取卡详情
        /// </summary>
        /// <param name="no">卡号</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        Task<CardDetailOutput> GetByNoPwd(string no, string pwd);
        /// <summary>
        /// 获取读者所有卡
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<CardListItemDto>> QueryUserCardListData(Guid userId);
        /// <summary>
        /// 获取读者卡申请记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<CardListItemDto>> QueryUserCardApplyListData(Guid userId);
        /// <summary>
        /// 创建读者卡
        /// </summary>
        /// <param name="cardData"></param>
        /// <returns></returns>
        Task<Guid> Create(CardDto cardData);
        /// <summary>
        /// 编辑读者卡
        /// </summary>
        /// <param name="cardData"></param>
        /// <returns></returns>
        Task<Guid> Update(CardDto cardData);
        /// <summary>
        /// 设置读者卡密码
        /// </summary>
        /// <param name="cardSecret"></param>
        /// <returns></returns>
        Task<bool> SetSecret(CardSecretDto cardSecret);
        /// <summary>
        /// 重置置读者卡密码
        /// </summary>
        /// <param name="cardSecret"></param>
        /// <returns></returns>
        Task<bool> ResetSecret(CardSecretDto cardSecret);
        /// <summary>
        /// 删除读者卡
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        Task<bool> Delete(Guid cardId);
        /// <summary>
        /// 批量修改读者卡数据
        /// </summary>
        /// <param name="cardData"></param>
        /// <returns></returns>
        Task<bool> BatchUpdate(CardBatchEditInput cardData);
        /// <summary>
        /// 读者卡数据导出
        /// </summary>
        /// <param name="exportFilter"></param>
        /// <returns></returns>
        Task<PagedList<CardListItemDto>> ExportCardData(CardExportFilter exportFilter);
        /// <summary>
        /// 修改卡密码
        /// </summary>
        /// <param name="cardPwdChange"></param>
        /// <returns></returns>
        Task<bool> ChangeCardPwd(ReaderCardPwdChangeInput cardPwdChange);
        /// <summary>
        /// 设置读者主卡
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        Task<bool> SetPrincipalCard(Guid cardId, Guid UserId);

    }
}
