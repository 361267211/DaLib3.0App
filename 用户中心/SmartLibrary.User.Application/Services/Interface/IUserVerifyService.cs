/*********************************************************
* 名    称：IUserVerifyService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户认证服务
* 更新历史：
*
* *******************************************************/
using SmartLibrary.User.Application.Dtos.User;
using System;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.Services.Interface
{
    /// <summary>
    /// 用户认证服务
    /// </summary>
    public interface IUserVerifyService
    {
        /// <summary>
        /// 手机号发送验证码
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        Task<string> SendMobileVerifyCode(string mobile);
        /// <summary>
        /// 检查手机号是否存在并发送验证码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        Task<string> CheckAndSendMobileVerifyCode(Guid userId, string mobile);

        /// <summary>
        /// 绑定用户手机
        /// </summary>
        /// <param name="bindData"></param>
        /// <returns></returns>
        Task<bool> BindMobile(BindMobileDto bindData);

        /// <summary>
        /// 邮箱发送验证码
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<string> SendEmailVerifyCode(string email);

        /// <summary>
        /// 检查邮箱是否存在并发送验证码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<string> CheckAndSendEmailVerifyCode(Guid userId, string email);
    
        /// <summary>
        ///  绑定用户邮箱
        /// </summary>
        /// <param name="bindData"></param>
        /// <returns></returns>
        Task<bool> BindEmail(BindEmailDto bindData);

        /// <summary>
        /// 修改用户身份证号
        /// </summary>
        /// <param name="bindIdCard"></param>
        /// <returns></returns>
        Task<bool> SetUserIdCard(BindIdCardDto bindIdCard);
    }
}
