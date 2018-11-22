using Microsoft.EntityFrameworkCore.Migrations;

namespace Agri.Data.Migrations
{
    public partial class MainMenuAddedExtraFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "MainMenus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Controller",
                table: "MainMenus",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Action",
                table: "MainMenus");

            migrationBuilder.DropColumn(
                name: "Controller",
                table: "MainMenus");
        }
    }
}
