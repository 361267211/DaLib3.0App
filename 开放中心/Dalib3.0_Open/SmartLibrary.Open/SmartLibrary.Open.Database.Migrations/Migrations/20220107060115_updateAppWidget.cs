using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.Open.Database.Migrations.Migrations
{
    public partial class updateAppWidget : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "AppWidget",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SceneType",
                table: "AppWidget",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "AppWidget",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "AppWidget");

            migrationBuilder.DropColumn(
                name: "SceneType",
                table: "AppWidget");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "AppWidget");
        }
    }
}
