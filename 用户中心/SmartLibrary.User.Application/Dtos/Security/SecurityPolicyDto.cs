/*********************************************************
* 名    称：SecurityPolicyDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220302
* 描    述：安全设置
* 更新历史：
*
* *******************************************************/

namespace SmartLibrary.User.Application.Dtos.Security
{
    /// <summary>
    /// 安全设置
    /// </summary>
    public class SecurityPolicyDto
    {
        /// <summary>
        /// 密码等级
        /// </summary>
        public int SecretLevel { get; set; }
        /// <summary>
        /// 密码过期天数
        /// </summary>
        public int SecretExpireDay { get; set; }
        /// <summary>
        /// 密码锁定次数
        /// </summary>
        public int SecretRetryTime { get; set; }
        /// <summary>
        /// 密码保持天数
        /// </summary>
        public int SecretKeepDay { get; set; }
        /// <summary>
        /// 是否开启验证码
        /// </summary>
        public bool UseLoginValidateCode { get; set; }
        /// <summary>
        ///首次登陆修改密码
        /// </summary>
        public bool FirstLoginChangePwd { get; set; }
    }
}
