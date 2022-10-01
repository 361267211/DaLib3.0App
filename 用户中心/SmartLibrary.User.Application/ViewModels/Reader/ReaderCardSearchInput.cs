/*********************************************************
* 名    称：ReaderCardSearchInput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：账号密码查询
* 更新历史：
*
* *******************************************************/

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 账号密码查询
    /// </summary>
    public class ReaderCardSearchInput
    {
        /// <summary>
        /// 卡号
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd { get; set; }
    }
}
