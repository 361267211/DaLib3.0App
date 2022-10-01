using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.SceneManage.Database.Migrations.Migrations
{
    public partial class AddSceneAppPlate : Migration
    {
        private readonly string schema;
        public AddSceneAppPlate(string schema)
        {
            this.schema = schema;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: $"{schema}");

            migrationBuilder.DropColumn(
                name: "AppPlateIds",
                schema: $"{schema}",
                table: "SceneApp");

            migrationBuilder.DropColumn(
                name: "SortType",
                schema: $"{schema}",
                table: "SceneApp");

            migrationBuilder.DropColumn(
                name: "TopCount",
                schema: $"{schema}",
                table: "SceneApp");

            migrationBuilder.CreateTable(
                name: "SceneAppPlate",
                schema: $"{schema}",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SceneAppId = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    SceneId = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    AppPlateId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TopCount = table.Column<int>(type: "integer", nullable: false),
                    SortType = table.Column<int>(type: "integer", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SceneAppPlate", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SceneAppPlate",
                schema: $"{schema}");

            migrationBuilder.AddColumn<string>(
                name: "AppPlateIds",
                schema: $"{schema}",
                table: "SceneApp",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SortType",
                schema: $"{schema}",
                table: "SceneApp",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TopCount",
                schema: $"{schema}",
                table: "SceneApp",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
