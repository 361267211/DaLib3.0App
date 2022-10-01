using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.AppCenter.Database.Migrations.Migrations
{
    public partial class v20220329 : Migration
    {
        private readonly string _schema;

        public v20220329(string schema)
        {
            _schema = schema;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppExplain",
                schema: _schema,
                table: "ThirdApplication",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppExplain",
                schema: _schema,
                table: "ThirdApplication");
        }
    }
}
