using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.SceneManage.Database.Migrations.Migrations
{
    public partial class removerequired : Migration
    {
        // 自定义一个构造函数，并将Schema作为入参
        private readonly string schema;
        public removerequired(string schema)
        {
            this.schema = schema;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Logo",
                schema: schema,
                table: "TerminalInstance",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Logo",
                schema: schema,
                table: "TerminalInstance",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
