using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.Open.Database.Migrations.Migrations
{
    public partial class addlogo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "Customer",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SimpleLogoUrl",
                table: "Customer",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "SimpleLogoUrl",
                table: "Customer");
        }
    }
}
