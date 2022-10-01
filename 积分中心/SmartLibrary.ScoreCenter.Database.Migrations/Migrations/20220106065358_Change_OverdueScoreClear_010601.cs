using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SmartLibrary.ScoreCenter.Database.Migrations.Migrations
{
    public partial class Change_OverdueScoreClear_010601 : Migration
    {
        private readonly string schema;
        public Change_OverdueScoreClear_010601(string schema)
        {
            this.schema = schema;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OverdueScoreClear",
                schema: $"{schema}");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OverdueScoreClear",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    ExpireTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    OverScore = table.Column<int>(type: "integer", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UserKey = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OverdueScoreClear", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OverdueScoreClear_CreateTime",
                schema: $"{schema}",
                table: "OverdueScoreClear",
                column: "CreateTime");
        }
    }
}
