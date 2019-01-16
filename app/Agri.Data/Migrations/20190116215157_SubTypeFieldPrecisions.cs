using Microsoft.EntityFrameworkCore.Migrations;

namespace Agri.Data.Migrations
{
    public partial class SubTypeFieldPrecisions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "SolidPerPoundPerAnimalPerDay",
                table: "AnimalSubType",
                type: "decimal(16,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "SolidPerGalPerAnimalPerDay",
                table: "AnimalSubType",
                type: "decimal(16,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "LiquidPerGalPerAnimalPerDay",
                table: "AnimalSubType",
                type: "decimal(16,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "SolidPerPoundPerAnimalPerDay",
                table: "AnimalSubType",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "SolidPerGalPerAnimalPerDay",
                table: "AnimalSubType",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "LiquidPerGalPerAnimalPerDay",
                table: "AnimalSubType",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)",
                oldNullable: true);
        }
    }
}
