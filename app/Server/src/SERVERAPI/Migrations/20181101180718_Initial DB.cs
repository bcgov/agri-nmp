using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SERVERAPI.Migrations
{
    public partial class InitialDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AmmoniaRetentions",
                columns: table => new
                {
                    SeasonApplicationId = table.Column<int>(nullable: false),
                    DM = table.Column<int>(nullable: false),
                    Value = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmmoniaRetentions", x => new { x.SeasonApplicationId, x.DM });
                    table.UniqueConstraint("AK_AmmoniaRetentions_DM_SeasonApplicationId", x => new { x.DM, x.SeasonApplicationId });
                });

            migrationBuilder.CreateTable(
                name: "Animals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Browsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    MinVersion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Browsers", x => x.Id);
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
                name: "CropTypes",
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
                    table.PrimaryKey("PK_CropTypes", x => x.Id);
                });

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
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
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
                name: "PrevManureApplicationYears",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrevManureApplicationYears", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrevYearManureApplDefaultNitrogens",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    PrevYearManureAppFrequency = table.Column<string>(nullable: true),
                    DefaultNitrogenCredit = table.Column<int[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrevYearManureApplDefaultNitrogens", x => x.Id);
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
                name: "STKKelownaRanges",
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
                    table.PrimaryKey("PK_STKKelownaRanges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "STPKelownaRanges",
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
                    table.PrimaryKey("PK_STPKelownaRanges", x => x.Id);
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
                name: "AnimalSubType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    LiquidPerGalPerAnimalPerDay = table.Column<decimal>(nullable: false),
                    SolidPerGalPerAnimalPerDay = table.Column<decimal>(nullable: false),
                    SolidPerPoundPerAnimalPerDay = table.Column<decimal>(nullable: false),
                    SolidLiquidSeparationPercentage = table.Column<decimal>(nullable: false),
                    AnimalId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalSubType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimalSubType_Animals_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        name: "FK_Crops_CropTypes_CropTypeId",
                        column: x => x.CropTypeId,
                        principalTable: "CropTypes",
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
                        name: "FK_PrevCropType_CropTypes_CropTypeId",
                        column: x => x.CropTypeId,
                        principalTable: "CropTypes",
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
                name: "Regions",
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
                    table.PrimaryKey("PK_Regions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Regions_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
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
                        name: "FK_STKRecommend_STKKelownaRanges_STKKelownaRangeId1",
                        column: x => x.STKKelownaRangeId1,
                        principalTable: "STKKelownaRanges",
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
                        name: "FK_STPRecommend_STPKelownaRanges_StpKelownaRangeId",
                        column: x => x.StpKelownaRangeId,
                        principalTable: "STPKelownaRanges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CropSTKRegionCds",
                columns: table => new
                {
                    CropId = table.Column<int>(nullable: false),
                    SoilTestPotassiumRegionCd = table.Column<int>(nullable: false),
                    PotassiumCropGroupRegionCd = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CropSTKRegionCds", x => new { x.CropId, x.PotassiumCropGroupRegionCd });
                    table.UniqueConstraint("AK_CropSTKRegionCds_CropId_SoilTestPotassiumRegionCd", x => new { x.CropId, x.SoilTestPotassiumRegionCd });
                    table.ForeignKey(
                        name: "FK_CropSTKRegionCds_Crops_CropId",
                        column: x => x.CropId,
                        principalTable: "Crops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CropSTPRegionCds",
                columns: table => new
                {
                    CropId = table.Column<int>(nullable: false),
                    SoilTestPhosphorousRegionCd = table.Column<int>(nullable: false),
                    PhosphorousCropGroupRegionCd = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CropSTPRegionCds", x => new { x.CropId, x.PhosphorousCropGroupRegionCd });
                    table.UniqueConstraint("AK_CropSTPRegionCds_CropId_SoilTestPhosphorousRegionCd", x => new { x.CropId, x.SoilTestPhosphorousRegionCd });
                    table.ForeignKey(
                        name: "FK_CropSTPRegionCds_Crops_CropId",
                        column: x => x.CropId,
                        principalTable: "Crops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CropYields",
                columns: table => new
                {
                    CropId = table.Column<int>(nullable: false),
                    LocationId = table.Column<int>(nullable: false),
                    Amt = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CropYields", x => new { x.CropId, x.LocationId });
                    table.ForeignKey(
                        name: "FK_CropYields_Crops_CropId",
                        column: x => x.CropId,
                        principalTable: "Crops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CropYields_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimalSubType_AnimalId",
                table: "AnimalSubType",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_Crops_CropTypeId",
                table: "Crops",
                column: "CropTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CropYields_LocationId",
                table: "CropYields",
                column: "LocationId");

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

            migrationBuilder.CreateIndex(
                name: "IX_PrevCropType_CropTypeId",
                table: "PrevCropType",
                column: "CropTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PrevCropType_CropTypeId1",
                table: "PrevCropType",
                column: "CropTypeId1");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_LocationId",
                table: "Regions",
                column: "LocationId");

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
                name: "AmmoniaRetentions");

            migrationBuilder.DropTable(
                name: "AnimalSubType");

            migrationBuilder.DropTable(
                name: "Browsers");

            migrationBuilder.DropTable(
                name: "ConversionFactors");

            migrationBuilder.DropTable(
                name: "CropSTKRegionCds");

            migrationBuilder.DropTable(
                name: "CropSTPRegionCds");

            migrationBuilder.DropTable(
                name: "CropYields");

            migrationBuilder.DropTable(
                name: "DefaultSoilTests");

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
                name: "NutrientIcons");

            migrationBuilder.DropTable(
                name: "PrevCropType");

            migrationBuilder.DropTable(
                name: "PrevManureApplicationYears");

            migrationBuilder.DropTable(
                name: "PrevYearManureApplDefaultNitrogens");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "SeasonApplications");

            migrationBuilder.DropTable(
                name: "SelectCodeItems");

            migrationBuilder.DropTable(
                name: "SelectListItems");

            migrationBuilder.DropTable(
                name: "SoilTestMethods");

            migrationBuilder.DropTable(
                name: "SoilTestPhosphorousRanges");

            migrationBuilder.DropTable(
                name: "SoilTestPotassiumRanges");

            migrationBuilder.DropTable(
                name: "STKRecommend");

            migrationBuilder.DropTable(
                name: "STPRecommend");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "Versions");

            migrationBuilder.DropTable(
                name: "Yields");

            migrationBuilder.DropTable(
                name: "Animals");

            migrationBuilder.DropTable(
                name: "Crops");

            migrationBuilder.DropTable(
                name: "DensityUnit");

            migrationBuilder.DropTable(
                name: "Fertilizer");

            migrationBuilder.DropTable(
                name: "DM");

            migrationBuilder.DropTable(
                name: "NMineralization");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "STKKelownaRanges");

            migrationBuilder.DropTable(
                name: "STPKelownaRanges");

            migrationBuilder.DropTable(
                name: "CropTypes");
        }
    }
}
