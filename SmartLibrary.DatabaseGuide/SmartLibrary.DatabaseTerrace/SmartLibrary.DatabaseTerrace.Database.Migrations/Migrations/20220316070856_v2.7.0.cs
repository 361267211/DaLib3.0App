using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.DatabaseTerrace.Database.Migrations.Migrations
{
    public partial class v270 : Migration
    {
        // 自定义一个构造函数，并将Schema作为入参
        private readonly string schema;
        public v270(string schema)
        {
            this.schema = schema;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InformationEditor",
                schema: schema,
                table: "DatabaseTerrace",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "InformationText",
                schema: schema,
                table: "DatabaseTerrace",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResourceStatisticsEditor",
                schema: schema,
                table: "DatabaseTerrace",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "ResourceStatisticsText",
                schema: schema,
                table: "DatabaseTerrace",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UseGuideEditor",
                schema: schema,
                table: "DatabaseTerrace",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "UseGuideText",
                schema: schema,
                table: "DatabaseTerrace",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InformationEditor",
                schema: schema,
                table: "DatabaseTerrace");

            migrationBuilder.DropColumn(
                name: "InformationText",
                schema: schema,
                table: "DatabaseTerrace");

            migrationBuilder.DropColumn(
                name: "ResourceStatisticsEditor",
                schema: schema,
                table: "DatabaseTerrace");

            migrationBuilder.DropColumn(
                name: "ResourceStatisticsText",
                schema: schema,
                table: "DatabaseTerrace");

            migrationBuilder.DropColumn(
                name: "UseGuideEditor",
                schema: schema,
                table: "DatabaseTerrace");

            migrationBuilder.DropColumn(
                name: "UseGuideText",
                schema: schema,
                table: "DatabaseTerrace");
        }
    }
}
