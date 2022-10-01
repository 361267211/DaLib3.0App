using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.User.Database.Migrations.Migrations
{
    public partial class user_supervisor : Migration
    {
        private readonly string schema;
        public user_supervisor(string schema)
        {
            this.schema = schema;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSuperVisor",
                schema: $"{schema}",
                table: "User",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSuperVisor",
                schema: $"{schema}",
                table: "User");
        }
    }
}
