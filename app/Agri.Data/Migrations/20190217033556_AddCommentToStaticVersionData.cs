using Microsoft.EntityFrameworkCore.Migrations;

namespace Agri.Data.Migrations
{
    public partial class AddCommentToStaticVersionData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "StaticDataVersions");

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "StaticDataVersions",
                type: "VARCHAR(4000)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comments",
                table: "StaticDataVersions");

            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "StaticDataVersions",
                nullable: true);
        }
    }
}
