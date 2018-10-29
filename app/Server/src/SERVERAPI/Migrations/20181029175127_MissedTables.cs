using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SERVERAPI.Migrations
{
    public partial class MissedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DefaultSoilTests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Nitrogen = table.Column<decimal>(nullable: false),
                    Phosphorous = table.Column<decimal>(nullable: false),
                    Potassium = table.Column<decimal>(nullable: false),
                    pH = table.Column<decimal>(nullable: false),
                    ConvertedKelownaP = table.Column<int>(nullable: false),
                    ConvertedKelownaK = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefaultSoilTests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NutrientIcons",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    name = table.Column<string>(nullable: true),
                    definition = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NutrientIcons", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SelectCodeItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Cd = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectCodeItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SelectListItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectListItems", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DefaultSoilTests");

            migrationBuilder.DropTable(
                name: "NutrientIcons");

            migrationBuilder.DropTable(
                name: "SelectCodeItems");

            migrationBuilder.DropTable(
                name: "SelectListItems");
        }
    }
}
