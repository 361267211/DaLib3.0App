using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.DatabaseTerrace.Database.Migrations.Migrations
{
    public partial class v108 : Migration
    {
        // 自定义一个构造函数，并将Schema作为入参
        private readonly string schema;
        public v108(string schema)
        {
            this.schema = schema;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedTime",
                schema: $"dbo.{schema}",
                table: "SysRole");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                schema: $"dbo.{schema}",
                table: "SysRole");

            migrationBuilder.AlterColumn<Guid>(
                name: "Pid",
                schema: $"dbo.{schema}",
                table: "SysMenuPermission",
                type: "uuid",
                nullable: false,
                comment: "父级id",
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedTime",
                schema: $"dbo.{schema}",
                table: "SysRole",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedTime",
                schema: $"dbo.{schema}",
                table: "SysRole",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Pid",
                schema: $"dbo.{schema}",
                table: "SysMenuPermission",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "父级id");
        }
    }
}
