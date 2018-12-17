using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Agri.Data.Migrations
{
    public partial class AddFieldsForAnimalAndSubtype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "AnimalSubType",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UseSortOrder",
                table: "Animals",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LiquidMaterialsConversionFactors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    InputUnit = table.Column<int>(nullable: false),
                    InputUnitName = table.Column<string>(nullable: true),
                    USGallonsOutput = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiquidMaterialsConversionFactors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ManureImportedDefaults",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DefaultSolidMoisture = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManureImportedDefaults", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SolidMaterialsConversionFactors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    InputUnit = table.Column<int>(nullable: false),
                    InputUnitName = table.Column<string>(nullable: true),
                    CubicYardsOutput = table.Column<string>(nullable: true),
                    CubicMetersOutput = table.Column<string>(nullable: true),
                    MetricTonsOutput = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolidMaterialsConversionFactors", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LiquidMaterialsConversionFactors");

            migrationBuilder.DropTable(
                name: "ManureImportedDefaults");

            migrationBuilder.DropTable(
                name: "SolidMaterialsConversionFactors");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "AnimalSubType");

            migrationBuilder.DropColumn(
                name: "UseSortOrder",
                table: "Animals");
        }
    }
}
