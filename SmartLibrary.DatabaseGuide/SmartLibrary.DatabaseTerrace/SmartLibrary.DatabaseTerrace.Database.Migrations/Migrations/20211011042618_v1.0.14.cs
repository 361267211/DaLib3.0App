using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.DatabaseTerrace.Database.Migrations.Migrations
{
    public partial class v1014 : Migration
    {
        // 自定义一个构造函数，并将Schema作为入参
        private readonly string schema;
        public v1014(string schema)
        {
            this.schema = schema;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                schema: $"dbo.{schema}",
                table: "SysMenuPermission");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: $"dbo.{schema}",
                table: "SysMenuPermission",
                type: "text",
                nullable: true);
        }
    }
}
