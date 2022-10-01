using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.News.Database.Migrations.Migrations
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
                name: "TemplateDetailDirectUrl",
                schema: schema,
                table: "NewsTemplate",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TemplateListDirectUrl",
                schema: schema,
                table: "NewsTemplate",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TemplateDetailDirectUrl",
                schema: schema,
                table: "NewsTemplate");

            migrationBuilder.DropColumn(
                name: "TemplateListDirectUrl",
                schema: schema,
                table: "NewsTemplate");
        }
    }
}
