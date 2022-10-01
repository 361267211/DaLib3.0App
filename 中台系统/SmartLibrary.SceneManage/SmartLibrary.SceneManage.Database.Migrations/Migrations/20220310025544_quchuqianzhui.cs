using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.SceneManage.Database.Migrations.Migrations
{
    public partial class quchuqianzhui : Migration
    {
        // 自定义一个构造函数，并将Schema作为入参
        private readonly string schema;
        public quchuqianzhui(string schema)
        {
            this.schema = schema;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: schema);

            migrationBuilder.RenameTable(
                name: "ThemeColor",
                schema: $"dbo.{schema}",
                newName: "ThemeColor",
                newSchema: schema);

            migrationBuilder.RenameTable(
                name: "TerminalInstance",
                schema: $"dbo.{schema}",
                newName: "TerminalInstance",
                newSchema: schema);

            migrationBuilder.RenameTable(
                name: "Template",
                schema: $"dbo.{schema}",
                newName: "Template",
                newSchema: schema);

            migrationBuilder.RenameTable(
                name: "SysUserRole",
                schema: $"dbo.{schema}",
                newName: "SysUserRole",
                newSchema: schema);

            migrationBuilder.RenameTable(
                name: "SysRoleMenu",
                schema: $"dbo.{schema}",
                newName: "SysRoleMenu",
                newSchema: schema);

            migrationBuilder.RenameTable(
                name: "SysRole",
                schema: $"dbo.{schema}",
                newName: "SysRole",
                newSchema: schema);

            migrationBuilder.RenameTable(
                name: "SysMenuPermission",
                schema: $"dbo.{schema}",
                newName: "SysMenuPermission",
                newSchema: schema);

            migrationBuilder.RenameTable(
                name: "SysMenuCategory",
                schema: $"dbo.{schema}",
                newName: "SysMenuCategory",
                newSchema: schema);

            migrationBuilder.RenameTable(
                name: "SceneUser",
                schema: $"dbo.{schema}",
                newName: "SceneUser",
                newSchema: schema);

            migrationBuilder.RenameTable(
                name: "SceneScreen",
                schema: $"dbo.{schema}",
                newName: "SceneScreen",
                newSchema: schema);

            migrationBuilder.RenameTable(
                name: "SceneAppPlate",
                schema: $"dbo.{schema}",
                newName: "SceneAppPlate",
                newSchema: schema);

            migrationBuilder.RenameTable(
                name: "SceneApp",
                schema: $"dbo.{schema}",
                newName: "SceneApp",
                newSchema: schema);

            migrationBuilder.RenameTable(
                name: "Scene",
                schema: $"dbo.{schema}",
                newName: "Scene",
                newSchema: schema);

            migrationBuilder.RenameTable(
                name: "Layout",
                schema: $"dbo.{schema}",
                newName: "Layout",
                newSchema: schema);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "ThemeColor",
                schema: "public",
                newName: "ThemeColor",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "TerminalInstance",
                schema: "public",
                newName: "TerminalInstance",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Template",
                schema: "public",
                newName: "Template",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "SysUserRole",
                schema: "public",
                newName: "SysUserRole",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "SysRoleMenu",
                schema: "public",
                newName: "SysRoleMenu",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "SysRole",
                schema: "public",
                newName: "SysRole",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "SysMenuPermission",
                schema: "public",
                newName: "SysMenuPermission",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "SysMenuCategory",
                schema: "public",
                newName: "SysMenuCategory",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "SceneUser",
                schema: "public",
                newName: "SceneUser",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "SceneScreen",
                schema: "public",
                newName: "SceneScreen",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "SceneAppPlate",
                schema: "public",
                newName: "SceneAppPlate",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "SceneApp",
                schema: "public",
                newName: "SceneApp",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Scene",
                schema: "public",
                newName: "Scene",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Layout",
                schema: "public",
                newName: "Layout",
                newSchema: "dbo");
        }
    }
}
