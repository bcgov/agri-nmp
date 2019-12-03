using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Agri.Data.Migrations
{
    public partial class InitialDB_Release3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppliedMigrationSeedData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    JsonFilename = table.Column<string>(nullable: true),
                    ReasonReference = table.Column<string>(nullable: true),
                    AppliedDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppliedMigrationSeedData", x => x.Id);
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
                    Action = table.Column<string>(nullable: true),
                    SortNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainMenus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ManageVersionUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManageVersionUsers", x => x.Id);
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
                name: "StaticDataVersions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "NOW()"),
                    CreatedBy = table.Column<string>(nullable: true),
                    Comments = table.Column<string>(type: "VARCHAR(4000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticDataVersions", x => x.Id);
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
                name: "SubMenu",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    Controller = table.Column<string>(nullable: true),
                    Action = table.Column<string>(nullable: true),
                    SortNumber = table.Column<int>(nullable: false),
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
                name: "AmmoniaRetentions",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    SeasonApplicationId = table.Column<int>(nullable: false),
                    DryMatter = table.Column<int>(nullable: false),
                    Value = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmmoniaRetentions", x => new { x.SeasonApplicationId, x.DryMatter, x.StaticDataVersionId });
                    table.UniqueConstraint("AK_AmmoniaRetentions_DryMatter_SeasonApplicationId_StaticDataV~", x => new { x.DryMatter, x.SeasonApplicationId, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_AmmoniaRetentions_StaticDataVersions_StaticDataVersionId",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Animals",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    UseSortOrder = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animals", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_Animals_StaticDataVersions_StaticDataVersionId",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BCSampleDateForNitrateCredit",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    CoastalFromDateMonth = table.Column<string>(nullable: false),
                    CoastalToDateMonth = table.Column<string>(nullable: true),
                    InteriorFromDateMonth = table.Column<string>(nullable: true),
                    InteriorToDateMonth = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BCSampleDateForNitrateCredit", x => new { x.CoastalFromDateMonth, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_BCSampleDateForNitrateCredit_StaticDataVersions_StaticDataV~",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConversionFactors",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
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
                    table.PrimaryKey("PK_ConversionFactors", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_ConversionFactors_StaticDataVersions_StaticDataVersionId",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CropTypes",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CoverCrop = table.Column<bool>(nullable: false),
                    CrudeProteinRequired = table.Column<bool>(nullable: false),
                    CustomCrop = table.Column<bool>(nullable: false),
                    ModifyNitrogen = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CropTypes", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_CropTypes_StaticDataVersions_StaticDataVersionId",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DefaultSoilTests",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
                    Nitrogen = table.Column<decimal>(nullable: false),
                    Phosphorous = table.Column<decimal>(nullable: false),
                    Potassium = table.Column<decimal>(nullable: false),
                    pH = table.Column<decimal>(nullable: false),
                    ConvertedKelownaP = table.Column<int>(nullable: false),
                    ConvertedKelownaK = table.Column<int>(nullable: false),
                    DefaultSoilTestMethodId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefaultSoilTests", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_DefaultSoilTests_StaticDataVersions_StaticDataVersionId",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DensityUnits",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ConvFactor = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DensityUnits", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_DensityUnits_StaticDataVersions_StaticDataVersionId",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DryMatters",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DryMatters", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_DryMatters_StaticDataVersions_StaticDataVersionId",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FertilizerMethods",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FertilizerMethods", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_FertilizerMethods_StaticDataVersions_StaticDataVersionId",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fertilizers",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    DryLiquid = table.Column<string>(nullable: true),
                    Nitrogen = table.Column<decimal>(nullable: false),
                    Phosphorous = table.Column<decimal>(nullable: false),
                    Potassium = table.Column<decimal>(nullable: false),
                    SortNum = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fertilizers", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_Fertilizers_StaticDataVersions_StaticDataVersionId",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FertilizerTypes",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    DryLiquid = table.Column<string>(nullable: true),
                    Custom = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FertilizerTypes", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_FertilizerTypes_StaticDataVersions_StaticDataVersionId",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FertilizerUnits",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    DryLiquid = table.Column<string>(nullable: true),
                    ConversionToImperialGallonsPerAcre = table.Column<decimal>(nullable: false),
                    FarmRequiredNutrientsStdUnitsConversion = table.Column<decimal>(nullable: false),
                    FarmRequiredNutrientsStdUnitsAreaConversion = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FertilizerUnits", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_FertilizerUnits_StaticDataVersions_StaticDataVersionId",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HarvestUnits",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HarvestUnits", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_HarvestUnits_StaticDataVersions_StaticDataVersionId",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LiquidMaterialApplicationUsGallonsPerAcreRateConversions",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
                    ApplicationRateUnit = table.Column<int>(nullable: false),
                    ApplicationRateUnitName = table.Column<string>(nullable: true),
                    USGallonsPerAcreConversion = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiquidMaterialApplicationUsGallonsPerAcreRateConversions", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_LiquidMaterialApplicationUsGallonsPerAcreRateConversions_St~",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LiquidMaterialsConversionFactors",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
                    InputUnit = table.Column<int>(nullable: false),
                    InputUnitName = table.Column<string>(nullable: true),
                    USGallonsOutput = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiquidMaterialsConversionFactors", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_LiquidMaterialsConversionFactors_StaticDataVersions_StaticD~",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LiquidSolidSeparationDefaults",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
                    PercentOfLiquidSeparation = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiquidSolidSeparationDefaults", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_LiquidSolidSeparationDefaults_StaticDataVersions_StaticData~",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ManureImportedDefaults",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
                    DefaultSolidMoisture = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManureImportedDefaults", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_ManureImportedDefaults_StaticDataVersions_StaticDataVersion~",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
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
                    table.PrimaryKey("PK_Messages", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_Messages_StaticDataVersions_StaticDataVersionId",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NitrateCreditSampleDates",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Location = table.Column<string>(nullable: true),
                    FromDateMonth = table.Column<string>(nullable: true),
                    ToDateMonth = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NitrateCreditSampleDates", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_NitrateCreditSampleDates_StaticDataVersions_StaticDataVersi~",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NitrogenMineralizations",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
                    LocationId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    FirstYearValue = table.Column<decimal>(nullable: false),
                    LongTermValue = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NitrogenMineralizations", x => new { x.Id, x.LocationId, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_NitrogenMineralizations_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NitrogenMineralizations_StaticDataVersions_StaticDataVersio~",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NitrogenRecommendations",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
                    RecommendationDesc = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NitrogenRecommendations", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_NitrogenRecommendations_StaticDataVersions_StaticDataVersio~",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhosphorusSoilTestRanges",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    LowerLimit = table.Column<int>(nullable: false),
                    UpperLimit = table.Column<int>(nullable: false),
                    Rating = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhosphorusSoilTestRanges", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_PhosphorusSoilTestRanges_StaticDataVersions_StaticDataVersi~",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PotassiumSoilTestRanges",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    LowerLimit = table.Column<int>(nullable: false),
                    UpperLimit = table.Column<int>(nullable: false),
                    Rating = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PotassiumSoilTestRanges", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_PotassiumSoilTestRanges_StaticDataVersions_StaticDataVersio~",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrevManureApplicationYears",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    FieldManureApplicationHistory = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrevManureApplicationYears", x => new { x.Id, x.StaticDataVersionId });
                    table.UniqueConstraint("AK_PrevManureApplicationYears_FieldManureApplicationHistory_St~", x => new { x.FieldManureApplicationHistory, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_PrevManureApplicationYears_StaticDataVersions_StaticDataVer~",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    SoilTestPhosphorousRegionCd = table.Column<int>(nullable: false),
                    SoilTestPotassiumRegionCd = table.Column<int>(nullable: false),
                    LocationId = table.Column<int>(nullable: false),
                    SortNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_Regions_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Regions_StaticDataVersions_StaticDataVersionId",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RptCompletedFertilizerRequiredStdUnits",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
                    SolidUnitId = table.Column<int>(nullable: false),
                    LiquidUnitId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RptCompletedFertilizerRequiredStdUnits", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_RptCompletedFertilizerRequiredStdUnits_StaticDataVersions_S~",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RptCompletedManureRequiredStdUnits",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
                    SolidUnitId = table.Column<int>(nullable: false),
                    LiquidUnitId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RptCompletedManureRequiredStdUnits", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_RptCompletedManureRequiredStdUnits_StaticDataVersions_Stati~",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeasonApplications",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
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
                    table.PrimaryKey("PK_SeasonApplications", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_SeasonApplications_StaticDataVersions_StaticDataVersionId",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SoilTestMethods",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ConvertToKelownaPHLessThan72 = table.Column<decimal>(nullable: false),
                    ConvertToKelownaPHGreaterThanEqual72 = table.Column<decimal>(nullable: false),
                    ConvertToKelownaK = table.Column<decimal>(nullable: false),
                    SortNum = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoilTestMethods", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_SoilTestMethods_StaticDataVersions_StaticDataVersionId",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SoilTestPhosphorousKelownaRanges",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
                    Range = table.Column<string>(nullable: true),
                    RangeLow = table.Column<int>(nullable: false),
                    RangeHigh = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoilTestPhosphorousKelownaRanges", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_SoilTestPhosphorousKelownaRanges_StaticDataVersions_StaticD~",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SoilTestPhosphorusRanges",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    UpperLimit = table.Column<int>(nullable: false),
                    Rating = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoilTestPhosphorusRanges", x => new { x.UpperLimit, x.StaticDataVersionId });
                    table.UniqueConstraint("AK_SoilTestPhosphorusRanges_StaticDataVersionId_UpperLimit", x => new { x.StaticDataVersionId, x.UpperLimit });
                    table.ForeignKey(
                        name: "FK_SoilTestPhosphorusRanges_StaticDataVersions_StaticDataVersi~",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SoilTestPotassiumKelownaRanges",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
                    Range = table.Column<string>(nullable: true),
                    RangeLow = table.Column<int>(nullable: false),
                    RangeHigh = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoilTestPotassiumKelownaRanges", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_SoilTestPotassiumKelownaRanges_StaticDataVersions_StaticDat~",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SoilTestPotassiumRanges",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    UpperLimit = table.Column<int>(nullable: false),
                    Rating = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoilTestPotassiumRanges", x => new { x.UpperLimit, x.StaticDataVersionId });
                    table.UniqueConstraint("AK_SoilTestPotassiumRanges_StaticDataVersionId_UpperLimit", x => new { x.StaticDataVersionId, x.UpperLimit });
                    table.ForeignKey(
                        name: "FK_SoilTestPotassiumRanges_StaticDataVersions_StaticDataVersio~",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SolidMaterialApplicationTonPerAcreRateConversions",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
                    ApplicationRateUnit = table.Column<int>(nullable: false),
                    ApplicationRateUnitName = table.Column<string>(nullable: true),
                    TonsPerAcreConversion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolidMaterialApplicationTonPerAcreRateConversions", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_SolidMaterialApplicationTonPerAcreRateConversions_StaticDat~",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SolidMaterialsConversionFactors",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
                    InputUnit = table.Column<int>(nullable: false),
                    InputUnitName = table.Column<string>(nullable: true),
                    CubicYardsOutput = table.Column<string>(nullable: true),
                    CubicMetersOutput = table.Column<string>(nullable: true),
                    MetricTonsOutput = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolidMaterialsConversionFactors", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_SolidMaterialsConversionFactors_StaticDataVersions_StaticDa~",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
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
                    table.PrimaryKey("PK_Units", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_Units_StaticDataVersions_StaticDataVersionId",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Yields",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
                    YieldDesc = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Yields", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_Yields_StaticDataVersions_StaticDataVersionId",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimalSubType",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    LiquidPerGalPerAnimalPerDay = table.Column<decimal>(type: "decimal(16,4)", nullable: true),
                    SolidPerGalPerAnimalPerDay = table.Column<decimal>(type: "decimal(16,4)", nullable: true),
                    SolidPerPoundPerAnimalPerDay = table.Column<decimal>(type: "decimal(16,4)", nullable: true),
                    SolidLiquidSeparationPercentage = table.Column<decimal>(nullable: false),
                    WashWater = table.Column<decimal>(nullable: false),
                    MilkProduction = table.Column<decimal>(nullable: false),
                    AnimalId = table.Column<int>(nullable: false),
                    SortOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalSubType", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_AnimalSubType_StaticDataVersions_StaticDataVersionId",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimalSubType_Animals_AnimalId_StaticDataVersionId",
                        columns: x => new { x.AnimalId, x.StaticDataVersionId },
                        principalTable: "Animals",
                        principalColumns: new[] { "Id", "StaticDataVersionId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Breed",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
                    BreedName = table.Column<string>(nullable: true),
                    AnimalId = table.Column<int>(nullable: false),
                    BreedManureFactor = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Breed", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_Breed_StaticDataVersions_StaticDataVersionId",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Breed_Animals_AnimalId_StaticDataVersionId",
                        columns: x => new { x.AnimalId, x.StaticDataVersionId },
                        principalTable: "Animals",
                        principalColumns: new[] { "Id", "StaticDataVersionId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Manures",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ManureClass = table.Column<string>(nullable: true),
                    SolidLiquid = table.Column<string>(nullable: true),
                    Moisture = table.Column<string>(nullable: true),
                    Nitrogen = table.Column<decimal>(nullable: false),
                    Ammonia = table.Column<int>(nullable: false),
                    Phosphorous = table.Column<decimal>(nullable: false),
                    Potassium = table.Column<decimal>(nullable: false),
                    DryMatterId = table.Column<int>(nullable: false),
                    NMineralizationId = table.Column<int>(nullable: false),
                    SortNum = table.Column<int>(nullable: false),
                    CubicYardConversion = table.Column<decimal>(nullable: false),
                    Nitrate = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manures", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_Manures_StaticDataVersions_StaticDataVersionId",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Manures_DryMatters_DryMatterId_StaticDataVersionId",
                        columns: x => new { x.DryMatterId, x.StaticDataVersionId },
                        principalTable: "DryMatters",
                        principalColumns: new[] { "Id", "StaticDataVersionId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LiquidFertilizerDensities",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
                    FertilizerId = table.Column<int>(nullable: false),
                    DensityUnitId = table.Column<int>(nullable: false),
                    Value = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiquidFertilizerDensities", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_LiquidFertilizerDensities_StaticDataVersions_StaticDataVers~",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LiquidFertilizerDensities_DensityUnits_DensityUnitId_Static~",
                        columns: x => new { x.DensityUnitId, x.StaticDataVersionId },
                        principalTable: "DensityUnits",
                        principalColumns: new[] { "Id", "StaticDataVersionId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LiquidFertilizerDensities_Fertilizers_FertilizerId_StaticDa~",
                        columns: x => new { x.FertilizerId, x.StaticDataVersionId },
                        principalTable: "Fertilizers",
                        principalColumns: new[] { "Id", "StaticDataVersionId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrevYearManureApplicationNitrogenDefaults",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    FieldManureApplicationHistory = table.Column<int>(nullable: false),
                    DefaultNitrogenCredit = table.Column<int[]>(nullable: true),
                    PreviousYearManureAplicationFrequency = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrevYearManureApplicationNitrogenDefaults", x => new { x.Id, x.StaticDataVersionId });
                    table.UniqueConstraint("AK_PrevYearManureApplicationNitrogenDefaults_FieldManureApplic~", x => new { x.FieldManureApplicationHistory, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_PrevYearManureApplicationNitrogenDefaults_StaticDataVersion~",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrevYearManureApplicationNitrogenDefaults_PrevManureApplica~",
                        columns: x => new { x.FieldManureApplicationHistory, x.StaticDataVersionId },
                        principalTable: "PrevManureApplicationYears",
                        principalColumns: new[] { "FieldManureApplicationHistory", "StaticDataVersionId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubRegion",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    AnnualPrecipitation = table.Column<int>(nullable: false),
                    AnnualPrecipitationOctToMar = table.Column<int>(nullable: false),
                    RegionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubRegion", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_SubRegion_StaticDataVersions_StaticDataVersionId",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubRegion_Regions_RegionId_StaticDataVersionId",
                        columns: x => new { x.RegionId, x.StaticDataVersionId },
                        principalTable: "Regions",
                        principalColumns: new[] { "Id", "StaticDataVersionId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SoilTestPhosphorousRecommendation",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    SoilTestPhosphorousKelownaRangeId = table.Column<int>(nullable: false),
                    SoilTestPhosphorousRegionCode = table.Column<int>(nullable: false),
                    PhosphorousCropGroupRegionCode = table.Column<int>(nullable: false),
                    P2O5RecommendationKilogramPerHectare = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoilTestPhosphorousRecommendation", x => new { x.SoilTestPhosphorousKelownaRangeId, x.SoilTestPhosphorousRegionCode, x.PhosphorousCropGroupRegionCode, x.StaticDataVersionId });
                    table.UniqueConstraint("AK_SoilTestPhosphorousRecommendation_PhosphorousCropGroupRegio~", x => new { x.PhosphorousCropGroupRegionCode, x.SoilTestPhosphorousKelownaRangeId, x.SoilTestPhosphorousRegionCode, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_SoilTestPhosphorousRecommendation_StaticDataVersions_Static~",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SoilTestPhosphorousRecommendation_SoilTestPhosphorousKelown~",
                        columns: x => new { x.SoilTestPhosphorousKelownaRangeId, x.StaticDataVersionId },
                        principalTable: "SoilTestPhosphorousKelownaRanges",
                        principalColumns: new[] { "Id", "StaticDataVersionId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SoilTestPotassiumRecommendation",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    SoilTestPotassiumKelownaRangeId = table.Column<int>(nullable: false),
                    SoilTestPotassiumRegionCode = table.Column<int>(nullable: false),
                    PotassiumCropGroupRegionCode = table.Column<int>(nullable: false),
                    K2ORecommendationKilogramPerHectare = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoilTestPotassiumRecommendation", x => new { x.SoilTestPotassiumKelownaRangeId, x.SoilTestPotassiumRegionCode, x.PotassiumCropGroupRegionCode, x.StaticDataVersionId });
                    table.UniqueConstraint("AK_SoilTestPotassiumRecommendation_PotassiumCropGroupRegionCod~", x => new { x.PotassiumCropGroupRegionCode, x.SoilTestPotassiumKelownaRangeId, x.SoilTestPotassiumRegionCode, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_SoilTestPotassiumRecommendation_StaticDataVersions_StaticDa~",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SoilTestPotassiumRecommendation_SoilTestPotassiumKelownaRan~",
                        columns: x => new { x.SoilTestPotassiumKelownaRangeId, x.StaticDataVersionId },
                        principalTable: "SoilTestPotassiumKelownaRanges",
                        principalColumns: new[] { "Id", "StaticDataVersionId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Crops",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
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
                    table.PrimaryKey("PK_Crops", x => new { x.Id, x.StaticDataVersionId });
                    table.UniqueConstraint("AK_Crops_Id_PreviousCropCode_StaticDataVersionId", x => new { x.Id, x.PreviousCropCode, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_Crops_StaticDataVersions_StaticDataVersionId",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Crops_CropTypes_CropTypeId_StaticDataVersionId",
                        columns: x => new { x.CropTypeId, x.StaticDataVersionId },
                        principalTable: "CropTypes",
                        principalColumns: new[] { "Id", "StaticDataVersionId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Crops_PrevYearManureApplicationNitrogenDefaults_ManureAppli~",
                        columns: x => new { x.ManureApplicationHistory, x.StaticDataVersionId },
                        principalTable: "PrevYearManureApplicationNitrogenDefaults",
                        principalColumns: new[] { "FieldManureApplicationHistory", "StaticDataVersionId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CropSoilTestPhosphorousRegions",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    CropId = table.Column<int>(nullable: false),
                    SoilTestPhosphorousRegionCode = table.Column<int>(nullable: false),
                    PhosphorousCropGroupRegionCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CropSoilTestPhosphorousRegions", x => new { x.CropId, x.SoilTestPhosphorousRegionCode, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_CropSoilTestPhosphorousRegions_StaticDataVersions_StaticDat~",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CropSoilTestPhosphorousRegions_Crops_CropId_StaticDataVersi~",
                        columns: x => new { x.CropId, x.StaticDataVersionId },
                        principalTable: "Crops",
                        principalColumns: new[] { "Id", "StaticDataVersionId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CropSoilTestPotassiumRegions",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    CropId = table.Column<int>(nullable: false),
                    SoilTestPotassiumRegionCode = table.Column<int>(nullable: false),
                    PotassiumCropGroupRegionCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CropSoilTestPotassiumRegions", x => new { x.CropId, x.SoilTestPotassiumRegionCode, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_CropSoilTestPotassiumRegions_StaticDataVersions_StaticDataV~",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CropSoilTestPotassiumRegions_Crops_CropId_StaticDataVersion~",
                        columns: x => new { x.CropId, x.StaticDataVersionId },
                        principalTable: "Crops",
                        principalColumns: new[] { "Id", "StaticDataVersionId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CropYields",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    CropId = table.Column<int>(nullable: false),
                    LocationId = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CropYields", x => new { x.CropId, x.LocationId, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_CropYields_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CropYields_StaticDataVersions_StaticDataVersionId",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CropYields_Crops_CropId_StaticDataVersionId",
                        columns: x => new { x.CropId, x.StaticDataVersionId },
                        principalTable: "Crops",
                        principalColumns: new[] { "Id", "StaticDataVersionId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PreviousCropType",
                columns: table => new
                {
                    StaticDataVersionId = table.Column<int>(nullable: false, defaultValue: 1),
                    Id = table.Column<int>(nullable: false),
                    PreviousCropCode = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NitrogenCreditMetric = table.Column<int>(nullable: false),
                    NitrogenCreditImperial = table.Column<int>(nullable: false),
                    CropId = table.Column<int>(nullable: false),
                    CropTypeId = table.Column<int>(nullable: true),
                    CropTypeStaticDataVersionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreviousCropType", x => new { x.Id, x.StaticDataVersionId });
                    table.ForeignKey(
                        name: "FK_PreviousCropType_StaticDataVersions_StaticDataVersionId",
                        column: x => x.StaticDataVersionId,
                        principalTable: "StaticDataVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreviousCropType_CropTypes_CropTypeId_CropTypeStaticDataVer~",
                        columns: x => new { x.CropTypeId, x.CropTypeStaticDataVersionId },
                        principalTable: "CropTypes",
                        principalColumns: new[] { "Id", "StaticDataVersionId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PreviousCropType_Crops_CropId_PreviousCropCode_StaticDataVe~",
                        columns: x => new { x.CropId, x.PreviousCropCode, x.StaticDataVersionId },
                        principalTable: "Crops",
                        principalColumns: new[] { "Id", "PreviousCropCode", "StaticDataVersionId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AmmoniaRetentions_StaticDataVersionId",
                table: "AmmoniaRetentions",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Animals_StaticDataVersionId",
                table: "Animals",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalSubType_StaticDataVersionId",
                table: "AnimalSubType",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalSubType_AnimalId_StaticDataVersionId",
                table: "AnimalSubType",
                columns: new[] { "AnimalId", "StaticDataVersionId" });

            migrationBuilder.CreateIndex(
                name: "IX_BCSampleDateForNitrateCredit_StaticDataVersionId",
                table: "BCSampleDateForNitrateCredit",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Breed_StaticDataVersionId",
                table: "Breed",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Breed_AnimalId_StaticDataVersionId",
                table: "Breed",
                columns: new[] { "AnimalId", "StaticDataVersionId" });

            migrationBuilder.CreateIndex(
                name: "IX_ConversionFactors_StaticDataVersionId",
                table: "ConversionFactors",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Crops_StaticDataVersionId",
                table: "Crops",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Crops_CropTypeId_StaticDataVersionId",
                table: "Crops",
                columns: new[] { "CropTypeId", "StaticDataVersionId" });

            migrationBuilder.CreateIndex(
                name: "IX_Crops_ManureApplicationHistory_StaticDataVersionId",
                table: "Crops",
                columns: new[] { "ManureApplicationHistory", "StaticDataVersionId" });

            migrationBuilder.CreateIndex(
                name: "IX_CropSoilTestPhosphorousRegions_StaticDataVersionId",
                table: "CropSoilTestPhosphorousRegions",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_CropSoilTestPhosphorousRegions_CropId_StaticDataVersionId",
                table: "CropSoilTestPhosphorousRegions",
                columns: new[] { "CropId", "StaticDataVersionId" });

            migrationBuilder.CreateIndex(
                name: "IX_CropSoilTestPotassiumRegions_StaticDataVersionId",
                table: "CropSoilTestPotassiumRegions",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_CropSoilTestPotassiumRegions_CropId_StaticDataVersionId",
                table: "CropSoilTestPotassiumRegions",
                columns: new[] { "CropId", "StaticDataVersionId" });

            migrationBuilder.CreateIndex(
                name: "IX_CropTypes_StaticDataVersionId",
                table: "CropTypes",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_CropYields_LocationId",
                table: "CropYields",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_CropYields_StaticDataVersionId",
                table: "CropYields",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_CropYields_CropId_StaticDataVersionId",
                table: "CropYields",
                columns: new[] { "CropId", "StaticDataVersionId" });

            migrationBuilder.CreateIndex(
                name: "IX_DefaultSoilTests_StaticDataVersionId",
                table: "DefaultSoilTests",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_DensityUnits_StaticDataVersionId",
                table: "DensityUnits",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_DryMatters_StaticDataVersionId",
                table: "DryMatters",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_FertilizerMethods_StaticDataVersionId",
                table: "FertilizerMethods",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Fertilizers_StaticDataVersionId",
                table: "Fertilizers",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_FertilizerTypes_StaticDataVersionId",
                table: "FertilizerTypes",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_FertilizerUnits_StaticDataVersionId",
                table: "FertilizerUnits",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_HarvestUnits_StaticDataVersionId",
                table: "HarvestUnits",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_LiquidFertilizerDensities_StaticDataVersionId",
                table: "LiquidFertilizerDensities",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_LiquidFertilizerDensities_DensityUnitId_StaticDataVersionId",
                table: "LiquidFertilizerDensities",
                columns: new[] { "DensityUnitId", "StaticDataVersionId" });

            migrationBuilder.CreateIndex(
                name: "IX_LiquidFertilizerDensities_FertilizerId_StaticDataVersionId",
                table: "LiquidFertilizerDensities",
                columns: new[] { "FertilizerId", "StaticDataVersionId" });

            migrationBuilder.CreateIndex(
                name: "IX_LiquidMaterialApplicationUsGallonsPerAcreRateConversions_St~",
                table: "LiquidMaterialApplicationUsGallonsPerAcreRateConversions",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_LiquidMaterialsConversionFactors_StaticDataVersionId",
                table: "LiquidMaterialsConversionFactors",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_LiquidSolidSeparationDefaults_StaticDataVersionId",
                table: "LiquidSolidSeparationDefaults",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ManageVersionUsers_UserName",
                table: "ManageVersionUsers",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ManureImportedDefaults_StaticDataVersionId",
                table: "ManureImportedDefaults",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Manures_StaticDataVersionId",
                table: "Manures",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Manures_DryMatterId_StaticDataVersionId",
                table: "Manures",
                columns: new[] { "DryMatterId", "StaticDataVersionId" });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_StaticDataVersionId",
                table: "Messages",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_NitrateCreditSampleDates_StaticDataVersionId",
                table: "NitrateCreditSampleDates",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_NitrogenMineralizations_LocationId",
                table: "NitrogenMineralizations",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_NitrogenMineralizations_StaticDataVersionId",
                table: "NitrogenMineralizations",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_NitrogenRecommendations_StaticDataVersionId",
                table: "NitrogenRecommendations",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_PhosphorusSoilTestRanges_StaticDataVersionId",
                table: "PhosphorusSoilTestRanges",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_PotassiumSoilTestRanges_StaticDataVersionId",
                table: "PotassiumSoilTestRanges",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_PreviousCropType_StaticDataVersionId",
                table: "PreviousCropType",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_PreviousCropType_CropTypeId_CropTypeStaticDataVersionId",
                table: "PreviousCropType",
                columns: new[] { "CropTypeId", "CropTypeStaticDataVersionId" });

            migrationBuilder.CreateIndex(
                name: "IX_PreviousCropType_CropId_PreviousCropCode_StaticDataVersionId",
                table: "PreviousCropType",
                columns: new[] { "CropId", "PreviousCropCode", "StaticDataVersionId" });

            migrationBuilder.CreateIndex(
                name: "IX_PrevManureApplicationYears_StaticDataVersionId",
                table: "PrevManureApplicationYears",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_PrevYearManureApplicationNitrogenDefaults_StaticDataVersion~",
                table: "PrevYearManureApplicationNitrogenDefaults",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_LocationId",
                table: "Regions",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_StaticDataVersionId",
                table: "Regions",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_RptCompletedFertilizerRequiredStdUnits_StaticDataVersionId",
                table: "RptCompletedFertilizerRequiredStdUnits",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_RptCompletedManureRequiredStdUnits_StaticDataVersionId",
                table: "RptCompletedManureRequiredStdUnits",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_SeasonApplications_StaticDataVersionId",
                table: "SeasonApplications",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_SoilTestMethods_StaticDataVersionId",
                table: "SoilTestMethods",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_SoilTestPhosphorousKelownaRanges_StaticDataVersionId",
                table: "SoilTestPhosphorousKelownaRanges",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_SoilTestPhosphorousRecommendation_StaticDataVersionId",
                table: "SoilTestPhosphorousRecommendation",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_SoilTestPhosphorousRecommendation_SoilTestPhosphorousKelown~",
                table: "SoilTestPhosphorousRecommendation",
                columns: new[] { "SoilTestPhosphorousKelownaRangeId", "StaticDataVersionId" });

            migrationBuilder.CreateIndex(
                name: "IX_SoilTestPotassiumKelownaRanges_StaticDataVersionId",
                table: "SoilTestPotassiumKelownaRanges",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_SoilTestPotassiumRecommendation_StaticDataVersionId",
                table: "SoilTestPotassiumRecommendation",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_SoilTestPotassiumRecommendation_SoilTestPotassiumKelownaRan~",
                table: "SoilTestPotassiumRecommendation",
                columns: new[] { "SoilTestPotassiumKelownaRangeId", "StaticDataVersionId" });

            migrationBuilder.CreateIndex(
                name: "IX_SolidMaterialApplicationTonPerAcreRateConversions_StaticDat~",
                table: "SolidMaterialApplicationTonPerAcreRateConversions",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_SolidMaterialsConversionFactors_StaticDataVersionId",
                table: "SolidMaterialsConversionFactors",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubMenu_MainMenuId",
                table: "SubMenu",
                column: "MainMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_SubRegion_StaticDataVersionId",
                table: "SubRegion",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubRegion_RegionId_StaticDataVersionId",
                table: "SubRegion",
                columns: new[] { "RegionId", "StaticDataVersionId" });

            migrationBuilder.CreateIndex(
                name: "IX_Units_StaticDataVersionId",
                table: "Units",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Yields_StaticDataVersionId",
                table: "Yields",
                column: "StaticDataVersionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AmmoniaRetentions");

            migrationBuilder.DropTable(
                name: "AnimalSubType");

            migrationBuilder.DropTable(
                name: "AppliedMigrationSeedData");

            migrationBuilder.DropTable(
                name: "BCSampleDateForNitrateCredit");

            migrationBuilder.DropTable(
                name: "Breed");

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
                name: "LiquidMaterialApplicationUsGallonsPerAcreRateConversions");

            migrationBuilder.DropTable(
                name: "LiquidMaterialsConversionFactors");

            migrationBuilder.DropTable(
                name: "LiquidSolidSeparationDefaults");

            migrationBuilder.DropTable(
                name: "ManageVersionUsers");

            migrationBuilder.DropTable(
                name: "ManureImportedDefaults");

            migrationBuilder.DropTable(
                name: "Manures");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "NitrateCreditSampleDates");

            migrationBuilder.DropTable(
                name: "NitrogenMineralizations");

            migrationBuilder.DropTable(
                name: "NitrogenRecommendations");

            migrationBuilder.DropTable(
                name: "NutrientIcons");

            migrationBuilder.DropTable(
                name: "PhosphorusSoilTestRanges");

            migrationBuilder.DropTable(
                name: "PotassiumSoilTestRanges");

            migrationBuilder.DropTable(
                name: "PreviousCropType");

            migrationBuilder.DropTable(
                name: "RptCompletedFertilizerRequiredStdUnits");

            migrationBuilder.DropTable(
                name: "RptCompletedManureRequiredStdUnits");

            migrationBuilder.DropTable(
                name: "SeasonApplications");

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
                name: "SolidMaterialApplicationTonPerAcreRateConversions");

            migrationBuilder.DropTable(
                name: "SolidMaterialsConversionFactors");

            migrationBuilder.DropTable(
                name: "SubMenu");

            migrationBuilder.DropTable(
                name: "SubRegion");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "UserPrompts");

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
                name: "Crops");

            migrationBuilder.DropTable(
                name: "SoilTestPhosphorousKelownaRanges");

            migrationBuilder.DropTable(
                name: "SoilTestPotassiumKelownaRanges");

            migrationBuilder.DropTable(
                name: "MainMenus");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "CropTypes");

            migrationBuilder.DropTable(
                name: "PrevYearManureApplicationNitrogenDefaults");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "PrevManureApplicationYears");

            migrationBuilder.DropTable(
                name: "StaticDataVersions");
        }
    }
}
