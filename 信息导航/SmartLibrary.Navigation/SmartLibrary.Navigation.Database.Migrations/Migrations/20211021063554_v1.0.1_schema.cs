using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.Navigation.Database.Migrations.Migrations
{
    public partial class v101_schema : Migration
    {
        private readonly string schema;
        public v101_schema(string schema)
        {
            this.schema = schema;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatorName",
                schema:  schema,
                table: "NavigationCatalogue",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorName",
                schema:  schema,
                table: "NavigationCatalogue");
        }
    }
}
