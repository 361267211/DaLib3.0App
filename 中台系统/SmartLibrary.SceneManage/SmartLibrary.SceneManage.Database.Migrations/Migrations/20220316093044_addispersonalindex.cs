using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.SceneManage.Database.Migrations.Migrations
{
    public partial class addispersonalindex : Migration
    { 
        // 自定义一个构造函数，并将Schema作为入参
        private readonly string schema;
        public addispersonalindex(string schema)
        {
            this.schema = schema;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPersonalIndex",
                schema: schema,
                table: "Scene",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPersonalIndex",
                schema: "public",
                table: "Scene");
        }
    }
}
