/*********************************************************
 * 名    称：租户信息
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：租户信息的模型。
 *
 * 更新历史：
 *
 * *******************************************************/

using System;

namespace kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure
{
    /// <summary>
    /// 租户信息的模型类，后期可继续拓展
    /// </summary>
    public class TenantInfo
    {
        /// <summary>
        /// 多租户姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 租户UserKey
        /// </summary>
        public string UserKey { get; set; }
    }
}
