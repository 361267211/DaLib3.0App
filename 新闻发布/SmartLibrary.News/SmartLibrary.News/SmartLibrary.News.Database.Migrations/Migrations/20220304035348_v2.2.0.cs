using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.News.Database.Migrations.Migrations
{
    public partial class v220 : Migration
    {
        // 自定义一个构造函数，并将Schema作为入参
        private readonly string schema;
        public v220(string schema)
        {
            this.schema = schema;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NewsColumnBack",
                schema: schema,
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Alias = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Label = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Terminals = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Extension = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    LinkUrl = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DefaultTemplate = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    HeadTemplate = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    FootTemplate = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    SideList = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SysMesList = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsOpenCover = table.Column<int>(type: "integer", nullable: false),
                    CoverSize = table.Column<string>(type: "text", nullable: true),
                    IsLoginAcess = table.Column<int>(type: "integer", nullable: false),
                    VisitingList = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsOpenComment = table.Column<int>(type: "integer", nullable: false),
                    IsOpenAudit = table.Column<int>(type: "integer", nullable: false),
                    AuditFlow = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsColumnBack", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NewsContentBack",
                schema: schema,
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ColumnID = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ColumnIDs = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    Title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TitleStyle = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    SubTitle = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ParentCatalogue = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Content = table.Column<string>(type: "text", nullable: true),
                    Cover = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Author = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Publisher = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PublisherName = table.Column<string>(type: "text", nullable: true),
                    PublishDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Terminals = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AuditStatus = table.Column<int>(type: "integer", nullable: false),
                    Keywords = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    JumpLink = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    HitCount = table.Column<int>(type: "integer", nullable: false),
                    OrderNum = table.Column<int>(type: "integer", nullable: false),
                    AuditProcessJson = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ExpendFiled1 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ExpendFiled2 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ExpendFiled3 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ExpendFiled4 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ExpendFiled5 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    ExternalLink = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsContentBack", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewsColumnBack",
                schema: schema);

            migrationBuilder.DropTable(
                name: "NewsContentBack",
                schema: schema);
        }
    }
}
