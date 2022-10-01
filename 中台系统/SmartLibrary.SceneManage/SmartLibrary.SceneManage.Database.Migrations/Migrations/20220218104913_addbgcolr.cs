using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.SceneManage.Database.Migrations.Migrations
{
    public partial class addbgcolr : Migration
    {
        private readonly string schema;
        public addbgcolr(string schema)
        {
            this.schema = schema;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BackgroundColor",
                schema: $"{schema}",
                table: "Template",
                type: "integer",
                maxLength: 48,
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackgroundColor",
                schema: $"{schema}",
                table: "Template");
        }
    }
}
