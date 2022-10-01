/*********************************************************
 * 名    称：HeaderService
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/21 22:06:42
 * 描    述：
 *
 * 更新历史：
 *
 * *******************************************************/
using SmartLibrary.SceneManage.Common.Dtos;
using SmartLibrary.SceneManage.Service.Dtos.SceneManage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.SceneManage.Service.Service
{
    public interface IHeaderFooterService
    {
        Task<FootTemplateSettingDto> GetFooterData(string templateCode);
        Task<HeaderViewModel> GetHeaderData(string templateCode);
        /// <summary>
        /// 获取头部模板高级设置项
        /// </summary>
        /// <param name="headTemplateId"></param>
        /// <returns></returns>
        Task<HeadTemplateSettingDto> GetHeadTemplateSettingsById(string headTemplateId);

        /// <summary>
        /// 获取底部模板高级设置项
        /// </summary>
        /// <param name="footTemplateId"></param>
        /// <returns></returns>
        Task<FootTemplateSettingDto> GetFootTemplateSettingsById(string footTemplateId);

        /// <summary>
        /// 更新头部模板高级设置项
        /// </summary>
        /// <param name="head"></param>
        /// <returns></returns>
        Task UpdateHeadTemplateSettings(HeadTemplateSettingDto head);

        /// <summary>
        /// 新增头部模板高级设置项
        /// </summary>
        /// <param name="head"></param>
        /// <returns></returns>
        Task AddHeadTemplateSettings(HeadTemplateSettingDto head);
        /// <summary>
        /// 更新底部模板高级设置项
        /// </summary>
        /// <param name="foot"></param>
        /// <returns></returns>
        Task UpdateFootTemplateSettings(FootTemplateSettingDto foot);

        /// <summary>
        /// 新增底部模板高级设置项
        /// </summary>
        /// <param name="foot"></param>
        /// <returns></returns>
        Task AddFootTemplateSettings(FootTemplateSettingDto foot);

        /// <summary>
        /// 新增底部模板高级设置项
        /// </summary>
        /// <param name="foot"></param>
        /// <returns></returns>
        Task<List<SysDictModel<string>>> GetNavColumnList();
    }
}