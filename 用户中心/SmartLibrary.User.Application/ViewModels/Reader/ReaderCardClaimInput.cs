/*********************************************************
* 名    称：ReaderCardClaimInput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：读者卡领取
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 读者卡领取
    /// </summary>
    public class ReaderCardClaimInput
    {
        /// <summary>
        /// 卡Id
        /// </summary>
        public Guid CardID { get; set; }
    }
}
