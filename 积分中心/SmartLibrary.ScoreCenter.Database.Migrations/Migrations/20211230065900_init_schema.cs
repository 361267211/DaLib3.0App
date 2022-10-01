using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.ScoreCenter.Database.Migrations.Migrations
{
    public partial class init_schema : Migration
    {
        private readonly string schema;
        public init_schema(string schema)
        {
            this.schema = schema;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: $"{schema}");

            migrationBuilder.CreateTable(
                name: "BasicConfig",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    ShowLevel = table.Column<bool>(type: "boolean", nullable: false),
                    ShowRule = table.Column<bool>(type: "boolean", nullable: false),
                    RuleContent = table.Column<string>(type: "character varying(8000)", maxLength: 8000, nullable: true),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasicConfig", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "GoodsRecord",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    TotalCount = table.Column<int>(type: "integer", nullable: false),
                    CurrentCount = table.Column<int>(type: "integer", nullable: false),
                    FreezeCount = table.Column<int>(type: "integer", nullable: false),
                    SaleOutCount = table.Column<int>(type: "integer", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false),
                    IntroPicUrl = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ObtainWay = table.Column<int>(type: "integer", nullable: false),
                    ObtainAddress = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ObtainContact = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    BeginDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DetailInfo = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ObtainTime = table.Column<string>(type: "text", nullable: true),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsRecord", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MedalObtainTask",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Desc = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    IntroPicUrl = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    AppCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    EventCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TriggerWay = table.Column<int>(type: "integer", nullable: false),
                    TotalTime = table.Column<int>(type: "integer", nullable: false),
                    MustContinue = table.Column<bool>(type: "boolean", nullable: false),
                    BeginDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    PcLink = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    AppLink = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    MicroAppLink = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    H5Link = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ForPc = table.Column<bool>(type: "boolean", nullable: false),
                    ForApp = table.Column<bool>(type: "boolean", nullable: false),
                    ForMicroApp = table.Column<bool>(type: "boolean", nullable: false),
                    ForH5 = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedalObtainTask", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "OperationRecord",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    OperateKey = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationRecord", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "OrderRecord",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    GoodsID = table.Column<Guid>(type: "uuid", nullable: false),
                    ExchangeCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ExchangeCount = table.Column<int>(type: "integer", nullable: false),
                    ExchangeUserKey = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ExchangeName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Express = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ExpressNo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ObtainWay = table.Column<int>(type: "integer", nullable: false),
                    ExchangeTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ObtainTime = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    RecieveAddrss = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderRecord", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "OverdueScoreClear",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    UserKey = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ExpireTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    OverScore = table.Column<int>(type: "integer", nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OverdueScoreClear", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ScoreConsumeTask",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Desc = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    AppCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    EventCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ConsumeScore = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreConsumeTask", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ScoreLevel",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ArchiveScore = table.Column<int>(type: "integer", nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreLevel", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ScoreManualProcess",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Desc = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false),
                    ValidTerm = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    SourceFrom = table.Column<int>(type: "integer", nullable: false),
                    OperatorName = table.Column<string>(type: "text", nullable: true),
                    OperatorUserKey = table.Column<string>(type: "text", nullable: true),
                    OperatorTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreManualProcess", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ScoreObtainTask",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Desc = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    AppCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    EventCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FullEventCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ObtainScore = table.Column<int>(type: "integer", nullable: false),
                    IntroPicUrl = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    PcLink = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    AppLink = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    MicroAppLink = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    H5Link = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ForPc = table.Column<bool>(type: "boolean", nullable: false),
                    ForApp = table.Column<bool>(type: "boolean", nullable: false),
                    ForMicroApp = table.Column<bool>(type: "boolean", nullable: false),
                    ForH5 = table.Column<bool>(type: "boolean", nullable: false),
                    ValidTerm = table.Column<int>(type: "integer", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    TriggerTerm = table.Column<int>(type: "integer", nullable: false),
                    TriggerTime = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreObtainTask", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ScoreRecieveRule",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    ProcessID = table.Column<Guid>(type: "uuid", nullable: false),
                    RuleType = table.Column<int>(type: "integer", nullable: false),
                    PropertyCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OperateType = table.Column<int>(type: "integer", nullable: false),
                    PropertyValue = table.Column<string>(type: "text", nullable: true),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreRecieveRule", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ScoreRecieveUser",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    ProcessID = table.Column<Guid>(type: "uuid", nullable: false),
                    UserKey = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SourceFrom = table.Column<int>(type: "integer", nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreRecieveUser", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SysMenuPermission",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Pid = table.Column<string>(type: "text", nullable: true, comment: "父级id"),
                    Path = table.Column<string>(type: "text", nullable: true),
                    FullPath = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Icon = table.Column<string>(type: "text", nullable: true),
                    Router = table.Column<string>(type: "text", nullable: true),
                    Component = table.Column<string>(type: "text", nullable: true),
                    Permission = table.Column<string>(type: "text", nullable: true),
                    Category = table.Column<string>(type: "text", nullable: true),
                    OpenWay = table.Column<int>(type: "integer", nullable: false),
                    Visible = table.Column<bool>(type: "boolean", nullable: false),
                    Sort = table.Column<int>(type: "integer", nullable: false),
                    Remark = table.Column<string>(type: "text", nullable: true),
                    IsSysMenu = table.Column<bool>(type: "boolean", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysMenuPermission", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserEventScore",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    AppCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    EventName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    EventCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FullEventCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    EventScore = table.Column<int>(type: "integer", nullable: false),
                    UserKey = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Remark = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    TriggerTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEventScore", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserMedal",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    MedalObtainTaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserKey = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    TriggerTime = table.Column<int>(type: "integer", nullable: false),
                    TotalTime = table.Column<int>(type: "integer", nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMedal", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserMedalEvent",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    MedalObtainTaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserKey = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TriggerTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    TriggerReset = table.Column<bool>(type: "boolean", nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMedalEvent", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserScore",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    UserKey = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AvailableScore = table.Column<int>(type: "integer", nullable: false),
                    FreezeScore = table.Column<int>(type: "integer", nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserScore", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BasicConfig_CreateTime",
                schema: $"{schema}",
                table: "BasicConfig",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsRecord_CreateTime",
                schema: $"{schema}",
                table: "GoodsRecord",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_MedalObtainTask_CreateTime",
                schema: $"{schema}",
                table: "MedalObtainTask",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_OperationRecord_OperateKey",
                schema: $"{schema}",
                table: "OperationRecord",
                column: "OperateKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderRecord_CreateTime",
                schema: $"{schema}",
                table: "OrderRecord",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_OverdueScoreClear_CreateTime",
                schema: $"{schema}",
                table: "OverdueScoreClear",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreConsumeTask_CreateTime",
                schema: $"{schema}",
                table: "ScoreConsumeTask",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreLevel_CreateTime",
                schema: $"{schema}",
                table: "ScoreLevel",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreManualProcess_CreateTime",
                schema: $"{schema}",
                table: "ScoreManualProcess",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreObtainTask_CreateTime",
                schema: $"{schema}",
                table: "ScoreObtainTask",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreRecieveRule_CreateTime",
                schema: $"{schema}",
                table: "ScoreRecieveRule",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreRecieveUser_CreateTime",
                schema: $"{schema}",
                table: "ScoreRecieveUser",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_SysMenuPermission_CreateTime",
                schema: $"{schema}",
                table: "SysMenuPermission",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_UserEventScore_CreateTime",
                schema: $"{schema}",
                table: "UserEventScore",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_UserMedal_CreateTime",
                schema: $"{schema}",
                table: "UserMedal",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_UserMedalEvent_CreateTime",
                schema: $"{schema}",
                table: "UserMedalEvent",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_UserScore_CreateTime",
                schema: $"{schema}",
                table: "UserScore",
                column: "CreateTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BasicConfig",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "GoodsRecord",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "MedalObtainTask",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "OperationRecord",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "OrderRecord",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "OverdueScoreClear",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "ScoreConsumeTask",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "ScoreLevel",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "ScoreManualProcess",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "ScoreObtainTask",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "ScoreRecieveRule",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "ScoreRecieveUser",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "SysMenuPermission",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "UserEventScore",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "UserMedal",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "UserMedalEvent",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "UserScore",
                schema: $"{schema}");
        }
    }
}
