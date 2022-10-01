using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.News.Database.Migrations.Migrations
{
    public partial class v150 : Migration
    {
        private readonly string schema;
        public v150(string schema)
        {
            this.schema = schema;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FootTemplate",
                schema: schema,
                table: "NewsColumn",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeadTemplate",
                schema: schema,
                table: "NewsColumn",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FootTemplate",
                schema: schema,
                table: "NewsColumn");

            migrationBuilder.DropColumn(
                name: "HeadTemplate",
                schema: schema,
                table: "NewsColumn");
        }
    }
}
