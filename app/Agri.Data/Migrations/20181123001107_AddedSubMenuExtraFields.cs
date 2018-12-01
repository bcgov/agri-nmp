using Microsoft.EntityFrameworkCore.Migrations;

namespace Agri.Data.Migrations
{
    public partial class AddedSubMenuExtraFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "SubMenu",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Controller",
                table: "SubMenu",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Action",
                table: "SubMenu");

            migrationBuilder.DropColumn(
                name: "Controller",
                table: "SubMenu");
        }
    }
}
