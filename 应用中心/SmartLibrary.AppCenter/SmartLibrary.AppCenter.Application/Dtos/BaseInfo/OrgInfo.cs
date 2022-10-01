/*********************************************************
 * 名    称：OrgInfo
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/12/2 21:13:47
 * 描    述：机构信息
 *
 * 更新历史：
 *
 * *******************************************************/

namespace SmartLibrary.AppCenter.Application.Dtos.BaseInfo
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class OrgInfo
    {
        /// <summary>
        /// 机构标识
        /// </summary>
        public string OrgCode { get; set; }
        /// <summary>
        /// 门户地址
        /// </summary>
        public string PortalUrl { get; set; }
        /// <summary>
        /// 后台地址
        /// </summary>
        public string ManageUrl { get; set; }
        /// <summary>
        /// 文件地址
        /// </summary>
        public string FileUrl { get; set; }
        /// <summary>
        /// 前台登录地址
        /// </summary>
        public string LoginUrl { get; set; }
        /// <summary>
        /// 后台登录地址
        /// </summary>
        public string MgrLoginUrl { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>
        public string OrgName { get; set; }

        /// <summary>
        /// Logo地址
        /// </summary>
        public string LogoUrl { get; set; }

        /// <summary>
        /// 简版Logo地址
        /// </summary>
        public string SimpleLogoUrl { get; set; }
    }
}
