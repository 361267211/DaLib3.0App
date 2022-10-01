using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.AppCenter.Database.Migrations.Migrations
{
    public partial class v20220328 : Migration
    {
        private readonly string _schema;

        public v20220328(string schema)
        {
            _schema = schema;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: _schema,
                table: "ThirdApplication",
                type: "integer",
                nullable: false,
                defaultValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                schema: _schema,
                table: "ThirdApplication");
        }
    }
}
