/*********************************************************
* 名    称：IBasicConfigService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：基础配置服务接口
* 更新历史：
*
* *******************************************************/
using Furion.DependencyInjection;
using SmartLibrary.User.Application.Dtos.BasicConfigSet;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.Services.Interface
{
    /// <summary>
    /// 基础配置服务接口
    /// </summary>
    public interface IBasicConfigService : IScoped
    {
        /// <summary>
        /// 获取基础配置初始数据
        /// </summary>
        /// <returns></returns>
        public Task<object> GetBasicConfigInitData();
        /// <summary>
        /// 获取基础配置信息
        /// </summary>
        /// <returns></returns>
        public Task<BasicConfigSetDto> GetBasicConfigSet();
        /// <summary>
        /// 设置配置信息
        /// </summary>
        /// <param name="configData"></param>
        /// <returns></returns>
        public Task<bool> SetBasicConfig(BasicConfigSetDto configData);
        /// <summary>
        /// 获取读者编辑属性配置
        /// </summary>
        /// <returns></returns>
        public Task<List<ReaderEditPropertyDto>> QueryReaderEditProperty();
        /// <summary>
        /// 设置读者可修改属性
        /// </summary>
        /// <param name="editProperties"></param>
        /// <returns></returns>
        public Task<bool> SetReaderEditProperty(List<ReaderEditPropertyDto> editProperties);
        /// <summary>
        /// 获取读者领卡用户
        /// </summary>
        /// <returns></returns>
        public Task<List<InfoPermitReaderDto>> GetCardClaimReader();
        /// <summary>
        /// 设置读者领卡用户
        /// </summary>
        /// <param name="cardClaimReaders"></param>
        /// <returns></returns>
        public Task<bool> SetCardClaimReader(List<InfoPermitReaderDto> cardClaimReaders);
        /// <summary>
        /// 获取读者用户信息完善
        /// </summary>
        /// <returns></returns>
        public Task<List<InfoPermitReaderDto>> GetInfoAppendReader();
        /// <summary>
        /// 设置读者用户信息完善
        /// </summary>
        /// <param name="infoAppendReaders"></param>
        /// <returns></returns>
        public Task<bool> SetInfoAppendReader(List<InfoPermitReaderDto> infoAppendReaders);


    }
}
