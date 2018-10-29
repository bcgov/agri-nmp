using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SERVERAPI.Migrations
{
    public partial class CropsAndChildren : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CropType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    CoverCrop = table.Column<bool>(nullable: false),
                    CrudeProteinRequired = table.Column<bool>(nullable: false),
                    CustomCrop = table.Column<bool>(nullable: false),
                    ModifyNitrogen = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CropType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Crops",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CropName = table.Column<string>(nullable: true),
                    CropTypeId = table.Column<int>(nullable: false),
                    YieldCd = table.Column<int>(nullable: false),
                    CropRemovalFactor_N = table.Column<decimal>(nullable: true),
                    CropRemovalFactorP2O5 = table.Column<decimal>(nullable: true),
                    CropRemovalFactorK2O = table.Column<decimal>(nullable: true),
                    N_RecommCd = table.Column<decimal>(nullable: false),
                    N_Recomm_lbPerAc = table.Column<decimal>(nullable: true),
                    N_High_lbPerAc = table.Column<decimal>(nullable: true),
                    PrevCropCd = table.Column<int>(nullable: false),
                    SortNum = table.Column<int>(nullable: false),
                    PrevYearManureAppl_VolCatCd = table.Column<int>(nullable: false),
                    HarvestBushelsPerTon = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Crops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Crops_CropType_CropTypeId",
                        column: x => x.CropTypeId,
                        principalTable: "CropType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrevCropType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    PrevCropCd = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    nCreditMetric = table.Column<int>(nullable: false),
                    nCreditImperial = table.Column<int>(nullable: false),
                    CropTypeId1 = table.Column<int>(nullable: true),
                    CropTypeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrevCropType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrevCropType_CropType_CropTypeId",
                        column: x => x.CropTypeId,
                        principalTable: "CropType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PrevCropType_PrevCropType_CropTypeId1",
                        column: x => x.CropTypeId1,
                        principalTable: "PrevCropType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Region",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    SoilTestPhosphorousRegionCd = table.Column<int>(nullable: false),
                    SoilTestPotassiumRegionCd = table.Column<int>(nullable: false),
                    LocationId = table.Column<int>(nullable: false),
                    SortNum = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Region", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Region_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CropSTKRegionCd",
                columns: table => new
                {
                    CropId = table.Column<int>(nullable: false),
                    SoilTestPotassiumRegionCd = table.Column<int>(nullable: false),
                    PotassiumCropGroupRegionCd = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CropSTKRegionCd", x => new { x.CropId, x.PotassiumCropGroupRegionCd });
                    table.UniqueConstraint("AK_CropSTKRegionCd_CropId_SoilTestPotassiumRegionCd", x => new { x.CropId, x.SoilTestPotassiumRegionCd });
                    table.ForeignKey(
                        name: "FK_CropSTKRegionCd_Crops_CropId",
                        column: x => x.CropId,
                        principalTable: "Crops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CropSTPRegionCd",
                columns: table => new
                {
                    CropId = table.Column<int>(nullable: false),
                    SoilTestPhosphorousRegionCd = table.Column<int>(nullable: false),
                    PhosphorousCropGroupRegionCd = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CropSTPRegionCd", x => new { x.CropId, x.PhosphorousCropGroupRegionCd });
                    table.UniqueConstraint("AK_CropSTPRegionCd_CropId_SoilTestPhosphorousRegionCd", x => new { x.CropId, x.SoilTestPhosphorousRegionCd });
                    table.ForeignKey(
                        name: "FK_CropSTPRegionCd_Crops_CropId",
                        column: x => x.CropId,
                        principalTable: "Crops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CropYield",
                columns: table => new
                {
                    CropId = table.Column<int>(nullable: false),
                    LocationId = table.Column<int>(nullable: false),
                    Amt = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CropYield", x => new { x.CropId, x.LocationId });
                    table.ForeignKey(
                        name: "FK_CropYield_Crops_CropId",
                        column: x => x.CropId,
                        principalTable: "Crops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CropYield_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Crops_CropTypeId",
                table: "Crops",
                column: "CropTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CropYield_LocationId",
                table: "CropYield",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_PrevCropType_CropTypeId",
                table: "PrevCropType",
                column: "CropTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PrevCropType_CropTypeId1",
                table: "PrevCropType",
                column: "CropTypeId1");

            migrationBuilder.CreateIndex(
                name: "IX_Region_LocationId",
                table: "Region",
                column: "LocationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CropSTKRegionCd");

            migrationBuilder.DropTable(
                name: "CropSTPRegionCd");

            migrationBuilder.DropTable(
                name: "CropYield");

            migrationBuilder.DropTable(
                name: "PrevCropType");

            migrationBuilder.DropTable(
                name: "Region");

            migrationBuilder.DropTable(
                name: "Crops");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "CropType");
        }
    }
}
