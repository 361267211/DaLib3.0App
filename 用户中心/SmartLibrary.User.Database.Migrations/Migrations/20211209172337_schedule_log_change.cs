using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.User.Database.Migrations.Migrations
{
    public partial class schedule_log_change : Migration
    {
        private readonly string schema;
        public schedule_log_change(string schema)
        {
            this.schema = schema;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AsyncReaderId",
                schema: $"{schema}",
                table: "User");

            migrationBuilder.AlterColumn<string>(
                name: "TaskParam",
                schema: "public",
                table: "SchedulerEntity",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AdapterParm",
                schema: "public",
                table: "SchedulerEntity",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AsyncReaderId",
                schema: $"{schema}",
                table: "Card",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AsyncReaderId",
                schema: $"{schema}",
                table: "Card");

            migrationBuilder.AddColumn<string>(
                name: "AsyncReaderId",
                schema: $"{schema}",
                table: "User",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TaskParam",
                schema: "public",
                table: "SchedulerEntity",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AdapterParm",
                schema: "public",
                table: "SchedulerEntity",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);
        }
    }
}
