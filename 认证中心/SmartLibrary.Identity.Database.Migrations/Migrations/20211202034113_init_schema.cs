using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.Identity.Database.Migrations.Migrations
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
                name: "LoginConfigSet",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    LoginType = table.Column<int>(type: "integer", nullable: false),
                    Icon = table.Column<string>(type: "text", nullable: true),
                    LoginName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Desc = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Enable = table.Column<bool>(type: "boolean", nullable: false),
                    IsOpen = table.Column<bool>(type: "boolean", nullable: false),
                    LoginConfig = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Sort = table.Column<int>(type: "integer", nullable: false),
                    NeedConfig = table.Column<bool>(type: "boolean", nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginConfigSet", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RegisterConfigSet",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    OpenRegistion = table.Column<bool>(type: "boolean", nullable: false),
                    RegisteType = table.Column<int>(type: "integer", nullable: false),
                    RegisteFlow = table.Column<int>(type: "integer", nullable: false),
                    ProtoUrl = table.Column<string>(type: "text", nullable: true),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisterConfigSet", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SysMenuPermission",
                schema: $"{schema}",
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
                name: "UserRegisterProperty",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    PropertyCode = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    IsEnable = table.Column<bool>(type: "boolean", nullable: false),
                    IsCheck = table.Column<bool>(type: "boolean", nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRegisterProperty", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoginConfigSet_CreateTime",
                schema: $"{schema}",
                table: "LoginConfigSet",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_RegisterConfigSet_CreateTime",
                schema: $"{schema}",
                table: "RegisterConfigSet",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_UserRegisterProperty_CreateTime",
                schema: $"{schema}",
                table: "UserRegisterProperty",
                column: "CreateTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoginConfigSet",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "RegisterConfigSet",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "SysMenuPermission",
                schema: $"{schema}");

            migrationBuilder.DropTable(
                name: "UserRegisterProperty",
                schema: $"{schema}");
        }
    }
}
