/*********************************************************
 * 名    称：TenantModelCacheKeyFactory
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：TenantModelCacheKey的工厂。
 *
 * 更新历史：
 *
 * *******************************************************/

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure
{
    internal sealed class TenantModelCacheKeyFactory<TContext> : ModelCacheKeyFactory
        where TContext : DbContext, ITenantDbContext
    {

        public override object Create(DbContext context)
        {
            var dbContext = context as TContext;
            //可以制造不同
           // dbContext.TenantInfo.Name += DateTime.Now.Second.ToString();
            var tenantModelCacheKey = new TenantModelCacheKey<TContext>(dbContext, dbContext?.TenantInfo?.Name ?? "no_tenant_identifier");

            return tenantModelCacheKey;
        }

        public TenantModelCacheKeyFactory(ModelCacheKeyFactoryDependencies dependencies) : base(dependencies)
        {
        }
    }

    internal sealed class TenantModelCacheKey<TContext> : ModelCacheKey
        where TContext : DbContext, ITenantDbContext
    {
        private readonly TContext context;
        private readonly string identifier;
        public TenantModelCacheKey(TContext context, string identifier) : base(context)
        {
            this.context = context;
            this.identifier = identifier;
        }

        protected override bool Equals(ModelCacheKey other)
        {
            return base.Equals(other) && (other as TenantModelCacheKey<TContext>)?.identifier == identifier;
        }

        public override int GetHashCode()
        {
            var hashCode = base.GetHashCode();
            if (identifier != null)
            {
                hashCode ^= identifier.GetHashCode();
            }

            return hashCode;
        }
    }
}
