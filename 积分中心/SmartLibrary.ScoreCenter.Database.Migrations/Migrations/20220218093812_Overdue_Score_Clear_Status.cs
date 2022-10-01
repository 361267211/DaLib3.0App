using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.ScoreCenter.Database.Migrations.Migrations
{
    public partial class Overdue_Score_Clear_Status : Migration
    {
        private readonly string schema;
        public Overdue_Score_Clear_Status(string schema)
        {
            this.schema = schema;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: $"{schema}",
                table: "OverdueScoreClear",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                schema: $"{schema}",
                table: "OverdueScoreClear");
        }
    }
}
