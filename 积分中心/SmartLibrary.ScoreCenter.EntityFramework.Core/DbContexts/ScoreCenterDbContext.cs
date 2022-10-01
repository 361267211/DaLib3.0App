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
using SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.EntityFramework.Core.DbContexts
{
    /// <summary>
    /// 示例数据库上下文
    /// </summary>
    public class ScoreCenterDbContext : AppDbContext<ScoreCenterDbContext>, ITenantDbContext
    {
        public TenantInfo TenantInfo => tenantInfo;
        private readonly TenantInfo tenantInfo;



        public ScoreCenterDbContext(DbContextOptions<ScoreCenterDbContext> options, TenantInfo tenantInfo) : base(options)
        {
            this.tenantInfo = tenantInfo;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseBatchEF_Npgsql();
            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        /// 创建模型前
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var schemaName = GetFullSchemaName();
            //系统菜单管理
            var sysMenuPermissionBuilder = modelBuilder.Entity<SysMenuPermission>().ToTable(nameof(SysMenuPermission), schemaName);
            sysMenuPermissionBuilder.HasKey(x => x.Id).IsClustered(false);
            sysMenuPermissionBuilder.HasIndex(x => x.CreateTime).IsClustered(true);

            //基础配置
            var basicConfigBuilder = modelBuilder.Entity<BasicConfig>().ToTable(nameof(BasicConfig), schemaName);
            basicConfigBuilder.HasKey(x => x.Id).IsClustered(false);
            basicConfigBuilder.HasIndex(x => x.CreateTime).IsClustered(true);

            //商品管理
            var goodsRecordBuilder = modelBuilder.Entity<GoodsRecord>().ToTable(nameof(GoodsRecord), schemaName);
            goodsRecordBuilder.HasKey(x => x.Id).IsClustered(false);
            goodsRecordBuilder.HasIndex(x => x.CreateTime).IsClustered(true);

            //勋章获取任务
            var medalObtainTaskBuilder = modelBuilder.Entity<MedalObtainTask>().ToTable(nameof(MedalObtainTask), schemaName);
            medalObtainTaskBuilder.HasKey(x => x.Id).IsClustered(false);
            medalObtainTaskBuilder.HasIndex(x => x.CreateTime).IsClustered(true);

            //操作记录表
            var operationBuilder = modelBuilder.Entity<OperationRecord>().ToTable(nameof(OperationRecord), schemaName);
            operationBuilder.HasKey(x => x.Id).IsClustered(true);
            operationBuilder.HasIndex(x => x.OperateKey).IsUnique(true);

            //订单记录表
            var orderRecordBuilder = modelBuilder.Entity<OrderRecord>().ToTable(nameof(OrderRecord), schemaName);
            orderRecordBuilder.HasKey(x => x.Id).IsClustered(false);
            orderRecordBuilder.HasIndex(x => x.CreateTime).IsClustered(true);

            //用户积分清算记录
            var overdueScoreClearBuilder = modelBuilder.Entity<OverdueScoreClear>().ToTable(nameof(OverdueScoreClear), schemaName);
            overdueScoreClearBuilder.HasKey(x => x.Id).IsClustered(false);
            overdueScoreClearBuilder.HasIndex(x => x.CreateTime).IsClustered(true);
            overdueScoreClearBuilder.Property(x => x.Id).UseIdentityColumn();

            //积分消费任务配置
            var scoreConsumeTaskBuilder = modelBuilder.Entity<ScoreConsumeTask>().ToTable(nameof(ScoreConsumeTask), schemaName);
            scoreConsumeTaskBuilder.HasKey(x => x.Id).IsClustered(false);
            scoreConsumeTaskBuilder.HasIndex(x => x.CreateTime).IsClustered(true);

            //积分等级
            var scoreLevelBuilder = modelBuilder.Entity<ScoreLevel>().ToTable(nameof(ScoreLevel), schemaName);
            scoreLevelBuilder.HasKey(x => x.Id).IsClustered(false);
            scoreLevelBuilder.HasIndex(x => x.CreateTime).IsClustered(true);

            //积分手动处理
            var scoreManualProcessBuilder = modelBuilder.Entity<ScoreManualProcess>().ToTable(nameof(ScoreManualProcess), schemaName);
            scoreManualProcessBuilder.HasKey(x => x.Id).IsClustered(false);
            scoreManualProcessBuilder.HasIndex(x => x.CreateTime).IsClustered(true);

            //积分获取任务
            var scoreObtainTaskBuilder = modelBuilder.Entity<ScoreObtainTask>().ToTable(nameof(ScoreObtainTask), schemaName);
            scoreObtainTaskBuilder.HasKey(x => x.Id).IsClustered(false);
            scoreObtainTaskBuilder.HasIndex(x => x.CreateTime).IsClustered(true);

            //积分调整影响人数
            var scoreRecieveUserBuilder = modelBuilder.Entity<ScoreRecieveUser>().ToTable(nameof(ScoreRecieveUser), schemaName);
            scoreRecieveUserBuilder.HasKey(x => x.Id).IsClustered(false);
            scoreRecieveUserBuilder.HasIndex(x => x.CreateTime).IsClustered(true);

            //积分调整人员查询规则
            var scoreRecieveRuleBuilder = modelBuilder.Entity<ScoreRecieveRule>().ToTable(nameof(ScoreRecieveRule), schemaName);
            scoreRecieveRuleBuilder.HasKey(x => x.Id).IsClustered(false);
            scoreRecieveRuleBuilder.HasIndex(x => x.CreateTime).IsClustered(true);

            //用户积分
            var userScoreBuilder = modelBuilder.Entity<UserScore>().ToTable(nameof(UserScore), schemaName);
            userScoreBuilder.HasKey(x => x.Id).IsClustered(false);
            userScoreBuilder.HasIndex(x => x.CreateTime).IsClustered(true);

            //用户积分明细
            var userEventScoreBuilder = modelBuilder.Entity<UserEventScore>().ToTable(nameof(UserEventScore), schemaName);
            userEventScoreBuilder.HasKey(x => x.Id).IsClustered(false);
            userEventScoreBuilder.HasIndex(x => x.CreateTime).IsClustered(true);

            //用户勋章
            var userMedalBuilder = modelBuilder.Entity<UserMedal>().ToTable(nameof(UserMedal), schemaName);
            userMedalBuilder.HasKey(x => x.Id).IsClustered(false);
            userMedalBuilder.HasIndex(x => x.CreateTime).IsClustered(true);

            //用户勋章获取明细
            var userMedalEventBuilder = modelBuilder.Entity<UserMedalEvent>().ToTable(nameof(UserMedalEvent), schemaName);
            userMedalEventBuilder.HasKey(x => x.Id).IsClustered(false);
            userMedalEventBuilder.HasIndex(x => x.CreateTime).IsClustered(true);

            //用户喜爱商品
            var goodsPreferBuilder = modelBuilder.Entity<UserGoodsPrefer>().ToTable(nameof(UserGoodsPrefer), schemaName);
            goodsPreferBuilder.HasKey(x => x.Id).IsClustered(false);
            goodsPreferBuilder.HasIndex(x => x.CreateTime).IsClustered(true);

            if (string.IsNullOrWhiteSpace(schemaName))
            {
                modelBuilder.HasDefaultSchema("dbo");
            }
        }

        /// <summary>
        /// 创建model时获取其schema
        /// </summary>
        /// <returns></returns>
        private string GetFullSchemaName()
        {
            return this.tenantInfo != null && !string.IsNullOrWhiteSpace(this.tenantInfo.Name) ? $"{this.tenantInfo.Name}" : "dbo";
        }
        public string GetSchemaName()
        {
            return this.tenantInfo.Name;
        }
    }
}
