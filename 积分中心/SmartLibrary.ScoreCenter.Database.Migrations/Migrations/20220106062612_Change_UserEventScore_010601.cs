using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.ScoreCenter.Database.Migrations.Migrations
{
    public partial class Change_UserEventScore_010601 : Migration
    {
        private readonly string schema;
        public Change_UserEventScore_010601(string schema)
        {
            this.schema = schema;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ScoreObtainDate",
                schema: $"{schema}",
                table: "UserEventScore",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ScoreExpireDate",
                schema: $"{schema}",
                table: "UserEventScore",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ScoreObtainDate",
                schema: $"{schema}",
                table: "UserEventScore",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ScoreExpireDate",
                schema: $"{schema}",
                table: "UserEventScore",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);
        }
    }
}
