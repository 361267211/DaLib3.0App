using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.DatabaseTerrace.Database.Migrations.Migrations
{
    public partial class v200 : Migration
    {
        // 自定义一个构造函数，并将Schema作为入参
        private readonly string schema;
        public v200(string schema)
        {
            this.schema = schema;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserGroups",
                schema: $"dbo.{schema}",
                table: "DatabaseTerrace",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserTypes",
                schema: $"dbo.{schema}",
                table: "DatabaseTerrace",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserGroups",
                schema: $"dbo.{schema}",
                table: "DatabaseTerrace");

            migrationBuilder.DropColumn(
                name: "UserTypes",
                schema: $"dbo.{schema}",
                table: "DatabaseTerrace");
        }
    }
}
