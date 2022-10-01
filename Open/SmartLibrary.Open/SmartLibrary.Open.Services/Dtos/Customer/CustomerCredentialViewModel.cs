/*********************************************************
* 名    称：CustomerCredentialViewModel.cs
* 作    者：刘启平
* 联系方式：电话[13627622058],邮件[83379242@qq.com]
* 创建时间：20210913
* 描    述：客户凭据信息
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.Open.Services.Dtos
{
    /// <summary>
    /// 客户凭据信息
    /// </summary>
    public class CustomerCredentialViewModel
    {
        /// <summary>
        /// 客户ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 识别码
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Secret { get; set; }
    }
}
