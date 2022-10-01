using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.SceneManage.Database.Migrations.Migrations
{
    public partial class updateremoveheaderfooterrequired : Migration
    {
        private readonly string schema;
        public updateremoveheaderfooterrequired(string schema)
        {
            this.schema = schema;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: $"{schema}");

            migrationBuilder.AlterColumn<string>(
                name: "LayoutId",
                schema: $"{schema}",
                table: "Template",
                type: "character varying(48)",
                maxLength: 48,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "LayoutId",
                schema: $"{schema}",
                table: "Scene",
                type: "character varying(48)",
                maxLength: 48,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "HeaderTemplateId",
                schema: $"{schema}",
                table: "Scene",
                type: "character varying(48)",
                maxLength: 48,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(48)",
                oldMaxLength: 48);

            migrationBuilder.AlterColumn<string>(
                name: "FooterTemplateId",
                schema: $"{schema}",
                table: "Scene",
                type: "character varying(48)",
                maxLength: 48,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(48)",
                oldMaxLength: 48);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LayoutId",
                schema: $"{schema}",
                table: "Template",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "character varying(48)",
                oldMaxLength: 48,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LayoutId",
                schema: $"{schema}",
                table: "Scene",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(48)",
                oldMaxLength: 48);

            migrationBuilder.AlterColumn<string>(
                name: "HeaderTemplateId",
                schema: $"{schema}",
                table: "Scene",
                type: "character varying(48)",
                maxLength: 48,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(48)",
                oldMaxLength: 48,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FooterTemplateId",
                schema: $"{schema}",
                table: "Scene",
                type: "character varying(48)",
                maxLength: 48,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(48)",
                oldMaxLength: 48,
                oldNullable: true);
        }
    }
}
