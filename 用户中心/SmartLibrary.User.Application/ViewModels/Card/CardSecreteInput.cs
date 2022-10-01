/*********************************************************
* 名    称：CardSecreteInput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：读者卡密码设置
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 卡密码设置
    /// </summary>
    public class CardSecreteInput
    {
        /// <summary>
        /// 卡ID
        /// </summary>
        public Guid CardId { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        public string Secret { get; set; }
    }
}
