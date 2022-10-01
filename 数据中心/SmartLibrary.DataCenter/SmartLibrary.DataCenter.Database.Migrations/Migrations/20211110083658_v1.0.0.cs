using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.DataCenter.Database.Migrations.Migrations
{
    public partial class v100 : Migration
    {
        // 自定义一个构造函数，并将Schema作为入参
        private readonly string schema;
        public v100(string schema)
        {
            this.schema = schema;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DatabaseCollectKind",
                schema: $"dbo.{schema}",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DatabaseName = table.Column<string>(type: "text", nullable: true),
                    EnglishAbbr = table.Column<string>(type: "text", nullable: true),
                    NationName = table.Column<string>(type: "text", nullable: true),
                    PubNum = table.Column<string>(type: "text", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatabaseCollectKind", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DatabaseProvider",
                schema: $"dbo.{schema}",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProviderName = table.Column<string>(type: "text", nullable: true),
                    Remark = table.Column<string>(type: "text", nullable: true),
                    Operator = table.Column<string>(type: "text", nullable: true),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: true),
                    Gathering = table.Column<string>(type: "text", nullable: true),
                    Contacts = table.Column<string>(type: "text", nullable: true),
                    District = table.Column<string>(type: "text", nullable: true),
                    Tel = table.Column<string>(type: "text", nullable: true),
                    ContractsTel = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<int>(type: "integer", nullable: false),
                    Introduction = table.Column<string>(type: "text", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatabaseProvider", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProviderResource",
                schema: $"dbo.{schema}",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Provider = table.Column<string>(type: "text", nullable: true),
                    LinkTitle = table.Column<string>(type: "text", nullable: true),
                    LinkUrl = table.Column<string>(type: "text", nullable: true),
                    TerraceFullName = table.Column<string>(type: "text", nullable: true),
                    TerraceShortName = table.Column<string>(type: "text", nullable: true),
                    ServiceType = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Medium = table.Column<int>(type: "integer", nullable: false),
                    LanguageKind = table.Column<int>(type: "integer", nullable: false),
                    FrequentUsedFlag = table.Column<bool>(type: "boolean", nullable: false),
                    ProviderType = table.Column<int>(type: "integer", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderResource", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DatabaseCollectKind",
                schema: $"dbo.{schema}");

            migrationBuilder.DropTable(
                name: "DatabaseProvider",
                schema: $"dbo.{schema}");

            migrationBuilder.DropTable(
                name: "ProviderResource",
                schema: $"dbo.{schema}");
        }
    }
}
