using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.DataCenter.Database.Migrations.Migrations
{
    public partial class v230 : Migration
    {
        // 自定义一个构造函数，并将Schema作为入参
        private readonly string schema;
        public v230(string schema)
        {
            this.schema = schema;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AlbumName",
                schema: $"dbo.{schema}",
                table: "ResourceAlbum",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "Provider",
                schema: $"dbo.{schema}",
                table: "ResourceAlbum",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Provider",
                schema: $"dbo.{schema}",
                table: "ResourceAlbum");

            migrationBuilder.AlterColumn<int>(
                name: "AlbumName",
                schema: $"dbo.{schema}",
                table: "ResourceAlbum",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
