using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.Open.Database.Migrations.Migrations
{
    public partial class assembly_v100 : Migration
    {
        // 自定义一个构造函数，并将Schema作为入参
        private readonly string schema;
        public assembly_v100(string schema)
        {
            this.schema = schema;
        }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreaterId",
                table: "AssemblyBaseInfo");

            migrationBuilder.DropColumn(
                name: "MaintainerId",
                table: "AssemblyBaseInfo");

            migrationBuilder.AddColumn<string>(
                name: "CreaterUserKey",
                table: "AssemblyBaseInfo",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaintainerUserKeys",
                table: "AssemblyBaseInfo",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrginCreaterName",
                table: "AssemblyBaseInfo",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrginOwnerName",
                table: "AssemblyBaseInfo",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreaterUserKey",
                table: "AssemblyBaseInfo");

            migrationBuilder.DropColumn(
                name: "MaintainerUserKeys",
                table: "AssemblyBaseInfo");

            migrationBuilder.DropColumn(
                name: "OrginCreaterName",
                table: "AssemblyBaseInfo");

            migrationBuilder.DropColumn(
                name: "OrginOwnerName",
                table: "AssemblyBaseInfo");

            migrationBuilder.AddColumn<Guid>(
                name: "CreaterId",
                table: "AssemblyBaseInfo",
                type: "uuid",
                maxLength: 40,
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "MaintainerId",
                table: "AssemblyBaseInfo",
                type: "uuid",
                maxLength: 40,
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
