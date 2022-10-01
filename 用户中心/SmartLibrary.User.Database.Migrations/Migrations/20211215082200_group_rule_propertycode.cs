using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.User.Database.Migrations.Migrations
{
    public partial class group_rule_propertycode : Migration
    {
        private readonly string schema;
        public group_rule_propertycode(string schema)
        {
            this.schema = schema;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PropertyCode",
                schema: $"{schema}",
                table: "PropertyGroupRule",
                type: "character varying(100)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PropertyCode",
                schema: $"{schema}",
                table: "PropertyGroupRule");
        }
    }
}
