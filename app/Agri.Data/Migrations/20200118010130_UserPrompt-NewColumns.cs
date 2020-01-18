using Microsoft.EntityFrameworkCore.Migrations;

namespace Agri.Data.Migrations
{
    public partial class UserPromptNewColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserPromptPage",
                table: "UserPrompts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserJourney",
                table: "UserPrompts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserJourney",
                table: "UserPrompts");

            migrationBuilder.DropColumn(
                name: "UserPromptPage",
                table: "UserPrompts");
        }
    }
}