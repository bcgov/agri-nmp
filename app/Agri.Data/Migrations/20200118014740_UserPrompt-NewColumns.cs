using Microsoft.EntityFrameworkCore.Migrations;

namespace Agri.Data.Migrations
{
    public partial class UserPromptNewColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM public.""UserPrompts""; ");

            migrationBuilder.AddColumn<string>(
                name: "UserPromptPage",
                table: "UserPrompts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserJourney",
                table: "UserPrompts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPrompts_Name",
                table: "UserPrompts",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserPrompts_Name",
                table: "UserPrompts");

            migrationBuilder.DropColumn(
                name: "UserJourney",
                table: "UserPrompts");

            migrationBuilder.DropColumn(
                name: "UserPromptPage",
                table: "UserPrompts");
        }
    }
}