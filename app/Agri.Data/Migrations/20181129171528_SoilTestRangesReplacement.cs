using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Agri.Data.Migrations
{
    public partial class SoilTestRangesReplacement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SoilTestRanges");

            migrationBuilder.CreateTable(
                name: "PhosphorusSoilTestRanges",
                columns: table => new
                {
                    UpperLimit = table.Column<int>(nullable: false),
                    Rating = table.Column<string>(nullable: true),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhosphorusSoilTestRanges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PotassiumSoilTestRanges",
                columns: table => new
                {
                    UpperLimit = table.Column<int>(nullable: false),
                    Rating = table.Column<string>(nullable: true),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PotassiumSoilTestRanges", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhosphorusSoilTestRanges");

            migrationBuilder.DropTable(
                name: "PotassiumSoilTestRanges");

            migrationBuilder.CreateTable(
                name: "SoilTestRanges",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Rating = table.Column<string>(nullable: true),
                    UpperLimit = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoilTestRanges", x => x.Id);
                });
        }
    }
}
