using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.News.Database.Migrations.Migrations
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
            migrationBuilder.DropColumn(
                name: "CoverSize",
                schema: schema,
                table: "NewsColumn");

            migrationBuilder.AddColumn<int>(
                name: "CoverHeight",
                schema: schema,
                table: "NewsColumn",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CoverWidth",
                schema: schema,
                table: "NewsColumn",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverHeight",
                schema: schema,
                table: "NewsColumn");

            migrationBuilder.DropColumn(
                name: "CoverWidth",
                schema: schema,
                table: "NewsColumn");

            migrationBuilder.AddColumn<string>(
                name: "CoverSize",
                schema: schema,
                table: "NewsColumn",
                type: "text",
                nullable: true);
        }
    }
}
