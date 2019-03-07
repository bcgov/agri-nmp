using Microsoft.EntityFrameworkCore.Migrations;

namespace Agri.Data.Migrations
{
    public partial class AddLowerLimitSoilTestRange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LowerLimit",
                table: "PotassiumSoilTestRanges",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LowerLimit",
                table: "PhosphorusSoilTestRanges",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LowerLimit",
                table: "PotassiumSoilTestRanges");

            migrationBuilder.DropColumn(
                name: "LowerLimit",
                table: "PhosphorusSoilTestRanges");
        }
    }
}
