/*********************************************************
* 名    称：AppCustomerTableViewModel.cs
* 作    者：刘启平
* 联系方式：电话[13627622058],邮件[83379242@qq.com]
* 创建时间：20210908
* 描    述：应用试用客户视图模型
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.Open.Services.Dtos
{
    /// <summary>
    /// 应用试用客户视图模型
    /// </summary>
    public class AppCustomerTableViewModel
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public Guid AppID { get; set; }
        /// <summary>
        /// 客户ID
        /// </summary>
        public Guid CustomerID { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public int SortNo { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 客户类型，1、正式授权 2、试用授权
        /// </summary>
        public int CustomerType { get; set; }

        /// <summary>
        /// 启用时间
        /// </summary>
        public DateTime BeginDate { get; set; }
        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime ExpireDate { get; set; }

    }
}
