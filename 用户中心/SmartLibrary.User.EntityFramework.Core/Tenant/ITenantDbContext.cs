/*********************************************************
 * 名    称：AssetDbContext
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：租户信息上下文。
 *
 * 更新历史：
 *
 * *******************************************************/

namespace kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure
{
    public interface ITenantDbContext
    {
        TenantInfo TenantInfo { get; }
    }
}
