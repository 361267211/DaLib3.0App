using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.AppCenter.Database.Migrations.Migrations
{
    public partial class v20220107 : Migration
    {
        private readonly string _schema;

        public v20220107(string schema)
        {
            _schema = schema;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppColumnInfo",
                schema: _schema,
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    AppId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AppRouteCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ColumnName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    ColumnId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ColumnCreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    VisitUrl = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppColumnInfo", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppColumnInfo",
                schema: _schema);
        }
    }
}
