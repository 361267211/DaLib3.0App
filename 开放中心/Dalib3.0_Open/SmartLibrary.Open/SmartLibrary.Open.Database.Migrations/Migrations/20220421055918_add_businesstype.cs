using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.Open.Database.Migrations.Migrations
{
    public partial class add_businesstype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BusinessType",
                table: "AppBranchEntryPoint",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessType",
                table: "AppBranchEntryPoint");
        }
    }
}
