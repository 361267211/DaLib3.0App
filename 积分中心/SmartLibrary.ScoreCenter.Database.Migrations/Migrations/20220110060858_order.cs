using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.ScoreCenter.Database.Migrations.Migrations
{
    public partial class order : Migration
    {
        private readonly string schema;
        public order(string schema)
        {
            this.schema = schema;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExchangeScore",
                schema: $"{schema}",
                table: "OrderRecord",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExchangeScore",
                schema: $"{schema}",
                table: "OrderRecord");
        }
    }
}
