using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.Navigation.Database.Migrations.Migrations
{
    public partial class v102_schema : Migration
    {
        private readonly string schema;
        public v102_schema(string schema)
        {
            this.schema = schema;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TitleStyle",
                schema: schema,
                table: "NavigationCatalogue",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleStyle",
                schema: schema,
                table: "Content",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TitleStyle",
                schema: schema,
                table: "NavigationCatalogue");

            migrationBuilder.DropColumn(
                name: "TitleStyle",
                schema: schema,
                table: "Content");
        }
    }
}
