using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.AppCenter.Database.Migrations.Migrations
{
    public partial class v20220114 : Migration
    {
        private readonly string _schema;

        public v20220114(string schema)
        {
            _schema = schema;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserSetId",
                schema: _schema,
                table: "AppUser",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<string>(
                name: "AppId",
                schema: _schema,
                table: "AppUser",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<string>(
                name: "NavigationId",
                schema: _schema,
                table: "AppNavigation",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "AppId",
                schema: _schema,
                table: "AppNavigation",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ManageRoleId",
                schema: _schema,
                table: "AppManager",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<string>(
                name: "AppId",
                schema: _schema,
                table: "AppManager",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldMaxLength: 32);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "UserSetId",
                schema: _schema,
                table: "AppUser",
                type: "uuid",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<Guid>(
                name: "AppId",
                schema: _schema,
                table: "AppUser",
                type: "uuid",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<Guid>(
                name: "NavigationId",
                schema: _schema,
                table: "AppNavigation",
                type: "uuid",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<Guid>(
                name: "AppId",
                schema: _schema,
                table: "AppNavigation",
                type: "uuid",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<Guid>(
                name: "ManageRoleId",
                schema: _schema,
                table: "AppManager",
                type: "uuid",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<Guid>(
                name: "AppId",
                schema: _schema,
                table: "AppManager",
                type: "uuid",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }
    }
}
