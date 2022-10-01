using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SmartLibrary.ScoreCenter.Database.Migrations.Migrations
{
    public partial class Add_GoodsPrefer : Migration
    {
        private readonly string schema;
        public Add_GoodsPrefer(string schema)
        {
            this.schema = schema;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserGoodsPrefer",
                schema: $"{schema}",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserKey = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    GoodsID = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGoodsPrefer", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserGoodsPrefer_CreateTime",
                schema: $"{schema}",
                table: "UserGoodsPrefer",
                column: "CreateTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserGoodsPrefer",
                schema: $"{schema}");
        }
    }
}
