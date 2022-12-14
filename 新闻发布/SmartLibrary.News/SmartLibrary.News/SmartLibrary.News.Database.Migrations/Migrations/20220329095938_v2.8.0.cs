using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.News.Database.Migrations.Migrations
{
    public partial class v280 : Migration
    {
        // 自定义一个构造函数，并将Schema作为入参
        private readonly string schema;
        public v280(string schema)
        {
            this.schema = schema;
        }


        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Alias",
                schema:schema,
                table: "NewsColumn",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Alias",
                schema:schema,
                table: "NewsColumn",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
