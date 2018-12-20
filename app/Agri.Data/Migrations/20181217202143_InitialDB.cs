using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Agri.Data.Migrations
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
                    DryMatter = table.Column<int>(nullable: false),
                    Value = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmmoniaRetentions", x => new { x.SeasonApplicationId, x.DryMatter });
                    table.UniqueConstraint("AK_AmmoniaRetentions_DryMatter_SeasonApplicationId", x => new { x.DryMatter, x.SeasonApplicationId });
                });

            migrationBuilder.CreateTable(
                name: "Animals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    UseSortOrder = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BCSampleDateForNitrateCredit",
                columns: table => new
                {
                    CoastalFromDateMonth = table.Column<string>(nullable: false),
                    CoastalToDateMonth = table.Column<string>(nullable: true),
                    InteriorFromDateMonth = table.Column<string>(nullable: true),
                    InteriorToDateMonth = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BCSampleDateForNitrateCredit", x => x.CoastalFromDateMonth);
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
                    NitrogenProteinConversion = table.Column<decimal>(nullable: false),
                    UnitConversion = table.Column<decimal>(nullable: false),
                    DefaultSoilTestKelownaPhosphorous = table.Column<int>(nullable: false),
                    DefaultSoilTestKelownaPotassium = table.Column<int>(nullable: false),
                    KilogramPerHectareToPoundPerAcreConversion = table.Column<decimal>(nullable: false),
                    PotassiumAvailabilityFirstYear = table.Column<decimal>(nullable: false),
                    PotassiumAvailabilityLongTerm = table.Column<decimal>(nullable: false),
                    PotassiumKtoK2OConversion = table.Column<decimal>(nullable: false),
                    PhosphorousAvailabilityFirstYear = table.Column<decimal>(nullable: false),
                    PhosphorousAvailabilityLongTerm = table.Column<decimal>(nullable: false),
                    PhosphorousPtoP2O5Conversion = table.Column<decimal>(nullable: false),
                    PoundPerTonConversion = table.Column<decimal>(nullable: false),
                    PoundPer1000FtSquaredToPoundPerAcreConversion = table.Column<decimal>(nullable: false),
                    DefaultApplicationOfManureInPrevYears = table.Column<string>(nullable: true),
                    SoilTestPPMToPoundPerAcre = table.Column<decimal>(nullable: false)
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
                name: "DensityUnits",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    ConvFactor = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DensityUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DryMatters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DryMatters", x => x.Id);
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
                name: "Fertilizers",
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
                    table.PrimaryKey("PK_Fertilizers", x => x.Id);
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
                    ConversionToImperialGallonsPerAcre = table.Column<decimal>(nullable: false),
                    FarmRequiredNutrientsStdUnitsConversion = table.Column<decimal>(nullable: false),
                    FarmRequiredNutrientsStdUnitsAreaConversion = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FertilizerUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HarvestUnits",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HarvestUnits", x => x.Id);
                });

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
                name: "MainMenus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    Controller = table.Column<string>(nullable: true),
                    Action = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainMenus", x => x.Id);
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
                name: "NitrogenMineralizations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    LocationId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    FirstYearValue = table.Column<decimal>(nullable: false),
                    LongTermValue = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NitrogenMineralizations", x => new { x.Id, x.LocationId });
                });

            migrationBuilder.CreateTable(
                name: "NitrogenRecommendations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    RecommendationDesc = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NitrogenRecommendations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NutrientIcons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    Definition = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NutrientIcons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrevManureApplicationYears",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    FieldManureApplicationHistory = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrevManureApplicationYears", x => x.Id);
                    table.UniqueConstraint("AK_PrevManureApplicationYears_FieldManureApplicationHistory", x => x.FieldManureApplicationHistory);
                });

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

            migrationBuilder.CreateTable(
                name: "SeasonApplications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    Season = table.Column<string>(nullable: true),
                    ApplicationMethod = table.Column<string>(nullable: true),
                    DryMatterLessThan1Percent = table.Column<decimal>(nullable: false),
                    DryMatter1To5Percent = table.Column<decimal>(nullable: false),
                    DryMatter5To10Percent = table.Column<decimal>(nullable: false),
                    DryMatterGreaterThan10Percent = table.Column<decimal>(nullable: false),
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
                    ConvertToKelownaPHLessThan72 = table.Column<decimal>(nullable: false),
                    ConvertToKelownaPHGreaterThanEqual72 = table.Column<decimal>(nullable: false),
                    ConvertToKelownaK = table.Column<decimal>(nullable: false),
                    SortNum = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoilTestMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SoilTestPhosphorousKelownaRanges",
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
                    table.PrimaryKey("PK_SoilTestPhosphorousKelownaRanges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SoilTestPhosphorusRanges",
                columns: table => new
                {
                    UpperLimit = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Rating = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoilTestPhosphorusRanges", x => x.UpperLimit);
                });

            migrationBuilder.CreateTable(
                name: "SoilTestPotassiumKelownaRanges",
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
                    table.PrimaryKey("PK_SoilTestPotassiumKelownaRanges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SoilTestPotassiumRanges",
                columns: table => new
                {
                    UpperLimit = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Rating = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoilTestPotassiumRanges", x => x.UpperLimit);
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

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    NutrientContentUnits = table.Column<string>(nullable: true),
                    ConversionlbTon = table.Column<decimal>(nullable: false),
                    NutrientRateUnits = table.Column<string>(nullable: true),
                    CostUnits = table.Column<string>(nullable: true),
                    CostApplications = table.Column<decimal>(nullable: false),
                    DollarUnitArea = table.Column<string>(nullable: true),
                    ValueMaterialUnits = table.Column<string>(nullable: true),
                    ValueN = table.Column<decimal>(nullable: false),
                    ValueP2O5 = table.Column<decimal>(nullable: false),
                    ValueK2O = table.Column<decimal>(nullable: false),
                    FarmReqdNutrientsStdUnitsConversion = table.Column<decimal>(nullable: false),
                    FarmReqdNutrientsStdUnitsAreaConversion = table.Column<decimal>(nullable: false),
                    SolidLiquid = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserPrompts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPrompts", x => x.Id);
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
                    LiquidPerGalPerAnimalPerDay = table.Column<decimal>(nullable: true),
                    SolidPerGalPerAnimalPerDay = table.Column<decimal>(nullable: true),
                    SolidPerPoundPerAnimalPerDay = table.Column<decimal>(nullable: true),
                    SolidLiquidSeparationPercentage = table.Column<decimal>(nullable: false),
                    WashWater = table.Column<decimal>(nullable: false),
                    MilkProduction = table.Column<decimal>(nullable: false),
                    AnimalId = table.Column<int>(nullable: false),
                    SortOrder = table.Column<int>(nullable: false)
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
                        name: "FK_LiquidFertilizerDensities_DensityUnits_DensityUnitId",
                        column: x => x.DensityUnitId,
                        principalTable: "DensityUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LiquidFertilizerDensities_Fertilizers_FertilizerId",
                        column: x => x.FertilizerId,
                        principalTable: "Fertilizers",
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
                    SortNumber = table.Column<int>(nullable: false)
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
                name: "SubMenu",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    Controller = table.Column<string>(nullable: true),
                    Action = table.Column<string>(nullable: true),
                    MainMenuId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubMenu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubMenu_MainMenus_MainMenuId",
                        column: x => x.MainMenuId,
                        principalTable: "MainMenus",
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
                    Nitrate = table.Column<decimal>(nullable: false),
                    NMineralizationId1 = table.Column<int>(nullable: true),
                    NMineralizationLocationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Manures_DryMatters_DMId",
                        column: x => x.DMId,
                        principalTable: "DryMatters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Manures_NitrogenMineralizations_NMineralizationId1_NMineral~",
                        columns: x => new { x.NMineralizationId1, x.NMineralizationLocationId },
                        principalTable: "NitrogenMineralizations",
                        principalColumns: new[] { "Id", "LocationId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PrevYearManureApplicationNitrogenDefaults",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    FieldManureApplicationHistory = table.Column<int>(nullable: false),
                    DefaultNitrogenCredit = table.Column<int[]>(nullable: true),
                    PreviousYearManureAplicationFrequency = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrevYearManureApplicationNitrogenDefaults", x => x.Id);
                    table.UniqueConstraint("AK_PrevYearManureApplicationNitrogenDefaults_FieldManureApplic~", x => x.FieldManureApplicationHistory);
                    table.ForeignKey(
                        name: "FK_PrevYearManureApplicationNitrogenDefaults_PrevManureApplica~",
                        column: x => x.FieldManureApplicationHistory,
                        principalTable: "PrevManureApplicationYears",
                        principalColumn: "FieldManureApplicationHistory",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SoilTestPhosphorousRecommendation",
                columns: table => new
                {
                    SoilTestPhosphorousKelownaRangeId = table.Column<int>(nullable: false),
                    SoilTestPhosphorousRegionCode = table.Column<int>(nullable: false),
                    PhosphorousCropGroupRegionCode = table.Column<int>(nullable: false),
                    P2O5RecommendationKilogramPerHectare = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoilTestPhosphorousRecommendation", x => new { x.SoilTestPhosphorousKelownaRangeId, x.SoilTestPhosphorousRegionCode, x.PhosphorousCropGroupRegionCode });
                    table.UniqueConstraint("AK_SoilTestPhosphorousRecommendation_PhosphorousCropGroupRegio~", x => new { x.PhosphorousCropGroupRegionCode, x.SoilTestPhosphorousKelownaRangeId, x.SoilTestPhosphorousRegionCode });
                    table.ForeignKey(
                        name: "FK_SoilTestPhosphorousRecommendation_SoilTestPhosphorousKelown~",
                        column: x => x.SoilTestPhosphorousKelownaRangeId,
                        principalTable: "SoilTestPhosphorousKelownaRanges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SoilTestPotassiumRecommendation",
                columns: table => new
                {
                    SoilTestPotassiumKelownaRangeId = table.Column<int>(nullable: false),
                    SoilTestPotassiumRegionCode = table.Column<int>(nullable: false),
                    PotassiumCropGroupRegionCode = table.Column<int>(nullable: false),
                    K2ORecommendationKilogramPerHectare = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoilTestPotassiumRecommendation", x => new { x.SoilTestPotassiumKelownaRangeId, x.SoilTestPotassiumRegionCode, x.PotassiumCropGroupRegionCode });
                    table.UniqueConstraint("AK_SoilTestPotassiumRecommendation_PotassiumCropGroupRegionCod~", x => new { x.PotassiumCropGroupRegionCode, x.SoilTestPotassiumKelownaRangeId, x.SoilTestPotassiumRegionCode });
                    table.ForeignKey(
                        name: "FK_SoilTestPotassiumRecommendation_SoilTestPotassiumKelownaRan~",
                        column: x => x.SoilTestPotassiumKelownaRangeId,
                        principalTable: "SoilTestPotassiumKelownaRanges",
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
                    CropRemovalFactorNitrogen = table.Column<decimal>(nullable: true),
                    CropRemovalFactorP2O5 = table.Column<decimal>(nullable: true),
                    CropRemovalFactorK2O = table.Column<decimal>(nullable: true),
                    NitrogenRecommendationId = table.Column<decimal>(nullable: false),
                    NitrogenRecommendationPoundPerAcre = table.Column<decimal>(nullable: true),
                    NitrogenRecommendationUpperLimitPoundPerAcre = table.Column<decimal>(nullable: true),
                    PreviousCropCode = table.Column<int>(nullable: false),
                    SortNumber = table.Column<int>(nullable: false),
                    ManureApplicationHistory = table.Column<int>(nullable: false),
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
                    table.ForeignKey(
                        name: "FK_Crops_PrevYearManureApplicationNitrogenDefaults_ManureAppli~",
                        column: x => x.ManureApplicationHistory,
                        principalTable: "PrevYearManureApplicationNitrogenDefaults",
                        principalColumn: "FieldManureApplicationHistory",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CropSoilTestPhosphorousRegions",
                columns: table => new
                {
                    CropId = table.Column<int>(nullable: false),
                    SoilTestPhosphorousRegionCode = table.Column<int>(nullable: false),
                    PhosphorousCropGroupRegionCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CropSoilTestPhosphorousRegions", x => new { x.CropId, x.SoilTestPhosphorousRegionCode });
                    table.ForeignKey(
                        name: "FK_CropSoilTestPhosphorousRegions_Crops_CropId",
                        column: x => x.CropId,
                        principalTable: "Crops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CropSoilTestPotassiumRegions",
                columns: table => new
                {
                    CropId = table.Column<int>(nullable: false),
                    SoilTestPotassiumRegionCode = table.Column<int>(nullable: false),
                    PotassiumCropGroupRegionCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CropSoilTestPotassiumRegions", x => new { x.CropId, x.SoilTestPotassiumRegionCode });
                    table.ForeignKey(
                        name: "FK_CropSoilTestPotassiumRegions_Crops_CropId",
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
                    Amount = table.Column<decimal>(nullable: true)
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

            migrationBuilder.CreateTable(
                name: "PreviousCropType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    PreviousCropCode = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NitrogenCreditMetric = table.Column<int>(nullable: false),
                    NitrogenCreditImperial = table.Column<int>(nullable: false),
                    CropId = table.Column<int>(nullable: true),
                    CropTypeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreviousCropType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreviousCropType_Crops_CropId",
                        column: x => x.CropId,
                        principalTable: "Crops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PreviousCropType_CropTypes_CropTypeId",
                        column: x => x.CropTypeId,
                        principalTable: "CropTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_Crops_ManureApplicationHistory",
                table: "Crops",
                column: "ManureApplicationHistory");

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
                name: "IX_Manures_NMineralizationId1_NMineralizationLocationId",
                table: "Manures",
                columns: new[] { "NMineralizationId1", "NMineralizationLocationId" });

            migrationBuilder.CreateIndex(
                name: "IX_PreviousCropType_CropId",
                table: "PreviousCropType",
                column: "CropId");

            migrationBuilder.CreateIndex(
                name: "IX_PreviousCropType_CropTypeId",
                table: "PreviousCropType",
                column: "CropTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_LocationId",
                table: "Regions",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_SubMenu_MainMenuId",
                table: "SubMenu",
                column: "MainMenuId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AmmoniaRetentions");

            migrationBuilder.DropTable(
                name: "AnimalSubType");

            migrationBuilder.DropTable(
                name: "BCSampleDateForNitrateCredit");

            migrationBuilder.DropTable(
                name: "Browsers");

            migrationBuilder.DropTable(
                name: "ConversionFactors");

            migrationBuilder.DropTable(
                name: "CropSoilTestPhosphorousRegions");

            migrationBuilder.DropTable(
                name: "CropSoilTestPotassiumRegions");

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
                name: "HarvestUnits");

            migrationBuilder.DropTable(
                name: "LiquidFertilizerDensities");

            migrationBuilder.DropTable(
                name: "LiquidMaterialsConversionFactors");

            migrationBuilder.DropTable(
                name: "ManureImportedDefaults");

            migrationBuilder.DropTable(
                name: "Manures");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "NitrogenRecommendations");

            migrationBuilder.DropTable(
                name: "NutrientIcons");

            migrationBuilder.DropTable(
                name: "PreviousCropType");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "RptCompletedFertilizerRequiredStdUnits");

            migrationBuilder.DropTable(
                name: "RptCompletedManureRequiredStdUnits");

            migrationBuilder.DropTable(
                name: "SeasonApplications");

            migrationBuilder.DropTable(
                name: "SelectCodeItems");

            migrationBuilder.DropTable(
                name: "SelectListItems");

            migrationBuilder.DropTable(
                name: "SoilTestMethods");

            migrationBuilder.DropTable(
                name: "SoilTestPhosphorousRecommendation");

            migrationBuilder.DropTable(
                name: "SoilTestPhosphorusRanges");

            migrationBuilder.DropTable(
                name: "SoilTestPotassiumRanges");

            migrationBuilder.DropTable(
                name: "SoilTestPotassiumRecommendation");

            migrationBuilder.DropTable(
                name: "SolidMaterialsConversionFactors");

            migrationBuilder.DropTable(
                name: "SubMenu");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "UserPrompts");

            migrationBuilder.DropTable(
                name: "Versions");

            migrationBuilder.DropTable(
                name: "Yields");

            migrationBuilder.DropTable(
                name: "Animals");

            migrationBuilder.DropTable(
                name: "DensityUnits");

            migrationBuilder.DropTable(
                name: "Fertilizers");

            migrationBuilder.DropTable(
                name: "DryMatters");

            migrationBuilder.DropTable(
                name: "NitrogenMineralizations");

            migrationBuilder.DropTable(
                name: "Crops");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "SoilTestPhosphorousKelownaRanges");

            migrationBuilder.DropTable(
                name: "SoilTestPotassiumKelownaRanges");

            migrationBuilder.DropTable(
                name: "MainMenus");

            migrationBuilder.DropTable(
                name: "CropTypes");

            migrationBuilder.DropTable(
                name: "PrevYearManureApplicationNitrogenDefaults");

            migrationBuilder.DropTable(
                name: "PrevManureApplicationYears");
        }
    }
}
