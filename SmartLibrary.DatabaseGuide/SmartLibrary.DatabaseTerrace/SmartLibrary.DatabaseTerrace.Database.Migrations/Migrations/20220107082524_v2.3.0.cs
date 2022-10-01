using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.DatabaseTerrace.Database.Migrations.Migrations
{
    public partial class v230 : Migration
    {
        // 自定义一个构造函数，并将Schema作为入参
        private readonly string schema;
        public v230(string schema)
        {
            this.schema = schema;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FootTemplate",
                schema: $"dbo.{schema}",
                table: "DatabaseTerraceSettings",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeadTemplate",
                schema: $"dbo.{schema}",
                table: "DatabaseTerraceSettings",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FootTemplate",
                schema: $"dbo.{schema}",
                table: "DatabaseTerraceSettings");

            migrationBuilder.DropColumn(
                name: "HeadTemplate",
                schema: $"dbo.{schema}",
                table: "DatabaseTerraceSettings");
        }
    }
}
