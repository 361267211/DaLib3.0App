/*********************************************************
* 名    称：BasicConfigSetInput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：基础配置设置
* 更新历史：
*
* *******************************************************/
namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 基础配置设置
    /// </summary>
    public class BasicConfigSetInput
    {
        /// <summary>
        /// 是否开启敏感信息过滤
        /// </summary>
        public bool SensitiveFilter { get; set; }
        /// <summary>
        /// 读者审批
        /// </summary>
        public bool UserInfoConfirm { get; set; }
        /// <summary>
        /// 属性审批
        /// </summary>
        public bool PropertyConfirm { get; set; }
        /// <summary>
        /// 读者卡认领
        /// </summary>
        public bool CardClaim { get; set; }
        /// <summary>
        /// 完善个人信息
        /// </summary>
        public bool UserInfoSupply { get; set; }
    }
}
