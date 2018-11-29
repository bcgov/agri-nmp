using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Agri.Data.Migrations
{
    public partial class FinalRepoRelatedChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PreviousYearManureAplicationFrequency",
                table: "PrevYearManureApplicationNitrogenDefaults",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DefaultSoilTestMethodId",
                table: "DefaultSoilTests",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "NitrateCreditSampleDates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Location = table.Column<string>(nullable: true),
                    FromDateMonth = table.Column<string>(nullable: true),
                    ToDateMonth = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NitrateCreditSampleDates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SoilTestRanges",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UpperLimit = table.Column<int>(nullable: false),
                    Rating = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoilTestRanges", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NitrateCreditSampleDates");

            migrationBuilder.DropTable(
                name: "SoilTestRanges");

            migrationBuilder.DropColumn(
                name: "PreviousYearManureAplicationFrequency",
                table: "PrevYearManureApplicationNitrogenDefaults");

            migrationBuilder.DropColumn(
                name: "DefaultSoilTestMethodId",
                table: "DefaultSoilTests");
        }
    }
}
