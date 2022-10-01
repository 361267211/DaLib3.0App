using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.Navigation.Database.Migrations.Migrations
{
    public partial class v107 : Migration
    {
        // 自定义一个构造函数，并将Schema作为入参
        private readonly string schema;
        public v107(string schema)
        {
            this.schema = schema;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FootTemplate",
                schema: schema,
                table: "NavigationColumn",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeadTemplate",
                schema: schema,
                table: "NavigationColumn",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FootTemplate",
                schema: schema,
                table: "NavigationColumn");

            migrationBuilder.DropColumn(
                name: "HeadTemplate",
                schema: schema,
                table: "NavigationColumn");
        }
    }
}
