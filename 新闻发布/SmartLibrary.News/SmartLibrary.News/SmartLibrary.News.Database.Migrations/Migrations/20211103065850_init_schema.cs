using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.News.Database.Migrations.Migrations
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
                name: schema);

            migrationBuilder.CreateTable(
                name: "LableInfo",
                schema: schema,
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LableInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NewsBodyTemplate",
                schema: schema,
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Note = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    PreviewPic = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsBodyTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NewsColumn",
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
                    table.PrimaryKey("PK_NewsColumn", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NewsColumnPermissions",
                schema: schema,
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ColumnID = table.Column<string>(type: "text", nullable: false),
                    ManagerID = table.Column<Guid>(type: "uuid", nullable: false),
                    Manager = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Permission = table.Column<int>(type: "integer", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsColumnPermissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NewsContent",
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
                    Content = table.Column<string>(type: "text", nullable: false),
                    Cover = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Author = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Publisher = table.Column<Guid>(type: "uuid", nullable: false),
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
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsContent", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NewsContentExpend",
                schema: schema,
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ColumnID = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    FiledName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Filed = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsContentExpend", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NewsSettings",
                schema: schema,
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    SensitiveWords = table.Column<int>(type: "integer", nullable: false),
                    Comments = table.Column<int>(type: "integer", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NewsTemplate",
                schema: schema,
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    HeadTemplate = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    FootTemplate = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Note = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    PreviewPic = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysMenuCategory",
                schema: schema,
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false),
                    Sort = table.Column<int>(type: "integer", nullable: false),
                    Visible = table.Column<bool>(type: "boolean", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysMenuCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysMenuPermission",
                schema: schema,
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysMenuPermission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysRole",
                schema: schema,
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Remark = table.Column<string>(type: "text", nullable: true),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysRoleMenu",
                schema: schema,
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleID = table.Column<Guid>(type: "uuid", nullable: false),
                    MenuPermissionID = table.Column<Guid>(type: "uuid", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysRoleMenu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysUserRole",
                schema: schema,
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserID = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleID = table.Column<Guid>(type: "uuid", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysUserRole", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LableInfo",
                schema: schema);

            migrationBuilder.DropTable(
                name: "NewsBodyTemplate",
                schema: schema);

            migrationBuilder.DropTable(
                name: "NewsColumn",
                schema: schema);

            migrationBuilder.DropTable(
                name: "NewsColumnPermissions",
                schema: schema);

            migrationBuilder.DropTable(
                name: "NewsContent",
                schema: schema);

            migrationBuilder.DropTable(
                name: "NewsContentExpend",
                schema: schema);

            migrationBuilder.DropTable(
                name: "NewsSettings",
                schema: schema);

            migrationBuilder.DropTable(
                name: "NewsTemplate",
                schema: schema);

            migrationBuilder.DropTable(
                name: "SysMenuCategory",
                schema: schema);

            migrationBuilder.DropTable(
                name: "SysMenuPermission",
                schema: schema);

            migrationBuilder.DropTable(
                name: "SysRole",
                schema: schema);

            migrationBuilder.DropTable(
                name: "SysRoleMenu",
                schema: schema);

            migrationBuilder.DropTable(
                name: "SysUserRole",
                schema: schema);
        }
    }
}
