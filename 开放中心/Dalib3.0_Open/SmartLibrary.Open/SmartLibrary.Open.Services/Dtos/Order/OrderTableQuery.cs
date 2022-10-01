/*********************************************************
* 名    称：OrderTableQuery.cs
* 作    者：刘启平
* 联系方式：电话[13627622058],邮件[83379242@qq.com]
* 创建时间：20210909
* 描    述：订单Table查询条件
* 更新历史：
*
* *******************************************************/
using System;
using SmartLibrary.Open.Common.AssemblyBase;

namespace SmartLibrary.Open.Services.Dtos
{
    /// <summary>
    /// 订单Table查询条件
    /// </summary>
    public class OrderTableQuery : TableQueryBase
    {
        /// <summary>
        /// 客户ID
        /// </summary>
        public Guid? CustomerID { get; set; }
        /// <summary>
        /// 客户Owner
        /// </summary>
        public string CustomerOwner { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 授权类型
        /// </summary>
        public int? AuthType { get; set; }
        /// <summary>
        /// 开通类型
        /// </summary>
        public int? OpenType { get; set; }
        /// <summary>
        /// 状态选择
        /// </summary>
        public int? Status { get; set; }

    }
}
