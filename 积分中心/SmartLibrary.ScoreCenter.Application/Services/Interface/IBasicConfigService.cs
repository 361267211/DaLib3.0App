/*********************************************************
* 名    称：IBasicConfigService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：基础配置服务
* 更新历史：
*
* *******************************************************/
using SmartLibrary.ScoreCenter.Application.ViewModels;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.Application.Services.Interface
{
    /// <summary>
    /// 基础配置服务
    /// </summary>
    public interface IBasicConfigService
    {
        /// <summary>
        /// 获取基础配置
        /// </summary>
        /// <returns></returns>
        Task<BasicConfigEditData> GetBasicConfigSet();
        /// <summary>
        /// 保存配置信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> SaveConfigSet(BasicConfigEditData input);

    }
}
