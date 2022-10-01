using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SmartLibrary.DatabaseTerrace.Database.Migrations.Migrations
{
    public partial class init_schema : Migration
    {
        // 自定义一个构造函数，并将Schema作为入参
        private readonly string schema;
        public init_schema(string schema)
        {
            this.schema = schema;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: $"dbo.{schema}");

            migrationBuilder.CreateTable(
                name: "Asset",
                schema: $"dbo.{schema}",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Content = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Language = table.Column<double>(type: "double precision", nullable: true),
                    Type = table.Column<double>(type: "double precision", nullable: true),
                    EnglihName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Plate = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asset", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomLabel",
                schema: $"dbo.{schema}",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LabelName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Createtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomLabel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DatabaseAcessUrl",
                schema: $"dbo.{schema}",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DatabaseID = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Url = table.Column<string>(type: "text", nullable: true),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatabaseAcessUrl", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DatabaseColumn",
                schema: $"dbo.{schema}",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ColumnName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MatchCount = table.Column<int>(type: "integer", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatabaseColumn", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DatabaseColumnRule",
                schema: $"dbo.{schema}",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ColumnID = table.Column<Guid>(type: "uuid", maxLength: 50, nullable: false),
                    RuleKey = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RuleValue = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RuleValueName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatabaseColumnRule", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DatabaseSubscriber",
                schema: $"dbo.{schema}",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DatabaseID = table.Column<Guid>(type: "uuid", nullable: false),
                    UserKey = table.Column<string>(type: "text", nullable: true),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatabaseSubscriber", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DatabaseTerrace",
                schema: $"dbo.{schema}",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Abbreviation = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DatabaseProviderID = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Initials = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ArticleTypes = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DomainClcs = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DomainEscs = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PurchaseType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Language = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Label = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ExpiryBeginTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ExpiryEndTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Cover = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Remark = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    IndirectUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    WhiteList = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Introduction = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Information = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    UseGuide = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    IsShow = table.Column<bool>(type: "boolean", nullable: false),
                    MonthClickNum = table.Column<long>(type: "bigint", nullable: false),
                    TotalClickNum = table.Column<long>(type: "bigint", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatabaseTerrace", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DatabaseTerraceSettings",
                schema: $"dbo.{schema}",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Introduce = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsLoginAcess = table.Column<bool>(type: "boolean", nullable: false),
                    IsOpenComment = table.Column<bool>(type: "boolean", nullable: false),
                    IsOpenFeedback = table.Column<bool>(type: "boolean", nullable: false),
                    CanFilterCustomLabel = table.Column<bool>(type: "boolean", nullable: false),
                    IsOpenVpn = table.Column<bool>(type: "boolean", nullable: false),
                    IsOpenExternalUrl = table.Column<bool>(type: "boolean", nullable: false),
                    Template = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatabaseTerraceSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DomainEscsAttr",
                schema: $"dbo.{schema}",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    DomainEscsName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ParentDomainEscs = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainEscsAttr", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                schema: $"dbo.{schema}",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DatabaseColumn_CreatedTime",
                schema: $"dbo.{schema}",
                table: "DatabaseColumn",
                column: "CreatedTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Asset",
                schema: $"dbo.{schema}");

            migrationBuilder.DropTable(
                name: "CustomLabel",
                schema: $"dbo.{schema}");

            migrationBuilder.DropTable(
                name: "DatabaseAcessUrl",
                schema: $"dbo.{schema}");

            migrationBuilder.DropTable(
                name: "DatabaseColumn",
                schema: $"dbo.{schema}");

            migrationBuilder.DropTable(
                name: "DatabaseColumnRule",
                schema: $"dbo.{schema}");

            migrationBuilder.DropTable(
                name: "DatabaseSubscriber",
                schema: $"dbo.{schema}");

            migrationBuilder.DropTable(
                name: "DatabaseTerrace",
                schema: $"dbo.{schema}");

            migrationBuilder.DropTable(
                name: "DatabaseTerraceSettings",
                schema: $"dbo.{schema}");

            migrationBuilder.DropTable(
                name: "DomainEscsAttr",
                schema: $"dbo.{schema}");

            migrationBuilder.DropTable(
                name: "Person",
                schema: $"dbo.{schema}");
        }
    }
}
