/*********************************************************
* 名    称：ISecurityService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：安全配置服务
* 更新历史：
*
* *******************************************************/
using SmartLibrary.User.Application.Dtos.Security;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.Services.Interface
{
    /// <summary>
    /// 安全配置服务
    /// </summary>
    public interface ISecurityService
    {
        /// <summary>
        /// 获取安全策略
        /// </summary>
        /// <returns></returns>
        public Task<SecurityPolicyDto> GetSecurityPolicy();
    }
}
