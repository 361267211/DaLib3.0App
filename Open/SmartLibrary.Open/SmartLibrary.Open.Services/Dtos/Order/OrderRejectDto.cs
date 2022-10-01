/*********************************************************
* 名    称：OrderRejectDto.cs
* 作    者：刘启平
* 联系方式：电话[13627622058],邮件[83379242@qq.com]
* 创建时间：20210909
* 描    述：订单拒绝参数
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.Open.Services.Dtos
{
    /// <summary>
    /// 订单拒绝参数
    /// </summary>
    public class OrderRejectDto
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public Guid OrderID { get; set; }
        /// <summary>
        /// 拒绝原因
        /// </summary>
        public string Reason { get; set; }
    }
}
