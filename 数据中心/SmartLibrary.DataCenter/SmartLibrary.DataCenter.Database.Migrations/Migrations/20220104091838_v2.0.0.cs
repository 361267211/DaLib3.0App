using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.DataCenter.Database.Migrations.Migrations
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
            migrationBuilder.AddColumn<int>(
                name: "DatabaseCode",
                schema: $"dbo.{schema}",
                table: "ProviderResource",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DatabaseCode",
                schema: $"dbo.{schema}",
                table: "DatabaseProvider",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DatabaseCode",
                schema: $"dbo.{schema}",
                table: "ProviderResource");

            migrationBuilder.DropColumn(
                name: "DatabaseCode",
                schema: $"dbo.{schema}",
                table: "DatabaseProvider");
        }
    }
}
