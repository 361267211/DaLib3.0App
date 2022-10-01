using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.SceneManage.Database.Migrations.Migrations
{
    public partial class updatetemplate : Migration
    {
        private readonly string schema;
        public updatetemplate(string schema)
        {
            this.schema = schema;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                   name: $"{schema}");

            migrationBuilder.DropColumn(
                name: "AspectRatio",
                 schema: $"{schema}",
                table: "Layout");

            migrationBuilder.DropColumn(
                name: "ColumnCount",
                 schema: $"{schema}",
                table: "Layout");

            migrationBuilder.DropColumn(
                name: "ScreenCount",
                 schema: $"{schema}",
                table: "Layout");

            migrationBuilder.AddColumn<string>(
                name: "AspectRatio",
                 schema: $"{schema}",
                table: "Template",
                type: "character varying(48)",
                maxLength: 48,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ColumnCount",
                 schema: $"{schema}",
                table: "Template",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsLock",
                 schema: $"{schema}",
                table: "Template",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ScreenCount",
                 schema: $"{schema}",
                table: "Template",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AspectRatio",
                 schema: $"{schema}",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "ColumnCount",
                 schema: $"{schema}",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "IsLock",
                 schema: $"{schema}",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "ScreenCount",
                 schema: $"{schema}",
                table: "Template");

            migrationBuilder.AddColumn<string>(
                name: "AspectRatio",
                 schema: $"{schema}",
                table: "Layout",
                type: "character varying(48)",
                maxLength: 48,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ColumnCount",
                 schema: $"{schema}",
                table: "Layout",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ScreenCount",
                 schema: $"{schema}",
                table: "Layout",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
