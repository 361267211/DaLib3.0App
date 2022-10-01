using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.Open.Database.Migrations.Migrations
{
    public partial class updateAppEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EventType",
                table: "AppEvent",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventType",
                table: "AppEvent");
        }
    }
}
