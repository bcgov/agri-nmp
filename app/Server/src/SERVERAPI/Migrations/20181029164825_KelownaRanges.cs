using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SERVERAPI.Migrations
{
    public partial class KelownaRanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StkKelownaRanges",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Range = table.Column<string>(nullable: true),
                    RangeLow = table.Column<int>(nullable: false),
                    RangeHigh = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StkKelownaRanges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StpKelownaRanges",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Range = table.Column<string>(nullable: true),
                    RangeLow = table.Column<int>(nullable: false),
                    RangeHigh = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StpKelownaRanges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "STKRecommend",
                columns: table => new
                {
                    STKKelownaRangeId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    SoilTestPotassiumRegionCd = table.Column<int>(nullable: false),
                    PotassiumCropGroupRegionCd = table.Column<int>(nullable: false),
                    K2O_Recommend_kgPeHa = table.Column<int>(nullable: false),
                    STKKelownaRangeId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STKRecommend", x => x.STKKelownaRangeId);
                    table.ForeignKey(
                        name: "FK_STKRecommend_StkKelownaRanges_STKKelownaRangeId1",
                        column: x => x.STKKelownaRangeId1,
                        principalTable: "StkKelownaRanges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "STPRecommend",
                columns: table => new
                {
                    STPKelownaRangeId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    SoilTestPhosphorousRegionCd = table.Column<int>(nullable: false),
                    PhosphorousCropGroupRegionCd = table.Column<int>(nullable: false),
                    P2O5_Recommend_KgPerHa = table.Column<int>(nullable: false),
                    StpKelownaRangeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STPRecommend", x => x.STPKelownaRangeId);
                    table.ForeignKey(
                        name: "FK_STPRecommend_StpKelownaRanges_StpKelownaRangeId",
                        column: x => x.StpKelownaRangeId,
                        principalTable: "StpKelownaRanges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_STKRecommend_STKKelownaRangeId1",
                table: "STKRecommend",
                column: "STKKelownaRangeId1");

            migrationBuilder.CreateIndex(
                name: "IX_STPRecommend_StpKelownaRangeId",
                table: "STPRecommend",
                column: "StpKelownaRangeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "STKRecommend");

            migrationBuilder.DropTable(
                name: "STPRecommend");

            migrationBuilder.DropTable(
                name: "StkKelownaRanges");

            migrationBuilder.DropTable(
                name: "StpKelownaRanges");
        }
    }
}
