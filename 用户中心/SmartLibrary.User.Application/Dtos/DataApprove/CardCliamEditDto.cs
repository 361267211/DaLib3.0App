/*********************************************************
* 名    称：CardCliamEditDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220302
* 描    述：读者领卡编辑对象
* 更新历史：
*
* *******************************************************/
using System;
 
namespace SmartLibrary.User.Application.Dtos.DataApprove
{
    public class CardCliamEditDto
    {
        /// <summary>
        /// 卡ID
        /// </summary>
        public Guid CardID { get; set; }
        /// <summary>
        /// 申请人ID
        /// </summary>
        public Guid UserID { get; set; }
        /// <summary>
        /// 是否需要审批
        /// </summary>
        public bool NeedConfirm { get; set; }
    }
}
