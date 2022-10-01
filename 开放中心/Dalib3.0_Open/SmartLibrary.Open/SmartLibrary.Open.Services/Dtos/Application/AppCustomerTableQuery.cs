/*********************************************************
* 名    称：AppCustomerTableQuery.cs
* 作    者：刘启平
* 联系方式：电话[13627622058],邮件[83379242@qq.com]
* 创建时间：20210908
* 描    述：应用客户授权信息查询条件
* 更新历史：
*
* *******************************************************/
using SmartLibrary.Open.Common.AssemblyBase;
using System;

namespace SmartLibrary.Open.Services.Dtos
{
    /// <summary>
    /// 应用客户授权信息查询条件
    /// </summary>
    public class AppCustomerTableQuery:TableQueryBase
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public Guid? AppID { get; set; }
        /// <summary>
        /// 用户类型,0：试用，1：正式
        /// </summary>
        public int? CustomerType { get; set; }
        /// <summary>
        /// 只查询即将到期客户
        /// </summary>
        public bool DueSoon { get; set; }
    }
}
