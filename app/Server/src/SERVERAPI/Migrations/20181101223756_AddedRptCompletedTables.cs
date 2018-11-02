using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SERVERAPI.Migrations
{
    public partial class AddedRptCompletedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RptCompletedFertilizerRequiredStdUnits",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    SolidUnitId = table.Column<int>(nullable: false),
                    LiquidUnitId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RptCompletedFertilizerRequiredStdUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RptCompletedManureRequiredStdUnits",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    SolidUnitId = table.Column<int>(nullable: false),
                    LiquidUnitId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RptCompletedManureRequiredStdUnits", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RptCompletedFertilizerRequiredStdUnits");

            migrationBuilder.DropTable(
                name: "RptCompletedManureRequiredStdUnits");
        }
    }
}
