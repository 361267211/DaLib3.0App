using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.Navigation.Database.Migrations.Migrations
{
    public partial class v108 : Migration
    {
        // 自定义一个构造函数，并将Schema作为入参
        private readonly string schema;
        public v108(string schema)
        {
            this.schema = schema;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VisitingList",
                schema: schema,
                table: "NavigationColumn",
                newName: "UserTypes");

            migrationBuilder.AddColumn<string>(
                name: "UserGroups",
                schema: schema,
                table: "NavigationColumn",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserGroups",
                schema: schema,
                table: "NavigationColumn");

            migrationBuilder.RenameColumn(
                name: "UserTypes",
                schema: schema,
                table: "NavigationColumn",
                newName: "VisitingList");
        }
    }
}
