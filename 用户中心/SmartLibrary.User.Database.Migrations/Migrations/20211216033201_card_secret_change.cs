using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.User.Database.Migrations.Migrations
{
    public partial class card_secret_change : Migration
    {
        private readonly string schema;
        public card_secret_change(string schema)
        {
            this.schema = schema;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SecretChangeTime",
                schema: $"{schema}",
                table: "Card",
                type: "timestamp without time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecretChangeTime",
                schema: $"{schema}",
                table: "Card");
        }
    }
}
