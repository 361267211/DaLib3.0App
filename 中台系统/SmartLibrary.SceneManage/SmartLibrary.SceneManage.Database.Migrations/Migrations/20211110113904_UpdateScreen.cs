using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.SceneManage.Database.Migrations.Migrations
{
    public partial class UpdateScreen : Migration
    {
        private readonly string schema;
        public UpdateScreen(string schema)
        {
            this.schema = schema;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: $"{schema}");

            migrationBuilder.AddColumn<int>(
                name: "Height",
                schema: $"{schema}",
                table: "SceneScreen",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                schema: $"{schema}",
                table: "SceneScreen");
        }
    }
}
