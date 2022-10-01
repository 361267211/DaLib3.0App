using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.News.Database.Migrations.Migrations
{
    public partial class v290 : Migration
    {
        // 自定义一个构造函数，并将Schema作为入参
        private readonly string schema;
        public v290(string schema)
        {
            this.schema = schema;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                schema:schema,
                table: "NewsContent",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                schema:schema,
                table: "NewsContent",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);
        }
    }
}
