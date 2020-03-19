using Microsoft.EntityFrameworkCore.Migrations;

namespace Agri.Data.Migrations
{
    public partial class AddDefaultSolidMoistureToManure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DefaultSolidMoisture",
                table: "Manures",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultSolidMoisture",
                table: "Manures");
        }
    }
}
