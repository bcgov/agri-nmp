using Microsoft.EntityFrameworkCore.Migrations;

namespace Agri.Data.Migrations
{
    public partial class AnimalSubTypeNewFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "SolidPerPoundPerAnimalPerDay",
                table: "AnimalSubType",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "SolidPerGalPerAnimalPerDay",
                table: "AnimalSubType",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "LiquidPerGalPerAnimalPerDay",
                table: "AnimalSubType",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AddColumn<decimal>(
                name: "MilkProduction",
                table: "AnimalSubType",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WashWater",
                table: "AnimalSubType",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MilkProduction",
                table: "AnimalSubType");

            migrationBuilder.DropColumn(
                name: "WashWater",
                table: "AnimalSubType");

            migrationBuilder.AlterColumn<decimal>(
                name: "SolidPerPoundPerAnimalPerDay",
                table: "AnimalSubType",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "SolidPerGalPerAnimalPerDay",
                table: "AnimalSubType",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "LiquidPerGalPerAnimalPerDay",
                table: "AnimalSubType",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);
        }
    }
}
