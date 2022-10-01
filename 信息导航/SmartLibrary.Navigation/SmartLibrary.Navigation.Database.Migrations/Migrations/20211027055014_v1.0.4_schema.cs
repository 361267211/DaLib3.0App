using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.Navigation.Database.Migrations.Migrations
{
    public partial class v104_schema : Migration
    {
        private readonly string schema;
        public v104_schema(string schema)
        {
            this.schema = schema;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Publisher",
                schema: schema,
                table: "Content",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Creator",
                schema: schema,
                table: "Content",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "CreatorName",
                schema: schema,
                table: "Content",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Creator",
                schema: schema,
                table: "Content");

            migrationBuilder.DropColumn(
                name: "CreatorName",
                schema: schema,
                table: "Content");

            migrationBuilder.AlterColumn<string>(
                name: "Publisher",
                schema: schema,
                table: "Content",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);
        }
    }
}
