using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.SceneManage.Database.Migrations.Migrations
{
    public partial class addparentsceneappid : Migration
    {
        private readonly string schema;
        public addparentsceneappid(string schema)
        {
            this.schema = schema;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParentSceneAppId",
                schema: $"{schema}",
                table: "SceneApp",
                type: "character varying(48)",
                maxLength: 48,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentSceneAppId",
                schema: $"{schema}",
                table: "SceneApp");
        }
    }
}
