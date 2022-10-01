using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.Navigation.Database.Migrations.Migrations
{
    public partial class v103_schema : Migration
    {
        private readonly string schema;
        public v103_schema(string schema)
        {
            this.schema = schema;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HitCount",
                schema: schema,
                table: "Content",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HitCount",
                schema: schema,
                table: "Content");
        }
    }
}
