using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.SceneManage.Database.Migrations.Migrations
{
    public partial class UpdateTemplate : Migration
    {
        private readonly string schema;
        public UpdateTemplate(string schema)
        {
            this.schema = schema;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: $"{schema}");

            migrationBuilder.AddColumn<string>(
                name: "Cover",
                schema: $"{schema}",
                table: "Template",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cover",
                schema: $"{schema}",
                table: "Template");
        }
    }
}
