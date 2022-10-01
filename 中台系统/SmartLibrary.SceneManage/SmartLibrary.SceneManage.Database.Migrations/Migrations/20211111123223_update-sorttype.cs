using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.SceneManage.Database.Migrations.Migrations
{
    public partial class updatesorttype : Migration
    {
        private readonly string schema;
        public updatesorttype(string schema)
        {
            this.schema = schema;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                   name: $"{schema}");

            migrationBuilder.AlterColumn<string>(
                name: "SortType",
                schema: $"{schema}",
                table: "SceneAppPlate",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SortType",
                schema: $"{schema}",
                table: "SceneAppPlate",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
