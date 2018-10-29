using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SERVERAPI.Migrations
{
    public partial class AllRemainingTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_STKRecommend_StkKelownaRanges_STKKelownaRangeId1",
                table: "STKRecommend");

            migrationBuilder.DropForeignKey(
                name: "FK_STPRecommend_StpKelownaRanges_StpKelownaRangeId",
                table: "STPRecommend");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StpKelownaRanges",
                table: "StpKelownaRanges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StkKelownaRanges",
                table: "StkKelownaRanges");

            migrationBuilder.RenameTable(
                name: "StpKelownaRanges",
                newName: "STPKelownaRanges");

            migrationBuilder.RenameTable(
                name: "StkKelownaRanges",
                newName: "STKKelownaRanges");

            migrationBuilder.AddPrimaryKey(
                name: "PK_STPKelownaRanges",
                table: "STPKelownaRanges",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_STKKelownaRanges",
                table: "STKKelownaRanges",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AmmoniaRetentions",
                columns: table => new
                {
                    SeasonApplicationId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DM = table.Column<int>(nullable: false),
                    Value = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmmoniaRetentions", x => x.SeasonApplicationId);
                });

            migrationBuilder.CreateTable(
                name: "ConversionFactors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    NProteinConversion = table.Column<decimal>(nullable: false),
                    UnitConversion = table.Column<decimal>(nullable: false),
                    DefaultSoilTestKelownaP = table.Column<int>(nullable: false),
                    DefaultSoilTestKelownaK = table.Column<int>(nullable: false),
                    KgPerHa_lbPerAc_Conversion = table.Column<decimal>(nullable: false),
                    PotassiumAvailabilityFirstYear = table.Column<decimal>(nullable: false),
                    PotassiumAvailabilityLongTerm = table.Column<decimal>(nullable: false),
                    PotassiumKtoK2Oconversion = table.Column<decimal>(nullable: false),
                    PhosphorousAvailabilityFirstYear = table.Column<decimal>(nullable: false),
                    PhosphorousAvailabilityLongTerm = table.Column<decimal>(nullable: false),
                    PhosphorousPtoP2O5KConversion = table.Column<decimal>(nullable: false),
                    lbPerTonConversion = table.Column<decimal>(nullable: false),
                    lbPer1000ftSquared_lbPerAc_Conversion = table.Column<decimal>(nullable: false),
                    DefaultApplicationOfManureInPrevYears = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversionFactors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DensityUnit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    ConvFactor = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DensityUnit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DM",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DM", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExternalLinks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalLinks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fertilizer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    DryLiquid = table.Column<string>(nullable: true),
                    Nitrogen = table.Column<decimal>(nullable: false),
                    Phosphorous = table.Column<decimal>(nullable: false),
                    Potassium = table.Column<decimal>(nullable: false),
                    SortNum = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fertilizer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FertilizerMethods",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FertilizerMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FertilizerTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    DryLiquid = table.Column<string>(nullable: true),
                    Custom = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FertilizerTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FertilizerUnits",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    DryLiquid = table.Column<string>(nullable: true),
                    ConvToImpGalPerAc = table.Column<decimal>(nullable: false),
                    FarmReqdNutrientsStdUnitsConversion = table.Column<decimal>(nullable: false),
                    FarmReqdNutrientsStdUnitsAreaConversion = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FertilizerUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Text = table.Column<string>(nullable: true),
                    DisplayMessage = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    BalanceType = table.Column<string>(nullable: true),
                    BalanceLow = table.Column<int>(nullable: false),
                    BalanceHigh = table.Column<int>(nullable: false),
                    SoilTestLow = table.Column<decimal>(nullable: false),
                    SoilTestHigh = table.Column<decimal>(nullable: false),
                    Balance1Low = table.Column<int>(nullable: false),
                    Balance1High = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NMineralization",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    Locationid = table.Column<int>(nullable: false),
                    FirstYearValue = table.Column<decimal>(nullable: false),
                    LongTermValue = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NMineralization", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SeasonApplications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    Season = table.Column<string>(nullable: true),
                    ApplicationMethod = table.Column<string>(nullable: true),
                    Moisture = table.Column<string>(nullable: true),
                    DM_lt1 = table.Column<decimal>(nullable: false),
                    DM_1_5 = table.Column<decimal>(nullable: false),
                    DM_5_10 = table.Column<decimal>(nullable: false),
                    DM_gt10 = table.Column<decimal>(nullable: false),
                    PoultrySolid = table.Column<string>(nullable: true),
                    Compost = table.Column<string>(nullable: true),
                    SortNum = table.Column<int>(nullable: false),
                    ManureType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeasonApplications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SoilTestMethods",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    ConvertToKelownaPlt72 = table.Column<decimal>(nullable: false),
                    ConvertToKelownaPge72 = table.Column<decimal>(nullable: false),
                    ConvertToKelownaK = table.Column<decimal>(nullable: false),
                    SortNum = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoilTestMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SoilTestPhosphorousRanges",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UpperLimit = table.Column<int>(nullable: false),
                    Rating = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoilTestPhosphorousRanges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SoilTestPotassiumRanges",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UpperLimit = table.Column<int>(nullable: false),
                    Rating = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoilTestPotassiumRanges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    NutrientContentUnits = table.Column<string>(nullable: true),
                    Conversion_lbTon = table.Column<decimal>(nullable: false),
                    NutrientRateUnits = table.Column<string>(nullable: true),
                    CostUnits = table.Column<string>(nullable: true),
                    CostApplications = table.Column<decimal>(nullable: false),
                    DollarUnitArea = table.Column<string>(nullable: true),
                    ValueMaterialUnits = table.Column<string>(nullable: true),
                    Value_N = table.Column<decimal>(nullable: false),
                    Value_P2O5 = table.Column<decimal>(nullable: false),
                    Value_K2O = table.Column<decimal>(nullable: false),
                    FarmReqdNutrientsStdUnitsConversion = table.Column<decimal>(nullable: false),
                    FarmReqdNutrientsStdUnitsAreaConversion = table.Column<decimal>(nullable: false),
                    SolidLiquid = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Versions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    StaticDataVersion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Versions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Yields",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    YieldDesc = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Yields", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LiquidFertilizerDensities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    FertilizerId = table.Column<int>(nullable: false),
                    DensityUnitId = table.Column<int>(nullable: false),
                    Value = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiquidFertilizerDensities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LiquidFertilizerDensities_DensityUnit_DensityUnitId",
                        column: x => x.DensityUnitId,
                        principalTable: "DensityUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LiquidFertilizerDensities_Fertilizer_FertilizerId",
                        column: x => x.FertilizerId,
                        principalTable: "Fertilizer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Manures",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    ManureClass = table.Column<string>(nullable: true),
                    SolidLiquid = table.Column<string>(nullable: true),
                    Moisture = table.Column<string>(nullable: true),
                    Nitrogen = table.Column<decimal>(nullable: false),
                    Ammonia = table.Column<int>(nullable: false),
                    Phosphorous = table.Column<decimal>(nullable: false),
                    Potassium = table.Column<decimal>(nullable: false),
                    DMId = table.Column<int>(nullable: false),
                    NMineralizationId = table.Column<int>(nullable: false),
                    SortNum = table.Column<int>(nullable: false),
                    CubicYardConversion = table.Column<decimal>(nullable: false),
                    Nitrate = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Manures_DM_DMId",
                        column: x => x.DMId,
                        principalTable: "DM",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Manures_NMineralization_NMineralizationId",
                        column: x => x.NMineralizationId,
                        principalTable: "NMineralization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LiquidFertilizerDensities_DensityUnitId",
                table: "LiquidFertilizerDensities",
                column: "DensityUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_LiquidFertilizerDensities_FertilizerId",
                table: "LiquidFertilizerDensities",
                column: "FertilizerId");

            migrationBuilder.CreateIndex(
                name: "IX_Manures_DMId",
                table: "Manures",
                column: "DMId");

            migrationBuilder.CreateIndex(
                name: "IX_Manures_NMineralizationId",
                table: "Manures",
                column: "NMineralizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_STKRecommend_STKKelownaRanges_STKKelownaRangeId1",
                table: "STKRecommend",
                column: "STKKelownaRangeId1",
                principalTable: "STKKelownaRanges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_STPRecommend_STPKelownaRanges_StpKelownaRangeId",
                table: "STPRecommend",
                column: "StpKelownaRangeId",
                principalTable: "STPKelownaRanges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_STKRecommend_STKKelownaRanges_STKKelownaRangeId1",
                table: "STKRecommend");

            migrationBuilder.DropForeignKey(
                name: "FK_STPRecommend_STPKelownaRanges_StpKelownaRangeId",
                table: "STPRecommend");

            migrationBuilder.DropTable(
                name: "AmmoniaRetentions");

            migrationBuilder.DropTable(
                name: "ConversionFactors");

            migrationBuilder.DropTable(
                name: "ExternalLinks");

            migrationBuilder.DropTable(
                name: "FertilizerMethods");

            migrationBuilder.DropTable(
                name: "FertilizerTypes");

            migrationBuilder.DropTable(
                name: "FertilizerUnits");

            migrationBuilder.DropTable(
                name: "LiquidFertilizerDensities");

            migrationBuilder.DropTable(
                name: "Manures");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "SeasonApplications");

            migrationBuilder.DropTable(
                name: "SoilTestMethods");

            migrationBuilder.DropTable(
                name: "SoilTestPhosphorousRanges");

            migrationBuilder.DropTable(
                name: "SoilTestPotassiumRanges");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "Versions");

            migrationBuilder.DropTable(
                name: "Yields");

            migrationBuilder.DropTable(
                name: "DensityUnit");

            migrationBuilder.DropTable(
                name: "Fertilizer");

            migrationBuilder.DropTable(
                name: "DM");

            migrationBuilder.DropTable(
                name: "NMineralization");

            migrationBuilder.DropPrimaryKey(
                name: "PK_STPKelownaRanges",
                table: "STPKelownaRanges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_STKKelownaRanges",
                table: "STKKelownaRanges");

            migrationBuilder.RenameTable(
                name: "STPKelownaRanges",
                newName: "StpKelownaRanges");

            migrationBuilder.RenameTable(
                name: "STKKelownaRanges",
                newName: "StkKelownaRanges");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StpKelownaRanges",
                table: "StpKelownaRanges",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StkKelownaRanges",
                table: "StkKelownaRanges",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_STKRecommend_StkKelownaRanges_STKKelownaRangeId1",
                table: "STKRecommend",
                column: "STKKelownaRangeId1",
                principalTable: "StkKelownaRanges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_STPRecommend_StpKelownaRanges_StpKelownaRangeId",
                table: "STPRecommend",
                column: "StpKelownaRangeId",
                principalTable: "StpKelownaRanges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
