using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.SceneManage.Database.Migrations.Migrations
{
    public partial class editbgcolr : Migration
    {
        private readonly string schema;
        public editbgcolr(string schema)
        {
            this.schema = schema;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BackgroundColor",
                schema: $"{schema}",
                table: "Template",
                type: "character varying(48)",
                maxLength: 48,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldMaxLength: 48);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "BackgroundColor",
                schema: $"{schema}",
                table: "Template",
                type: "integer",
                maxLength: 48,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(48)",
                oldMaxLength: 48);
        }
    }
}
