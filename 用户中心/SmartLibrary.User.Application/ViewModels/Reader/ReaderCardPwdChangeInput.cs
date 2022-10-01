/*********************************************************
* 名    称：ReaderCardPwdChangeInput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：卡密码输入
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 卡密码修改输入
    /// </summary>
    public class ReaderCardPwdChangeInput
    {
        /// <summary>
        /// 卡Id
        /// </summary>
        public Guid CardID { get; set; }
        /// <summary>
        /// 旧密码
        /// </summary>
        public string PrePwd { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPwd { get; set; }
    }
}
