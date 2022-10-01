/*********************************************************
* 名    称：SecurityService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：安全策略管理服务
* 更新历史：
*
* *******************************************************/
using Furion.DependencyInjection;
using SmartLibrary.User.Application.Dtos.Security;
using SmartLibrary.User.Application.Services.Interface;
using SmartLibrary.User.Common.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.Services.Impl
{
    /// <summary>
    /// 安全策略管理服务
    /// </summary>
    public class SecurityService : ISecurityService, IScoped
    {
        /// <summary>
        /// 获取安全策略
        /// </summary>
        /// <returns></returns>
        public async Task<SecurityPolicyDto> GetSecurityPolicy()
        {
            var securityPolicies = new List<SecurityPolicyDto>()
            {
                new SecurityPolicyDto
                {
                   SecretLevel=0,
                    SecretExpireDay=365*10,
                    SecretRetryTime=5*3,
                    SecretKeepDay=30*12,
                    UseLoginValidateCode=false,
                    FirstLoginChangePwd=false,
                },
                new SecurityPolicyDto
                {
                   SecretLevel=1,
                    SecretExpireDay=365*3,
                    SecretRetryTime=5*2,
                    SecretKeepDay=30*3,
                    UseLoginValidateCode=false,
                    FirstLoginChangePwd=true,
                },
                new SecurityPolicyDto
                {
                    SecretLevel=2,
                    SecretExpireDay=365,
                    SecretRetryTime=5,
                    SecretKeepDay=30,
                    UseLoginValidateCode=true,
                    FirstLoginChangePwd=true,
                },
            };

            var index = SiteGlobalConfig.SecurityLevel % 3;
            return await Task.FromResult(securityPolicies[index]);
        }
    }
}
