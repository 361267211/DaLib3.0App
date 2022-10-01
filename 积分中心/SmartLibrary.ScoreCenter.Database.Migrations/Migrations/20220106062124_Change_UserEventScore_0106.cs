using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.ScoreCenter.Database.Migrations.Migrations
{
    public partial class Change_UserEventScore_0106 : Migration
    {
        private readonly string schema;
        public Change_UserEventScore_0106(string schema)
        {
            this.schema = schema;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ScoreExpireDate",
                schema: $"{schema}",
                table: "UserEventScore",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ScoreObtainDate",
                schema: $"{schema}",
                table: "UserEventScore",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScoreExpireDate",
                schema: $"{schema}",
                table: "UserEventScore");

            migrationBuilder.DropColumn(
                name: "ScoreObtainDate",
                schema: $"{schema}",
                table: "UserEventScore");
        }
    }
}
