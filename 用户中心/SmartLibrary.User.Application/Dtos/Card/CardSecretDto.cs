/*********************************************************
* 名    称：CardSecretDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220301
* 描    述：读者卡密码修改
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.Dtos.Card
{
    /// <summary>
    /// 读者卡密码修改保存
    /// </summary>
    public class CardSecretDto
    {
        /// <summary>
        /// 卡Id
        /// </summary>
        public Guid CardId { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        public string Secret { get; set; }
    }
}
