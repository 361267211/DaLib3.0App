/*********************************************************
 * 名    称：AssetDbContext
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：数据库上下文。
 *
 * 更新历史：
 *
 * *******************************************************/


using Furion.DatabaseAccessor;
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.Extensions.Logging;
using SmartLibrary.Open.EntityFramework.Core.Entitys;
using SmartLibrary.Open.EntityFramework.Core.Entitys.Assembly;
using System;

namespace SmartLibrary.Open.EntityFramework.Core.DbContexts
{
    /// <summary>
    /// 示例数据库上下文
    /// </summary>
    public class OpenDbContext : AppDbContext<OpenDbContext>, ITenantDbContext
    {
        public TenantInfo TenantInfo => tenantInfo;
        private readonly TenantInfo tenantInfo;
        public static readonly LoggerFactory _LoggerFactory = new LoggerFactory(new[] { new DebugLoggerProvider() });

        public OpenDbContext(DbContextOptions<OpenDbContext> options, TenantInfo tenantInfo) : base(options)
        {
            this.tenantInfo = tenantInfo;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseBatchEF_Npgsql();
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLoggerFactory(_LoggerFactory);
        }


        /// <summary>
        /// 创建模型前
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //var schemaName = GetFullSchemaName();
            modelBuilder.Entity<AppBranch>().ToTable(nameof(AppBranch));
            modelBuilder.Entity<AppBranchEntryPoint>().ToTable(nameof(AppBranchEntryPoint));
            modelBuilder.Entity<ActivityInfo>().ToTable(nameof(ActivityInfo));
            modelBuilder.Entity<AppDictioanry>().ToTable(nameof(AppDictioanry));
            modelBuilder.Entity<AppDynamic>().ToTable(nameof(AppDynamic));
            modelBuilder.Entity<MicroApplication>().ToTable(nameof(MicroApplication));
            modelBuilder.Entity<AppAvailibleSortField>().ToTable(nameof(AppAvailibleSortField));
            modelBuilder.Entity<AppEvent>().ToTable(nameof(AppEvent));
            modelBuilder.Entity<AppServicePack>().ToTable(nameof(AppServicePack));
            modelBuilder.Entity<AppServiceType>().ToTable(nameof(AppServiceType));
            modelBuilder.Entity<AppSpecificCustomer>().ToTable(nameof(AppSpecificCustomer));
            modelBuilder.Entity<AppWidget>().ToTable(nameof(AppWidget));
            modelBuilder.Entity<Customer>().ToTable(nameof(Customer));
            modelBuilder.Entity<CustomerAppUsage>().ToTable(nameof(CustomerAppUsage));
            modelBuilder.Entity<Deployment>().ToTable(nameof(Deployment));
            modelBuilder.Entity<Developer>().ToTable(nameof(Developer));
            modelBuilder.Entity<Information>().ToTable(nameof(Information));
            modelBuilder.Entity<InfoSpecificCustomer>().ToTable(nameof(InfoSpecificCustomer));
            modelBuilder.Entity<Order>().ToTable(nameof(Order));
            modelBuilder.Entity<OrderNoSeed>().ToTable(nameof(OrderNoSeed));
            modelBuilder.Entity<SearchBoxTitleItem>().ToTable(nameof(SearchBoxTitleItem));
            modelBuilder.Entity<OpacTemplate>().ToTable(nameof(OpacTemplate));

            //汇编专题应用
            modelBuilder.Entity<AssemblyBaseInfo>().ToTable(nameof(AssemblyBaseInfo));
            modelBuilder.Entity<AssemblyArticleColumn>().ToTable(nameof(AssemblyArticleColumn));
            modelBuilder.Entity<ArtByImported>().ToTable(nameof(ArtByImported));
            modelBuilder.Entity<ArtByUpload>().ToTable(nameof(ArtByUpload));
            modelBuilder.Entity<ArtColSearchThemes>().ToTable(nameof(ArtColSearchThemes));
            modelBuilder.Entity<ArtRetrievalExp>().ToTable(nameof(ArtRetrievalExp));


            modelBuilder.Entity<BookDonationTemplate>().ToTable(nameof(BookDonationTemplate));



            //if (string.IsNullOrWhiteSpace(schemaName))
            //{
            //    modelBuilder.HasDefaultSchema("");
            //}
        }

        /// <summary>
        /// 创建model时获取其schema
        /// </summary>
        /// <returns></returns>
        private string GetFullSchemaName()
        {
            return "";
        }

        public string GetSchemaName()
        {
            return this.tenantInfo.Name;
        }
    }
}
