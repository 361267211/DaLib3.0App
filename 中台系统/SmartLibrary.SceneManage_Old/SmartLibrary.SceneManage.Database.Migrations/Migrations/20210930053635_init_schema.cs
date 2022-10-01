using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.SceneManage.Database.Migrations.Migrations
{
    public partial class init_schema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Layout",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ScreenCount = table.Column<int>(type: "int", nullable: false),
                    ColumnCount = table.Column<int>(type: "int", nullable: false),
                    AspectRatio = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    TerminalType = table.Column<int>(type: "int", nullable: false),
                    DeleteFlag = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Layout", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Scene",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TerminalInstanceId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    LayoutId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    TemplateId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    HeaderTemplateId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    FooterTemplateId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    ThemeColor = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Cover = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    VisitorLimitType = table.Column<int>(type: "int", nullable: false),
                    VisitUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsSystemScene = table.Column<bool>(type: "bit", nullable: false),
                    DeleteFlag = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scene", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SceneApp",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SceneId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    AppId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    AppWidgetId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    AppPlateIds = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TopCount = table.Column<int>(type: "int", nullable: false),
                    SortType = table.Column<int>(type: "int", nullable: false),
                    XIndex = table.Column<int>(type: "int", nullable: false),
                    YIndex = table.Column<int>(type: "int", nullable: false),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    DeleteFlag = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SceneApp", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SceneUser",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SceneId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    UserSetId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    UserSetType = table.Column<int>(type: "int", nullable: false),
                    DeleteFlag = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SceneUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Template",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Router = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LayoutId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Type = table.Column<int>(type: "int", maxLength: 32, nullable: false),
                    DefaultHeaderTemplateId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    DefaultFooterTemplateId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    DeleteFlag = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Template", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TerminalInstance",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TerminalType = table.Column<int>(type: "int", nullable: false),
                    KeyWords = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Logo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VisitUrl = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsSystemInstance = table.Column<bool>(type: "bit", nullable: false),
                    DeleteFlag = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TerminalInstance", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ThemeColor",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    DeleteFlag = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThemeColor", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Layout",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Scene",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SceneApp",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SceneUser",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Template",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TerminalInstance",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ThemeColor",
                schema: "dbo");
        }
    }
}
