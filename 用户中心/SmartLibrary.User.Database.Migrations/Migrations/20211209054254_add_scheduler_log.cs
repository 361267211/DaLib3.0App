using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SmartLibrary.User.Database.Migrations.Migrations
{
    public partial class add_scheduler_log : Migration
    {
        private readonly string schema;
        public add_scheduler_log(string schema)
        {
            this.schema = schema;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "SchedulerLogEntity",
            //    schema: "public",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        TenantId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
            //        JobId = table.Column<int>(type: "integer", nullable: false),
            //        JobName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
            //        Params = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
            //        BatchId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
            //        Context = table.Column<string>(type: "text", nullable: true),
            //        StartTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //        EndTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //        Status = table.Column<int>(type: "integer", nullable: false),
            //        CreateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //        NextFireTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_SchedulerLogEntity", x => x.Id);
            //    });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "SchedulerLogEntity",
            //    schema: "public");
        }
    }
}
