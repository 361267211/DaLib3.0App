using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.DatabaseTerrace.Database.Migrations.Migrations
{
    public partial class v105 : Migration
    {
        // 自定义一个构造函数，并将Schema作为入参
        private readonly string schema;
        public v105(string schema)
        {
            this.schema = schema;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DomainEscsName",
                schema: $"dbo.{schema}",
                table: "DomainEscsAttr");

            migrationBuilder.DropColumn(
                name: "ParentDomainEscs",
                schema: $"dbo.{schema}",
                table: "DomainEscsAttr");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DomainEscsName",
                schema: $"dbo.{schema}",
                table: "DomainEscsAttr",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParentDomainEscs",
                schema: $"dbo.{schema}",
                table: "DomainEscsAttr",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
