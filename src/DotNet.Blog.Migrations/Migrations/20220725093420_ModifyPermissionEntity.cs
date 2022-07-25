using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNet.Blog.Migrations.Migrations
{
    public partial class ModifyPermissionEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "Role",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ParentCode",
                table: "Permission",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_ParentCode",
                table: "Permission",
                column: "ParentCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Permission_Permission_ParentCode",
                table: "Permission",
                column: "ParentCode",
                principalTable: "Permission",
                principalColumn: "Code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permission_Permission_ParentCode",
                table: "Permission");

            migrationBuilder.DropIndex(
                name: "IX_Permission_ParentCode",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "ParentCode",
                table: "Permission");
        }
    }
}
