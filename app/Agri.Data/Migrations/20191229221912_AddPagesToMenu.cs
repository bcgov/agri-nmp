using Microsoft.EntityFrameworkCore.Migrations;

namespace Agri.Data.Migrations
{
    public partial class AddPagesToMenu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NextPage",
                table: "SubMenu",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Page",
                table: "SubMenu",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviousPage",
                table: "SubMenu",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NextPage",
                table: "MainMenus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Page",
                table: "MainMenus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviousPage",
                table: "MainMenus",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NextPage",
                table: "SubMenu");

            migrationBuilder.DropColumn(
                name: "Page",
                table: "SubMenu");

            migrationBuilder.DropColumn(
                name: "PreviousPage",
                table: "SubMenu");

            migrationBuilder.DropColumn(
                name: "NextPage",
                table: "MainMenus");

            migrationBuilder.DropColumn(
                name: "Page",
                table: "MainMenus");

            migrationBuilder.DropColumn(
                name: "PreviousPage",
                table: "MainMenus");
        }
    }
}
