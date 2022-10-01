/*********************************************************
* 名    称：PolicyKey.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220301
* 描    述：权限策略key
* 更新历史：
*
* *******************************************************/

namespace SmartLibrary.User.Application.Dtos.Common
{
    /// <summary>
    /// 权限验证策略
    /// </summary>
    public class PolicyKey
    {
        /// <summary>
        /// Token合法
        /// </summary>
        public const string TokenAuth = "TokenAuth";
        /// <summary>
        /// 馆员后台权限
        /// </summary>
        public const string StaffAuth = "StaffAuth";
        /// <summary>
        /// 读者前台权限
        /// </summary>
        public const string ReaderAuth = "ReaderAuth";
        /// <summary>
        /// 读者前台访问权限
        /// </summary>
        public const string PermitReaderAuth = "PermitReaderAuth";
        /// <summary>
        /// 是否登录标识
        /// </summary>
        public const string UnAuthKey = "UnAuth";
    }
}
