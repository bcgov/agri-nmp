using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Agri.Data.Migrations
{
    public partial class AddRelationshipToStaticDataVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimalSubType_Animals_AnimalId",
                table: "AnimalSubType");

            migrationBuilder.DropForeignKey(
                name: "FK_Breed_Animals_AnimalId",
                table: "Breed");

            migrationBuilder.DropForeignKey(
                name: "FK_Crops_CropTypes_CropTypeId",
                table: "Crops");

            migrationBuilder.DropForeignKey(
                name: "FK_Crops_PrevYearManureApplicationNitrogenDefaults_ManureAppli~",
                table: "Crops");

            migrationBuilder.DropForeignKey(
                name: "FK_CropSoilTestPhosphorousRegions_Crops_CropId",
                table: "CropSoilTestPhosphorousRegions");

            migrationBuilder.DropForeignKey(
                name: "FK_CropSoilTestPotassiumRegions_Crops_CropId",
                table: "CropSoilTestPotassiumRegions");

            migrationBuilder.DropForeignKey(
                name: "FK_CropYields_Crops_CropId",
                table: "CropYields");

            migrationBuilder.DropForeignKey(
                name: "FK_LiquidFertilizerDensities_DensityUnits_DensityUnitId",
                table: "LiquidFertilizerDensities");

            migrationBuilder.DropForeignKey(
                name: "FK_LiquidFertilizerDensities_Fertilizers_FertilizerId",
                table: "LiquidFertilizerDensities");

            migrationBuilder.DropForeignKey(
                name: "FK_Manures_DryMatters_DMId",
                table: "Manures");

            migrationBuilder.DropForeignKey(
                name: "FK_Manures_NitrogenMineralizations_NMineralizationId1_NMineral~",
                table: "Manures");

            migrationBuilder.DropForeignKey(
                name: "FK_PreviousCropType_Crops_CropId",
                table: "PreviousCropType");

            migrationBuilder.DropForeignKey(
                name: "FK_PreviousCropType_CropTypes_CropTypeId",
                table: "PreviousCropType");

            migrationBuilder.DropForeignKey(
                name: "FK_PrevYearManureApplicationNitrogenDefaults_PrevManureApplica~",
                table: "PrevYearManureApplicationNitrogenDefaults");

            migrationBuilder.DropForeignKey(
                name: "FK_SoilTestPhosphorousRecommendation_SoilTestPhosphorousKelown~",
                table: "SoilTestPhosphorousRecommendation");

            migrationBuilder.DropForeignKey(
                name: "FK_SoilTestPotassiumRecommendation_SoilTestPotassiumKelownaRan~",
                table: "SoilTestPotassiumRecommendation");

            migrationBuilder.DropForeignKey(
                name: "FK_SubRegion_Regions_RegionId",
                table: "SubRegion");

            migrationBuilder.DropTable(
                name: "SelectCodeItems");

            migrationBuilder.DropTable(
                name: "SelectListItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Yields",
                table: "Yields");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Units",
                table: "Units");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubRegion",
                table: "SubRegion");

            migrationBuilder.DropIndex(
                name: "IX_SubRegion_RegionId",
                table: "SubRegion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SolidMaterialsConversionFactors",
                table: "SolidMaterialsConversionFactors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SolidMaterialApplicationTonPerAcreRateConversions",
                table: "SolidMaterialApplicationTonPerAcreRateConversions");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_SoilTestPotassiumRecommendation_PotassiumCropGroupRegionCod~",
                table: "SoilTestPotassiumRecommendation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SoilTestPotassiumRecommendation",
                table: "SoilTestPotassiumRecommendation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SoilTestPotassiumRanges",
                table: "SoilTestPotassiumRanges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SoilTestPotassiumKelownaRanges",
                table: "SoilTestPotassiumKelownaRanges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SoilTestPhosphorusRanges",
                table: "SoilTestPhosphorusRanges");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_SoilTestPhosphorousRecommendation_PhosphorousCropGroupRegio~",
                table: "SoilTestPhosphorousRecommendation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SoilTestPhosphorousRecommendation",
                table: "SoilTestPhosphorousRecommendation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SoilTestPhosphorousKelownaRanges",
                table: "SoilTestPhosphorousKelownaRanges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SoilTestMethods",
                table: "SoilTestMethods");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SeasonApplications",
                table: "SeasonApplications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RptCompletedManureRequiredStdUnits",
                table: "RptCompletedManureRequiredStdUnits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RptCompletedFertilizerRequiredStdUnits",
                table: "RptCompletedFertilizerRequiredStdUnits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Regions",
                table: "Regions");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_PrevYearManureApplicationNitrogenDefaults_FieldManureApplic~",
                table: "PrevYearManureApplicationNitrogenDefaults");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PrevYearManureApplicationNitrogenDefaults",
                table: "PrevYearManureApplicationNitrogenDefaults");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_PrevManureApplicationYears_FieldManureApplicationHistory",
                table: "PrevManureApplicationYears");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PrevManureApplicationYears",
                table: "PrevManureApplicationYears");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PreviousCropType",
                table: "PreviousCropType");

            migrationBuilder.DropIndex(
                name: "IX_PreviousCropType_CropId",
                table: "PreviousCropType");

            migrationBuilder.DropIndex(
                name: "IX_PreviousCropType_CropTypeId",
                table: "PreviousCropType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PotassiumSoilTestRanges",
                table: "PotassiumSoilTestRanges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PhosphorusSoilTestRanges",
                table: "PhosphorusSoilTestRanges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NitrogenRecommendations",
                table: "NitrogenRecommendations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NitrogenMineralizations",
                table: "NitrogenMineralizations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NitrateCreditSampleDates",
                table: "NitrateCreditSampleDates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Messages",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Manures",
                table: "Manures");

            migrationBuilder.DropIndex(
                name: "IX_Manures_DMId",
                table: "Manures");

            migrationBuilder.DropIndex(
                name: "IX_Manures_NMineralizationId1_NMineralizationLocationId",
                table: "Manures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ManureImportedDefaults",
                table: "ManureImportedDefaults");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LiquidSolidSeparationDefaults",
                table: "LiquidSolidSeparationDefaults");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LiquidMaterialsConversionFactors",
                table: "LiquidMaterialsConversionFactors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LiquidMaterialApplicationUsGallonsPerAcreRateConversions",
                table: "LiquidMaterialApplicationUsGallonsPerAcreRateConversions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LiquidFertilizerDensities",
                table: "LiquidFertilizerDensities");

            migrationBuilder.DropIndex(
                name: "IX_LiquidFertilizerDensities_DensityUnitId",
                table: "LiquidFertilizerDensities");

            migrationBuilder.DropIndex(
                name: "IX_LiquidFertilizerDensities_FertilizerId",
                table: "LiquidFertilizerDensities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HarvestUnits",
                table: "HarvestUnits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FertilizerUnits",
                table: "FertilizerUnits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FertilizerTypes",
                table: "FertilizerTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fertilizers",
                table: "Fertilizers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FertilizerMethods",
                table: "FertilizerMethods");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DryMatters",
                table: "DryMatters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DensityUnits",
                table: "DensityUnits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DefaultSoilTests",
                table: "DefaultSoilTests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CropYields",
                table: "CropYields");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CropTypes",
                table: "CropTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CropSoilTestPotassiumRegions",
                table: "CropSoilTestPotassiumRegions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CropSoilTestPhosphorousRegions",
                table: "CropSoilTestPhosphorousRegions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Crops",
                table: "Crops");

            migrationBuilder.DropIndex(
                name: "IX_Crops_CropTypeId",
                table: "Crops");

            migrationBuilder.DropIndex(
                name: "IX_Crops_ManureApplicationHistory",
                table: "Crops");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConversionFactors",
                table: "ConversionFactors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Breed",
                table: "Breed");

            migrationBuilder.DropIndex(
                name: "IX_Breed_AnimalId",
                table: "Breed");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BCSampleDateForNitrateCredit",
                table: "BCSampleDateForNitrateCredit");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnimalSubType",
                table: "AnimalSubType");

            migrationBuilder.DropIndex(
                name: "IX_AnimalSubType_AnimalId",
                table: "AnimalSubType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Animals",
                table: "Animals");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AmmoniaRetentions_DryMatter_SeasonApplicationId",
                table: "AmmoniaRetentions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AmmoniaRetentions",
                table: "AmmoniaRetentions");

            migrationBuilder.DropColumn(
                name: "NMineralizationId1",
                table: "Manures");

            migrationBuilder.DropColumn(
                name: "NMineralizationLocationId",
                table: "Manures");

            migrationBuilder.RenameColumn(
                name: "DMId",
                table: "Manures",
                newName: "DryMatterId");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Yields",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "Yields",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Units",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "Units",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "SubRegion",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "SubRegion",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "SolidMaterialsConversionFactors",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "SolidMaterialsConversionFactors",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "SolidMaterialApplicationTonPerAcreRateConversions",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "SolidMaterialApplicationTonPerAcreRateConversions",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "SoilTestPotassiumRecommendation",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "UpperLimit",
                table: "SoilTestPotassiumRanges",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "SoilTestPotassiumRanges",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "SoilTestPotassiumKelownaRanges",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "SoilTestPotassiumKelownaRanges",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "UpperLimit",
                table: "SoilTestPhosphorusRanges",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "SoilTestPhosphorusRanges",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "SoilTestPhosphorousRecommendation",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "SoilTestPhosphorousKelownaRanges",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "SoilTestPhosphorousKelownaRanges",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "SoilTestMethods",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "SoilTestMethods",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "SeasonApplications",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "SeasonApplications",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "RptCompletedManureRequiredStdUnits",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "RptCompletedManureRequiredStdUnits",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "RptCompletedFertilizerRequiredStdUnits",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "RptCompletedFertilizerRequiredStdUnits",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Regions",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "Regions",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "PrevYearManureApplicationNitrogenDefaults",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "PrevManureApplicationYears",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "PreviousCropType",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "PreviousCropType",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CropStaticDataVersionId",
                table: "PreviousCropType",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CropTypeStaticDataVersionId",
                table: "PreviousCropType",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "PotassiumSoilTestRanges",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "PhosphorusSoilTestRanges",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "NitrogenRecommendations",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "NitrogenRecommendations",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "NitrogenMineralizations",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "NitrateCreditSampleDates",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Messages",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "Messages",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Manures",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "Manures",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ManureImportedDefaults",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "ManureImportedDefaults",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "LiquidSolidSeparationDefaults",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "LiquidSolidSeparationDefaults",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "LiquidMaterialsConversionFactors",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "LiquidMaterialsConversionFactors",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "LiquidMaterialApplicationUsGallonsPerAcreRateConversions",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "LiquidMaterialApplicationUsGallonsPerAcreRateConversions",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "LiquidFertilizerDensities",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "LiquidFertilizerDensities",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "HarvestUnits",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "HarvestUnits",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "FertilizerUnits",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "FertilizerUnits",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "FertilizerTypes",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "FertilizerTypes",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Fertilizers",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "Fertilizers",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "FertilizerMethods",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "FertilizerMethods",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "DryMatters",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "DryMatters",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "DensityUnits",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "DensityUnits",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "DefaultSoilTests",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "DefaultSoilTests",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "CropYields",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "CropTypes",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "CropTypes",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "CropSoilTestPotassiumRegions",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "CropSoilTestPhosphorousRegions",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Crops",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "Crops",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ConversionFactors",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "ConversionFactors",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Breed",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "Breed",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "BCSampleDateForNitrateCredit",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "AnimalSubType",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "AnimalSubType",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Animals",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "Animals",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "StaticDataVersionId",
                table: "AmmoniaRetentions",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Yields",
                table: "Yields",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Units",
                table: "Units",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubRegion",
                table: "SubRegion",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SolidMaterialsConversionFactors",
                table: "SolidMaterialsConversionFactors",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SolidMaterialApplicationTonPerAcreRateConversions",
                table: "SolidMaterialApplicationTonPerAcreRateConversions",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_SoilTestPotassiumRecommendation_PotassiumCropGroupRegionCod~",
                table: "SoilTestPotassiumRecommendation",
                columns: new[] { "PotassiumCropGroupRegionCode", "SoilTestPotassiumKelownaRangeId", "SoilTestPotassiumRegionCode", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SoilTestPotassiumRecommendation",
                table: "SoilTestPotassiumRecommendation",
                columns: new[] { "SoilTestPotassiumKelownaRangeId", "SoilTestPotassiumRegionCode", "PotassiumCropGroupRegionCode", "StaticDataVersionId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_SoilTestPotassiumRanges_StaticDataVersionId_UpperLimit",
                table: "SoilTestPotassiumRanges",
                columns: new[] { "StaticDataVersionId", "UpperLimit" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SoilTestPotassiumRanges",
                table: "SoilTestPotassiumRanges",
                columns: new[] { "UpperLimit", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SoilTestPotassiumKelownaRanges",
                table: "SoilTestPotassiumKelownaRanges",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_SoilTestPhosphorusRanges_StaticDataVersionId_UpperLimit",
                table: "SoilTestPhosphorusRanges",
                columns: new[] { "StaticDataVersionId", "UpperLimit" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SoilTestPhosphorusRanges",
                table: "SoilTestPhosphorusRanges",
                columns: new[] { "UpperLimit", "StaticDataVersionId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_SoilTestPhosphorousRecommendation_PhosphorousCropGroupRegio~",
                table: "SoilTestPhosphorousRecommendation",
                columns: new[] { "PhosphorousCropGroupRegionCode", "SoilTestPhosphorousKelownaRangeId", "SoilTestPhosphorousRegionCode", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SoilTestPhosphorousRecommendation",
                table: "SoilTestPhosphorousRecommendation",
                columns: new[] { "SoilTestPhosphorousKelownaRangeId", "SoilTestPhosphorousRegionCode", "PhosphorousCropGroupRegionCode", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SoilTestPhosphorousKelownaRanges",
                table: "SoilTestPhosphorousKelownaRanges",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SoilTestMethods",
                table: "SoilTestMethods",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SeasonApplications",
                table: "SeasonApplications",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_RptCompletedManureRequiredStdUnits",
                table: "RptCompletedManureRequiredStdUnits",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_RptCompletedFertilizerRequiredStdUnits",
                table: "RptCompletedFertilizerRequiredStdUnits",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Regions",
                table: "Regions",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_PrevYearManureApplicationNitrogenDefaults_FieldManureApplic~",
                table: "PrevYearManureApplicationNitrogenDefaults",
                columns: new[] { "FieldManureApplicationHistory", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PrevYearManureApplicationNitrogenDefaults",
                table: "PrevYearManureApplicationNitrogenDefaults",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_PrevManureApplicationYears_FieldManureApplicationHistory_St~",
                table: "PrevManureApplicationYears",
                columns: new[] { "FieldManureApplicationHistory", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PrevManureApplicationYears",
                table: "PrevManureApplicationYears",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PreviousCropType",
                table: "PreviousCropType",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PotassiumSoilTestRanges",
                table: "PotassiumSoilTestRanges",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PhosphorusSoilTestRanges",
                table: "PhosphorusSoilTestRanges",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_NitrogenRecommendations",
                table: "NitrogenRecommendations",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_NitrogenMineralizations",
                table: "NitrogenMineralizations",
                columns: new[] { "Id", "LocationId", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_NitrateCreditSampleDates",
                table: "NitrateCreditSampleDates",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Messages",
                table: "Messages",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Manures",
                table: "Manures",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ManureImportedDefaults",
                table: "ManureImportedDefaults",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_LiquidSolidSeparationDefaults",
                table: "LiquidSolidSeparationDefaults",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_LiquidMaterialsConversionFactors",
                table: "LiquidMaterialsConversionFactors",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_LiquidMaterialApplicationUsGallonsPerAcreRateConversions",
                table: "LiquidMaterialApplicationUsGallonsPerAcreRateConversions",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_LiquidFertilizerDensities",
                table: "LiquidFertilizerDensities",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_HarvestUnits",
                table: "HarvestUnits",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_FertilizerUnits",
                table: "FertilizerUnits",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_FertilizerTypes",
                table: "FertilizerTypes",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fertilizers",
                table: "Fertilizers",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_FertilizerMethods",
                table: "FertilizerMethods",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_DryMatters",
                table: "DryMatters",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_DensityUnits",
                table: "DensityUnits",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_DefaultSoilTests",
                table: "DefaultSoilTests",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CropYields",
                table: "CropYields",
                columns: new[] { "CropId", "LocationId", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CropTypes",
                table: "CropTypes",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CropSoilTestPotassiumRegions",
                table: "CropSoilTestPotassiumRegions",
                columns: new[] { "CropId", "SoilTestPotassiumRegionCode", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CropSoilTestPhosphorousRegions",
                table: "CropSoilTestPhosphorousRegions",
                columns: new[] { "CropId", "SoilTestPhosphorousRegionCode", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Crops",
                table: "Crops",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConversionFactors",
                table: "ConversionFactors",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Breed",
                table: "Breed",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_BCSampleDateForNitrateCredit",
                table: "BCSampleDateForNitrateCredit",
                columns: new[] { "CoastalFromDateMonth", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnimalSubType",
                table: "AnimalSubType",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Animals",
                table: "Animals",
                columns: new[] { "Id", "StaticDataVersionId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AmmoniaRetentions_DryMatter_SeasonApplicationId_StaticDataV~",
                table: "AmmoniaRetentions",
                columns: new[] { "DryMatter", "SeasonApplicationId", "StaticDataVersionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AmmoniaRetentions",
                table: "AmmoniaRetentions",
                columns: new[] { "SeasonApplicationId", "DryMatter", "StaticDataVersionId" });

            migrationBuilder.CreateIndex(
                name: "IX_Yields_StaticDataVersionId",
                table: "Yields",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Units_StaticDataVersionId",
                table: "Units",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubRegion_StaticDataVersionId",
                table: "SubRegion",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubRegion_RegionId_StaticDataVersionId",
                table: "SubRegion",
                columns: new[] { "RegionId", "StaticDataVersionId" });

            migrationBuilder.CreateIndex(
                name: "IX_SolidMaterialsConversionFactors_StaticDataVersionId",
                table: "SolidMaterialsConversionFactors",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_SolidMaterialApplicationTonPerAcreRateConversions_StaticDat~",
                table: "SolidMaterialApplicationTonPerAcreRateConversions",
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
                name: "IX_SoilTestPotassiumKelownaRanges_StaticDataVersionId",
                table: "SoilTestPotassiumKelownaRanges",
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
                name: "IX_SoilTestPhosphorousKelownaRanges_StaticDataVersionId",
                table: "SoilTestPhosphorousKelownaRanges",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_SoilTestMethods_StaticDataVersionId",
                table: "SoilTestMethods",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_SeasonApplications_StaticDataVersionId",
                table: "SeasonApplications",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_RptCompletedManureRequiredStdUnits_StaticDataVersionId",
                table: "RptCompletedManureRequiredStdUnits",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_RptCompletedFertilizerRequiredStdUnits_StaticDataVersionId",
                table: "RptCompletedFertilizerRequiredStdUnits",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_StaticDataVersionId",
                table: "Regions",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_PrevYearManureApplicationNitrogenDefaults_StaticDataVersion~",
                table: "PrevYearManureApplicationNitrogenDefaults",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_PrevManureApplicationYears_StaticDataVersionId",
                table: "PrevManureApplicationYears",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_PreviousCropType_StaticDataVersionId",
                table: "PreviousCropType",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_PreviousCropType_CropId_CropStaticDataVersionId",
                table: "PreviousCropType",
                columns: new[] { "CropId", "CropStaticDataVersionId" });

            migrationBuilder.CreateIndex(
                name: "IX_PreviousCropType_CropTypeId_CropTypeStaticDataVersionId",
                table: "PreviousCropType",
                columns: new[] { "CropTypeId", "CropTypeStaticDataVersionId" });

            migrationBuilder.CreateIndex(
                name: "IX_PotassiumSoilTestRanges_StaticDataVersionId",
                table: "PotassiumSoilTestRanges",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_PhosphorusSoilTestRanges_StaticDataVersionId",
                table: "PhosphorusSoilTestRanges",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_NitrogenRecommendations_StaticDataVersionId",
                table: "NitrogenRecommendations",
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
                name: "IX_NitrateCreditSampleDates_StaticDataVersionId",
                table: "NitrateCreditSampleDates",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_StaticDataVersionId",
                table: "Messages",
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
                name: "IX_ManureImportedDefaults_StaticDataVersionId",
                table: "ManureImportedDefaults",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_LiquidSolidSeparationDefaults_StaticDataVersionId",
                table: "LiquidSolidSeparationDefaults",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_LiquidMaterialsConversionFactors_StaticDataVersionId",
                table: "LiquidMaterialsConversionFactors",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_LiquidMaterialApplicationUsGallonsPerAcreRateConversions_St~",
                table: "LiquidMaterialApplicationUsGallonsPerAcreRateConversions",
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
                name: "IX_HarvestUnits_StaticDataVersionId",
                table: "HarvestUnits",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_FertilizerUnits_StaticDataVersionId",
                table: "FertilizerUnits",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_FertilizerTypes_StaticDataVersionId",
                table: "FertilizerTypes",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Fertilizers_StaticDataVersionId",
                table: "Fertilizers",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_FertilizerMethods_StaticDataVersionId",
                table: "FertilizerMethods",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_DryMatters_StaticDataVersionId",
                table: "DryMatters",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_DensityUnits_StaticDataVersionId",
                table: "DensityUnits",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_DefaultSoilTests_StaticDataVersionId",
                table: "DefaultSoilTests",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_CropYields_StaticDataVersionId",
                table: "CropYields",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_CropYields_CropId_StaticDataVersionId",
                table: "CropYields",
                columns: new[] { "CropId", "StaticDataVersionId" });

            migrationBuilder.CreateIndex(
                name: "IX_CropTypes_StaticDataVersionId",
                table: "CropTypes",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_CropSoilTestPotassiumRegions_StaticDataVersionId",
                table: "CropSoilTestPotassiumRegions",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_CropSoilTestPotassiumRegions_CropId_StaticDataVersionId",
                table: "CropSoilTestPotassiumRegions",
                columns: new[] { "CropId", "StaticDataVersionId" });

            migrationBuilder.CreateIndex(
                name: "IX_CropSoilTestPhosphorousRegions_StaticDataVersionId",
                table: "CropSoilTestPhosphorousRegions",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_CropSoilTestPhosphorousRegions_CropId_StaticDataVersionId",
                table: "CropSoilTestPhosphorousRegions",
                columns: new[] { "CropId", "StaticDataVersionId" });

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
                name: "IX_ConversionFactors_StaticDataVersionId",
                table: "ConversionFactors",
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
                name: "IX_BCSampleDateForNitrateCredit_StaticDataVersionId",
                table: "BCSampleDateForNitrateCredit",
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
                name: "IX_Animals_StaticDataVersionId",
                table: "Animals",
                column: "StaticDataVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_AmmoniaRetentions_StaticDataVersionId",
                table: "AmmoniaRetentions",
                column: "StaticDataVersionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AmmoniaRetentions_StaticDataVersions_StaticDataVersionId",
                table: "AmmoniaRetentions",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Animals_StaticDataVersions_StaticDataVersionId",
                table: "Animals",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnimalSubType_StaticDataVersions_StaticDataVersionId",
                table: "AnimalSubType",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnimalSubType_Animals_AnimalId_StaticDataVersionId",
                table: "AnimalSubType",
                columns: new[] { "AnimalId", "StaticDataVersionId" },
                principalTable: "Animals",
                principalColumns: new[] { "Id", "StaticDataVersionId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BCSampleDateForNitrateCredit_StaticDataVersions_StaticDataV~",
                table: "BCSampleDateForNitrateCredit",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Breed_StaticDataVersions_StaticDataVersionId",
                table: "Breed",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Breed_Animals_AnimalId_StaticDataVersionId",
                table: "Breed",
                columns: new[] { "AnimalId", "StaticDataVersionId" },
                principalTable: "Animals",
                principalColumns: new[] { "Id", "StaticDataVersionId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConversionFactors_StaticDataVersions_StaticDataVersionId",
                table: "ConversionFactors",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Crops_StaticDataVersions_StaticDataVersionId",
                table: "Crops",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Crops_CropTypes_CropTypeId_StaticDataVersionId",
                table: "Crops",
                columns: new[] { "CropTypeId", "StaticDataVersionId" },
                principalTable: "CropTypes",
                principalColumns: new[] { "Id", "StaticDataVersionId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Crops_PrevYearManureApplicationNitrogenDefaults_ManureAppli~",
                table: "Crops",
                columns: new[] { "ManureApplicationHistory", "StaticDataVersionId" },
                principalTable: "PrevYearManureApplicationNitrogenDefaults",
                principalColumns: new[] { "FieldManureApplicationHistory", "StaticDataVersionId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CropSoilTestPhosphorousRegions_StaticDataVersions_StaticDat~",
                table: "CropSoilTestPhosphorousRegions",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CropSoilTestPhosphorousRegions_Crops_CropId_StaticDataVersi~",
                table: "CropSoilTestPhosphorousRegions",
                columns: new[] { "CropId", "StaticDataVersionId" },
                principalTable: "Crops",
                principalColumns: new[] { "Id", "StaticDataVersionId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CropSoilTestPotassiumRegions_StaticDataVersions_StaticDataV~",
                table: "CropSoilTestPotassiumRegions",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CropSoilTestPotassiumRegions_Crops_CropId_StaticDataVersion~",
                table: "CropSoilTestPotassiumRegions",
                columns: new[] { "CropId", "StaticDataVersionId" },
                principalTable: "Crops",
                principalColumns: new[] { "Id", "StaticDataVersionId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CropTypes_StaticDataVersions_StaticDataVersionId",
                table: "CropTypes",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CropYields_StaticDataVersions_StaticDataVersionId",
                table: "CropYields",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CropYields_Crops_CropId_StaticDataVersionId",
                table: "CropYields",
                columns: new[] { "CropId", "StaticDataVersionId" },
                principalTable: "Crops",
                principalColumns: new[] { "Id", "StaticDataVersionId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DefaultSoilTests_StaticDataVersions_StaticDataVersionId",
                table: "DefaultSoilTests",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DensityUnits_StaticDataVersions_StaticDataVersionId",
                table: "DensityUnits",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DryMatters_StaticDataVersions_StaticDataVersionId",
                table: "DryMatters",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FertilizerMethods_StaticDataVersions_StaticDataVersionId",
                table: "FertilizerMethods",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fertilizers_StaticDataVersions_StaticDataVersionId",
                table: "Fertilizers",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FertilizerTypes_StaticDataVersions_StaticDataVersionId",
                table: "FertilizerTypes",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FertilizerUnits_StaticDataVersions_StaticDataVersionId",
                table: "FertilizerUnits",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HarvestUnits_StaticDataVersions_StaticDataVersionId",
                table: "HarvestUnits",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LiquidFertilizerDensities_StaticDataVersions_StaticDataVers~",
                table: "LiquidFertilizerDensities",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LiquidFertilizerDensities_DensityUnits_DensityUnitId_Static~",
                table: "LiquidFertilizerDensities",
                columns: new[] { "DensityUnitId", "StaticDataVersionId" },
                principalTable: "DensityUnits",
                principalColumns: new[] { "Id", "StaticDataVersionId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LiquidFertilizerDensities_Fertilizers_FertilizerId_StaticDa~",
                table: "LiquidFertilizerDensities",
                columns: new[] { "FertilizerId", "StaticDataVersionId" },
                principalTable: "Fertilizers",
                principalColumns: new[] { "Id", "StaticDataVersionId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LiquidMaterialApplicationUsGallonsPerAcreRateConversions_St~",
                table: "LiquidMaterialApplicationUsGallonsPerAcreRateConversions",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LiquidMaterialsConversionFactors_StaticDataVersions_StaticD~",
                table: "LiquidMaterialsConversionFactors",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LiquidSolidSeparationDefaults_StaticDataVersions_StaticData~",
                table: "LiquidSolidSeparationDefaults",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ManureImportedDefaults_StaticDataVersions_StaticDataVersion~",
                table: "ManureImportedDefaults",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Manures_StaticDataVersions_StaticDataVersionId",
                table: "Manures",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Manures_DryMatters_DryMatterId_StaticDataVersionId",
                table: "Manures",
                columns: new[] { "DryMatterId", "StaticDataVersionId" },
                principalTable: "DryMatters",
                principalColumns: new[] { "Id", "StaticDataVersionId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_StaticDataVersions_StaticDataVersionId",
                table: "Messages",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NitrateCreditSampleDates_StaticDataVersions_StaticDataVersi~",
                table: "NitrateCreditSampleDates",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NitrogenMineralizations_Locations_LocationId",
                table: "NitrogenMineralizations",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NitrogenMineralizations_StaticDataVersions_StaticDataVersio~",
                table: "NitrogenMineralizations",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NitrogenRecommendations_StaticDataVersions_StaticDataVersio~",
                table: "NitrogenRecommendations",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PhosphorusSoilTestRanges_StaticDataVersions_StaticDataVersi~",
                table: "PhosphorusSoilTestRanges",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PotassiumSoilTestRanges_StaticDataVersions_StaticDataVersio~",
                table: "PotassiumSoilTestRanges",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PreviousCropType_StaticDataVersions_StaticDataVersionId",
                table: "PreviousCropType",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PreviousCropType_Crops_CropId_CropStaticDataVersionId",
                table: "PreviousCropType",
                columns: new[] { "CropId", "CropStaticDataVersionId" },
                principalTable: "Crops",
                principalColumns: new[] { "Id", "StaticDataVersionId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PreviousCropType_CropTypes_CropTypeId_CropTypeStaticDataVer~",
                table: "PreviousCropType",
                columns: new[] { "CropTypeId", "CropTypeStaticDataVersionId" },
                principalTable: "CropTypes",
                principalColumns: new[] { "Id", "StaticDataVersionId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PrevManureApplicationYears_StaticDataVersions_StaticDataVer~",
                table: "PrevManureApplicationYears",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrevYearManureApplicationNitrogenDefaults_StaticDataVersion~",
                table: "PrevYearManureApplicationNitrogenDefaults",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrevYearManureApplicationNitrogenDefaults_PrevManureApplica~",
                table: "PrevYearManureApplicationNitrogenDefaults",
                columns: new[] { "FieldManureApplicationHistory", "StaticDataVersionId" },
                principalTable: "PrevManureApplicationYears",
                principalColumns: new[] { "FieldManureApplicationHistory", "StaticDataVersionId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Regions_StaticDataVersions_StaticDataVersionId",
                table: "Regions",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RptCompletedFertilizerRequiredStdUnits_StaticDataVersions_S~",
                table: "RptCompletedFertilizerRequiredStdUnits",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RptCompletedManureRequiredStdUnits_StaticDataVersions_Stati~",
                table: "RptCompletedManureRequiredStdUnits",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeasonApplications_StaticDataVersions_StaticDataVersionId",
                table: "SeasonApplications",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SoilTestMethods_StaticDataVersions_StaticDataVersionId",
                table: "SoilTestMethods",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SoilTestPhosphorousKelownaRanges_StaticDataVersions_StaticD~",
                table: "SoilTestPhosphorousKelownaRanges",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SoilTestPhosphorousRecommendation_StaticDataVersions_Static~",
                table: "SoilTestPhosphorousRecommendation",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SoilTestPhosphorousRecommendation_SoilTestPhosphorousKelown~",
                table: "SoilTestPhosphorousRecommendation",
                columns: new[] { "SoilTestPhosphorousKelownaRangeId", "StaticDataVersionId" },
                principalTable: "SoilTestPhosphorousKelownaRanges",
                principalColumns: new[] { "Id", "StaticDataVersionId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SoilTestPhosphorusRanges_StaticDataVersions_StaticDataVersi~",
                table: "SoilTestPhosphorusRanges",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SoilTestPotassiumKelownaRanges_StaticDataVersions_StaticDat~",
                table: "SoilTestPotassiumKelownaRanges",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SoilTestPotassiumRanges_StaticDataVersions_StaticDataVersio~",
                table: "SoilTestPotassiumRanges",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SoilTestPotassiumRecommendation_StaticDataVersions_StaticDa~",
                table: "SoilTestPotassiumRecommendation",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SoilTestPotassiumRecommendation_SoilTestPotassiumKelownaRan~",
                table: "SoilTestPotassiumRecommendation",
                columns: new[] { "SoilTestPotassiumKelownaRangeId", "StaticDataVersionId" },
                principalTable: "SoilTestPotassiumKelownaRanges",
                principalColumns: new[] { "Id", "StaticDataVersionId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SolidMaterialApplicationTonPerAcreRateConversions_StaticDat~",
                table: "SolidMaterialApplicationTonPerAcreRateConversions",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SolidMaterialsConversionFactors_StaticDataVersions_StaticDa~",
                table: "SolidMaterialsConversionFactors",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubRegion_StaticDataVersions_StaticDataVersionId",
                table: "SubRegion",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubRegion_Regions_RegionId_StaticDataVersionId",
                table: "SubRegion",
                columns: new[] { "RegionId", "StaticDataVersionId" },
                principalTable: "Regions",
                principalColumns: new[] { "Id", "StaticDataVersionId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Units_StaticDataVersions_StaticDataVersionId",
                table: "Units",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Yields_StaticDataVersions_StaticDataVersionId",
                table: "Yields",
                column: "StaticDataVersionId",
                principalTable: "StaticDataVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AmmoniaRetentions_StaticDataVersions_StaticDataVersionId",
                table: "AmmoniaRetentions");

            migrationBuilder.DropForeignKey(
                name: "FK_Animals_StaticDataVersions_StaticDataVersionId",
                table: "Animals");

            migrationBuilder.DropForeignKey(
                name: "FK_AnimalSubType_StaticDataVersions_StaticDataVersionId",
                table: "AnimalSubType");

            migrationBuilder.DropForeignKey(
                name: "FK_AnimalSubType_Animals_AnimalId_StaticDataVersionId",
                table: "AnimalSubType");

            migrationBuilder.DropForeignKey(
                name: "FK_BCSampleDateForNitrateCredit_StaticDataVersions_StaticDataV~",
                table: "BCSampleDateForNitrateCredit");

            migrationBuilder.DropForeignKey(
                name: "FK_Breed_StaticDataVersions_StaticDataVersionId",
                table: "Breed");

            migrationBuilder.DropForeignKey(
                name: "FK_Breed_Animals_AnimalId_StaticDataVersionId",
                table: "Breed");

            migrationBuilder.DropForeignKey(
                name: "FK_ConversionFactors_StaticDataVersions_StaticDataVersionId",
                table: "ConversionFactors");

            migrationBuilder.DropForeignKey(
                name: "FK_Crops_StaticDataVersions_StaticDataVersionId",
                table: "Crops");

            migrationBuilder.DropForeignKey(
                name: "FK_Crops_CropTypes_CropTypeId_StaticDataVersionId",
                table: "Crops");

            migrationBuilder.DropForeignKey(
                name: "FK_Crops_PrevYearManureApplicationNitrogenDefaults_ManureAppli~",
                table: "Crops");

            migrationBuilder.DropForeignKey(
                name: "FK_CropSoilTestPhosphorousRegions_StaticDataVersions_StaticDat~",
                table: "CropSoilTestPhosphorousRegions");

            migrationBuilder.DropForeignKey(
                name: "FK_CropSoilTestPhosphorousRegions_Crops_CropId_StaticDataVersi~",
                table: "CropSoilTestPhosphorousRegions");

            migrationBuilder.DropForeignKey(
                name: "FK_CropSoilTestPotassiumRegions_StaticDataVersions_StaticDataV~",
                table: "CropSoilTestPotassiumRegions");

            migrationBuilder.DropForeignKey(
                name: "FK_CropSoilTestPotassiumRegions_Crops_CropId_StaticDataVersion~",
                table: "CropSoilTestPotassiumRegions");

            migrationBuilder.DropForeignKey(
                name: "FK_CropTypes_StaticDataVersions_StaticDataVersionId",
                table: "CropTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_CropYields_StaticDataVersions_StaticDataVersionId",
                table: "CropYields");

            migrationBuilder.DropForeignKey(
                name: "FK_CropYields_Crops_CropId_StaticDataVersionId",
                table: "CropYields");

            migrationBuilder.DropForeignKey(
                name: "FK_DefaultSoilTests_StaticDataVersions_StaticDataVersionId",
                table: "DefaultSoilTests");

            migrationBuilder.DropForeignKey(
                name: "FK_DensityUnits_StaticDataVersions_StaticDataVersionId",
                table: "DensityUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_DryMatters_StaticDataVersions_StaticDataVersionId",
                table: "DryMatters");

            migrationBuilder.DropForeignKey(
                name: "FK_FertilizerMethods_StaticDataVersions_StaticDataVersionId",
                table: "FertilizerMethods");

            migrationBuilder.DropForeignKey(
                name: "FK_Fertilizers_StaticDataVersions_StaticDataVersionId",
                table: "Fertilizers");

            migrationBuilder.DropForeignKey(
                name: "FK_FertilizerTypes_StaticDataVersions_StaticDataVersionId",
                table: "FertilizerTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_FertilizerUnits_StaticDataVersions_StaticDataVersionId",
                table: "FertilizerUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_HarvestUnits_StaticDataVersions_StaticDataVersionId",
                table: "HarvestUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_LiquidFertilizerDensities_StaticDataVersions_StaticDataVers~",
                table: "LiquidFertilizerDensities");

            migrationBuilder.DropForeignKey(
                name: "FK_LiquidFertilizerDensities_DensityUnits_DensityUnitId_Static~",
                table: "LiquidFertilizerDensities");

            migrationBuilder.DropForeignKey(
                name: "FK_LiquidFertilizerDensities_Fertilizers_FertilizerId_StaticDa~",
                table: "LiquidFertilizerDensities");

            migrationBuilder.DropForeignKey(
                name: "FK_LiquidMaterialApplicationUsGallonsPerAcreRateConversions_St~",
                table: "LiquidMaterialApplicationUsGallonsPerAcreRateConversions");

            migrationBuilder.DropForeignKey(
                name: "FK_LiquidMaterialsConversionFactors_StaticDataVersions_StaticD~",
                table: "LiquidMaterialsConversionFactors");

            migrationBuilder.DropForeignKey(
                name: "FK_LiquidSolidSeparationDefaults_StaticDataVersions_StaticData~",
                table: "LiquidSolidSeparationDefaults");

            migrationBuilder.DropForeignKey(
                name: "FK_ManureImportedDefaults_StaticDataVersions_StaticDataVersion~",
                table: "ManureImportedDefaults");

            migrationBuilder.DropForeignKey(
                name: "FK_Manures_StaticDataVersions_StaticDataVersionId",
                table: "Manures");

            migrationBuilder.DropForeignKey(
                name: "FK_Manures_DryMatters_DryMatterId_StaticDataVersionId",
                table: "Manures");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_StaticDataVersions_StaticDataVersionId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_NitrateCreditSampleDates_StaticDataVersions_StaticDataVersi~",
                table: "NitrateCreditSampleDates");

            migrationBuilder.DropForeignKey(
                name: "FK_NitrogenMineralizations_Locations_LocationId",
                table: "NitrogenMineralizations");

            migrationBuilder.DropForeignKey(
                name: "FK_NitrogenMineralizations_StaticDataVersions_StaticDataVersio~",
                table: "NitrogenMineralizations");

            migrationBuilder.DropForeignKey(
                name: "FK_NitrogenRecommendations_StaticDataVersions_StaticDataVersio~",
                table: "NitrogenRecommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_PhosphorusSoilTestRanges_StaticDataVersions_StaticDataVersi~",
                table: "PhosphorusSoilTestRanges");

            migrationBuilder.DropForeignKey(
                name: "FK_PotassiumSoilTestRanges_StaticDataVersions_StaticDataVersio~",
                table: "PotassiumSoilTestRanges");

            migrationBuilder.DropForeignKey(
                name: "FK_PreviousCropType_StaticDataVersions_StaticDataVersionId",
                table: "PreviousCropType");

            migrationBuilder.DropForeignKey(
                name: "FK_PreviousCropType_Crops_CropId_CropStaticDataVersionId",
                table: "PreviousCropType");

            migrationBuilder.DropForeignKey(
                name: "FK_PreviousCropType_CropTypes_CropTypeId_CropTypeStaticDataVer~",
                table: "PreviousCropType");

            migrationBuilder.DropForeignKey(
                name: "FK_PrevManureApplicationYears_StaticDataVersions_StaticDataVer~",
                table: "PrevManureApplicationYears");

            migrationBuilder.DropForeignKey(
                name: "FK_PrevYearManureApplicationNitrogenDefaults_StaticDataVersion~",
                table: "PrevYearManureApplicationNitrogenDefaults");

            migrationBuilder.DropForeignKey(
                name: "FK_PrevYearManureApplicationNitrogenDefaults_PrevManureApplica~",
                table: "PrevYearManureApplicationNitrogenDefaults");

            migrationBuilder.DropForeignKey(
                name: "FK_Regions_StaticDataVersions_StaticDataVersionId",
                table: "Regions");

            migrationBuilder.DropForeignKey(
                name: "FK_RptCompletedFertilizerRequiredStdUnits_StaticDataVersions_S~",
                table: "RptCompletedFertilizerRequiredStdUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_RptCompletedManureRequiredStdUnits_StaticDataVersions_Stati~",
                table: "RptCompletedManureRequiredStdUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_SeasonApplications_StaticDataVersions_StaticDataVersionId",
                table: "SeasonApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_SoilTestMethods_StaticDataVersions_StaticDataVersionId",
                table: "SoilTestMethods");

            migrationBuilder.DropForeignKey(
                name: "FK_SoilTestPhosphorousKelownaRanges_StaticDataVersions_StaticD~",
                table: "SoilTestPhosphorousKelownaRanges");

            migrationBuilder.DropForeignKey(
                name: "FK_SoilTestPhosphorousRecommendation_StaticDataVersions_Static~",
                table: "SoilTestPhosphorousRecommendation");

            migrationBuilder.DropForeignKey(
                name: "FK_SoilTestPhosphorousRecommendation_SoilTestPhosphorousKelown~",
                table: "SoilTestPhosphorousRecommendation");

            migrationBuilder.DropForeignKey(
                name: "FK_SoilTestPhosphorusRanges_StaticDataVersions_StaticDataVersi~",
                table: "SoilTestPhosphorusRanges");

            migrationBuilder.DropForeignKey(
                name: "FK_SoilTestPotassiumKelownaRanges_StaticDataVersions_StaticDat~",
                table: "SoilTestPotassiumKelownaRanges");

            migrationBuilder.DropForeignKey(
                name: "FK_SoilTestPotassiumRanges_StaticDataVersions_StaticDataVersio~",
                table: "SoilTestPotassiumRanges");

            migrationBuilder.DropForeignKey(
                name: "FK_SoilTestPotassiumRecommendation_StaticDataVersions_StaticDa~",
                table: "SoilTestPotassiumRecommendation");

            migrationBuilder.DropForeignKey(
                name: "FK_SoilTestPotassiumRecommendation_SoilTestPotassiumKelownaRan~",
                table: "SoilTestPotassiumRecommendation");

            migrationBuilder.DropForeignKey(
                name: "FK_SolidMaterialApplicationTonPerAcreRateConversions_StaticDat~",
                table: "SolidMaterialApplicationTonPerAcreRateConversions");

            migrationBuilder.DropForeignKey(
                name: "FK_SolidMaterialsConversionFactors_StaticDataVersions_StaticDa~",
                table: "SolidMaterialsConversionFactors");

            migrationBuilder.DropForeignKey(
                name: "FK_SubRegion_StaticDataVersions_StaticDataVersionId",
                table: "SubRegion");

            migrationBuilder.DropForeignKey(
                name: "FK_SubRegion_Regions_RegionId_StaticDataVersionId",
                table: "SubRegion");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_StaticDataVersions_StaticDataVersionId",
                table: "Units");

            migrationBuilder.DropForeignKey(
                name: "FK_Yields_StaticDataVersions_StaticDataVersionId",
                table: "Yields");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Yields",
                table: "Yields");

            migrationBuilder.DropIndex(
                name: "IX_Yields_StaticDataVersionId",
                table: "Yields");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Units",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Units_StaticDataVersionId",
                table: "Units");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubRegion",
                table: "SubRegion");

            migrationBuilder.DropIndex(
                name: "IX_SubRegion_StaticDataVersionId",
                table: "SubRegion");

            migrationBuilder.DropIndex(
                name: "IX_SubRegion_RegionId_StaticDataVersionId",
                table: "SubRegion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SolidMaterialsConversionFactors",
                table: "SolidMaterialsConversionFactors");

            migrationBuilder.DropIndex(
                name: "IX_SolidMaterialsConversionFactors_StaticDataVersionId",
                table: "SolidMaterialsConversionFactors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SolidMaterialApplicationTonPerAcreRateConversions",
                table: "SolidMaterialApplicationTonPerAcreRateConversions");

            migrationBuilder.DropIndex(
                name: "IX_SolidMaterialApplicationTonPerAcreRateConversions_StaticDat~",
                table: "SolidMaterialApplicationTonPerAcreRateConversions");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_SoilTestPotassiumRecommendation_PotassiumCropGroupRegionCod~",
                table: "SoilTestPotassiumRecommendation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SoilTestPotassiumRecommendation",
                table: "SoilTestPotassiumRecommendation");

            migrationBuilder.DropIndex(
                name: "IX_SoilTestPotassiumRecommendation_StaticDataVersionId",
                table: "SoilTestPotassiumRecommendation");

            migrationBuilder.DropIndex(
                name: "IX_SoilTestPotassiumRecommendation_SoilTestPotassiumKelownaRan~",
                table: "SoilTestPotassiumRecommendation");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_SoilTestPotassiumRanges_StaticDataVersionId_UpperLimit",
                table: "SoilTestPotassiumRanges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SoilTestPotassiumRanges",
                table: "SoilTestPotassiumRanges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SoilTestPotassiumKelownaRanges",
                table: "SoilTestPotassiumKelownaRanges");

            migrationBuilder.DropIndex(
                name: "IX_SoilTestPotassiumKelownaRanges_StaticDataVersionId",
                table: "SoilTestPotassiumKelownaRanges");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_SoilTestPhosphorusRanges_StaticDataVersionId_UpperLimit",
                table: "SoilTestPhosphorusRanges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SoilTestPhosphorusRanges",
                table: "SoilTestPhosphorusRanges");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_SoilTestPhosphorousRecommendation_PhosphorousCropGroupRegio~",
                table: "SoilTestPhosphorousRecommendation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SoilTestPhosphorousRecommendation",
                table: "SoilTestPhosphorousRecommendation");

            migrationBuilder.DropIndex(
                name: "IX_SoilTestPhosphorousRecommendation_StaticDataVersionId",
                table: "SoilTestPhosphorousRecommendation");

            migrationBuilder.DropIndex(
                name: "IX_SoilTestPhosphorousRecommendation_SoilTestPhosphorousKelown~",
                table: "SoilTestPhosphorousRecommendation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SoilTestPhosphorousKelownaRanges",
                table: "SoilTestPhosphorousKelownaRanges");

            migrationBuilder.DropIndex(
                name: "IX_SoilTestPhosphorousKelownaRanges_StaticDataVersionId",
                table: "SoilTestPhosphorousKelownaRanges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SoilTestMethods",
                table: "SoilTestMethods");

            migrationBuilder.DropIndex(
                name: "IX_SoilTestMethods_StaticDataVersionId",
                table: "SoilTestMethods");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SeasonApplications",
                table: "SeasonApplications");

            migrationBuilder.DropIndex(
                name: "IX_SeasonApplications_StaticDataVersionId",
                table: "SeasonApplications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RptCompletedManureRequiredStdUnits",
                table: "RptCompletedManureRequiredStdUnits");

            migrationBuilder.DropIndex(
                name: "IX_RptCompletedManureRequiredStdUnits_StaticDataVersionId",
                table: "RptCompletedManureRequiredStdUnits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RptCompletedFertilizerRequiredStdUnits",
                table: "RptCompletedFertilizerRequiredStdUnits");

            migrationBuilder.DropIndex(
                name: "IX_RptCompletedFertilizerRequiredStdUnits_StaticDataVersionId",
                table: "RptCompletedFertilizerRequiredStdUnits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Regions",
                table: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Regions_StaticDataVersionId",
                table: "Regions");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_PrevYearManureApplicationNitrogenDefaults_FieldManureApplic~",
                table: "PrevYearManureApplicationNitrogenDefaults");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PrevYearManureApplicationNitrogenDefaults",
                table: "PrevYearManureApplicationNitrogenDefaults");

            migrationBuilder.DropIndex(
                name: "IX_PrevYearManureApplicationNitrogenDefaults_StaticDataVersion~",
                table: "PrevYearManureApplicationNitrogenDefaults");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_PrevManureApplicationYears_FieldManureApplicationHistory_St~",
                table: "PrevManureApplicationYears");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PrevManureApplicationYears",
                table: "PrevManureApplicationYears");

            migrationBuilder.DropIndex(
                name: "IX_PrevManureApplicationYears_StaticDataVersionId",
                table: "PrevManureApplicationYears");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PreviousCropType",
                table: "PreviousCropType");

            migrationBuilder.DropIndex(
                name: "IX_PreviousCropType_StaticDataVersionId",
                table: "PreviousCropType");

            migrationBuilder.DropIndex(
                name: "IX_PreviousCropType_CropId_CropStaticDataVersionId",
                table: "PreviousCropType");

            migrationBuilder.DropIndex(
                name: "IX_PreviousCropType_CropTypeId_CropTypeStaticDataVersionId",
                table: "PreviousCropType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PotassiumSoilTestRanges",
                table: "PotassiumSoilTestRanges");

            migrationBuilder.DropIndex(
                name: "IX_PotassiumSoilTestRanges_StaticDataVersionId",
                table: "PotassiumSoilTestRanges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PhosphorusSoilTestRanges",
                table: "PhosphorusSoilTestRanges");

            migrationBuilder.DropIndex(
                name: "IX_PhosphorusSoilTestRanges_StaticDataVersionId",
                table: "PhosphorusSoilTestRanges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NitrogenRecommendations",
                table: "NitrogenRecommendations");

            migrationBuilder.DropIndex(
                name: "IX_NitrogenRecommendations_StaticDataVersionId",
                table: "NitrogenRecommendations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NitrogenMineralizations",
                table: "NitrogenMineralizations");

            migrationBuilder.DropIndex(
                name: "IX_NitrogenMineralizations_LocationId",
                table: "NitrogenMineralizations");

            migrationBuilder.DropIndex(
                name: "IX_NitrogenMineralizations_StaticDataVersionId",
                table: "NitrogenMineralizations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NitrateCreditSampleDates",
                table: "NitrateCreditSampleDates");

            migrationBuilder.DropIndex(
                name: "IX_NitrateCreditSampleDates_StaticDataVersionId",
                table: "NitrateCreditSampleDates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Messages",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_StaticDataVersionId",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Manures",
                table: "Manures");

            migrationBuilder.DropIndex(
                name: "IX_Manures_StaticDataVersionId",
                table: "Manures");

            migrationBuilder.DropIndex(
                name: "IX_Manures_DryMatterId_StaticDataVersionId",
                table: "Manures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ManureImportedDefaults",
                table: "ManureImportedDefaults");

            migrationBuilder.DropIndex(
                name: "IX_ManureImportedDefaults_StaticDataVersionId",
                table: "ManureImportedDefaults");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LiquidSolidSeparationDefaults",
                table: "LiquidSolidSeparationDefaults");

            migrationBuilder.DropIndex(
                name: "IX_LiquidSolidSeparationDefaults_StaticDataVersionId",
                table: "LiquidSolidSeparationDefaults");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LiquidMaterialsConversionFactors",
                table: "LiquidMaterialsConversionFactors");

            migrationBuilder.DropIndex(
                name: "IX_LiquidMaterialsConversionFactors_StaticDataVersionId",
                table: "LiquidMaterialsConversionFactors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LiquidMaterialApplicationUsGallonsPerAcreRateConversions",
                table: "LiquidMaterialApplicationUsGallonsPerAcreRateConversions");

            migrationBuilder.DropIndex(
                name: "IX_LiquidMaterialApplicationUsGallonsPerAcreRateConversions_St~",
                table: "LiquidMaterialApplicationUsGallonsPerAcreRateConversions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LiquidFertilizerDensities",
                table: "LiquidFertilizerDensities");

            migrationBuilder.DropIndex(
                name: "IX_LiquidFertilizerDensities_StaticDataVersionId",
                table: "LiquidFertilizerDensities");

            migrationBuilder.DropIndex(
                name: "IX_LiquidFertilizerDensities_DensityUnitId_StaticDataVersionId",
                table: "LiquidFertilizerDensities");

            migrationBuilder.DropIndex(
                name: "IX_LiquidFertilizerDensities_FertilizerId_StaticDataVersionId",
                table: "LiquidFertilizerDensities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HarvestUnits",
                table: "HarvestUnits");

            migrationBuilder.DropIndex(
                name: "IX_HarvestUnits_StaticDataVersionId",
                table: "HarvestUnits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FertilizerUnits",
                table: "FertilizerUnits");

            migrationBuilder.DropIndex(
                name: "IX_FertilizerUnits_StaticDataVersionId",
                table: "FertilizerUnits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FertilizerTypes",
                table: "FertilizerTypes");

            migrationBuilder.DropIndex(
                name: "IX_FertilizerTypes_StaticDataVersionId",
                table: "FertilizerTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fertilizers",
                table: "Fertilizers");

            migrationBuilder.DropIndex(
                name: "IX_Fertilizers_StaticDataVersionId",
                table: "Fertilizers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FertilizerMethods",
                table: "FertilizerMethods");

            migrationBuilder.DropIndex(
                name: "IX_FertilizerMethods_StaticDataVersionId",
                table: "FertilizerMethods");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DryMatters",
                table: "DryMatters");

            migrationBuilder.DropIndex(
                name: "IX_DryMatters_StaticDataVersionId",
                table: "DryMatters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DensityUnits",
                table: "DensityUnits");

            migrationBuilder.DropIndex(
                name: "IX_DensityUnits_StaticDataVersionId",
                table: "DensityUnits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DefaultSoilTests",
                table: "DefaultSoilTests");

            migrationBuilder.DropIndex(
                name: "IX_DefaultSoilTests_StaticDataVersionId",
                table: "DefaultSoilTests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CropYields",
                table: "CropYields");

            migrationBuilder.DropIndex(
                name: "IX_CropYields_StaticDataVersionId",
                table: "CropYields");

            migrationBuilder.DropIndex(
                name: "IX_CropYields_CropId_StaticDataVersionId",
                table: "CropYields");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CropTypes",
                table: "CropTypes");

            migrationBuilder.DropIndex(
                name: "IX_CropTypes_StaticDataVersionId",
                table: "CropTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CropSoilTestPotassiumRegions",
                table: "CropSoilTestPotassiumRegions");

            migrationBuilder.DropIndex(
                name: "IX_CropSoilTestPotassiumRegions_StaticDataVersionId",
                table: "CropSoilTestPotassiumRegions");

            migrationBuilder.DropIndex(
                name: "IX_CropSoilTestPotassiumRegions_CropId_StaticDataVersionId",
                table: "CropSoilTestPotassiumRegions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CropSoilTestPhosphorousRegions",
                table: "CropSoilTestPhosphorousRegions");

            migrationBuilder.DropIndex(
                name: "IX_CropSoilTestPhosphorousRegions_StaticDataVersionId",
                table: "CropSoilTestPhosphorousRegions");

            migrationBuilder.DropIndex(
                name: "IX_CropSoilTestPhosphorousRegions_CropId_StaticDataVersionId",
                table: "CropSoilTestPhosphorousRegions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Crops",
                table: "Crops");

            migrationBuilder.DropIndex(
                name: "IX_Crops_StaticDataVersionId",
                table: "Crops");

            migrationBuilder.DropIndex(
                name: "IX_Crops_CropTypeId_StaticDataVersionId",
                table: "Crops");

            migrationBuilder.DropIndex(
                name: "IX_Crops_ManureApplicationHistory_StaticDataVersionId",
                table: "Crops");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConversionFactors",
                table: "ConversionFactors");

            migrationBuilder.DropIndex(
                name: "IX_ConversionFactors_StaticDataVersionId",
                table: "ConversionFactors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Breed",
                table: "Breed");

            migrationBuilder.DropIndex(
                name: "IX_Breed_StaticDataVersionId",
                table: "Breed");

            migrationBuilder.DropIndex(
                name: "IX_Breed_AnimalId_StaticDataVersionId",
                table: "Breed");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BCSampleDateForNitrateCredit",
                table: "BCSampleDateForNitrateCredit");

            migrationBuilder.DropIndex(
                name: "IX_BCSampleDateForNitrateCredit_StaticDataVersionId",
                table: "BCSampleDateForNitrateCredit");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnimalSubType",
                table: "AnimalSubType");

            migrationBuilder.DropIndex(
                name: "IX_AnimalSubType_StaticDataVersionId",
                table: "AnimalSubType");

            migrationBuilder.DropIndex(
                name: "IX_AnimalSubType_AnimalId_StaticDataVersionId",
                table: "AnimalSubType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Animals",
                table: "Animals");

            migrationBuilder.DropIndex(
                name: "IX_Animals_StaticDataVersionId",
                table: "Animals");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AmmoniaRetentions_DryMatter_SeasonApplicationId_StaticDataV~",
                table: "AmmoniaRetentions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AmmoniaRetentions",
                table: "AmmoniaRetentions");

            migrationBuilder.DropIndex(
                name: "IX_AmmoniaRetentions_StaticDataVersionId",
                table: "AmmoniaRetentions");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "Yields");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "SubRegion");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "SolidMaterialsConversionFactors");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "SolidMaterialApplicationTonPerAcreRateConversions");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "SoilTestPotassiumRecommendation");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "SoilTestPotassiumRanges");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "SoilTestPotassiumKelownaRanges");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "SoilTestPhosphorusRanges");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "SoilTestPhosphorousRecommendation");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "SoilTestPhosphorousKelownaRanges");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "SoilTestMethods");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "SeasonApplications");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "RptCompletedManureRequiredStdUnits");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "RptCompletedFertilizerRequiredStdUnits");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "PrevYearManureApplicationNitrogenDefaults");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "PrevManureApplicationYears");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "PreviousCropType");

            migrationBuilder.DropColumn(
                name: "CropStaticDataVersionId",
                table: "PreviousCropType");

            migrationBuilder.DropColumn(
                name: "CropTypeStaticDataVersionId",
                table: "PreviousCropType");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "PotassiumSoilTestRanges");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "PhosphorusSoilTestRanges");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "NitrogenRecommendations");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "NitrogenMineralizations");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "NitrateCreditSampleDates");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "Manures");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "ManureImportedDefaults");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "LiquidSolidSeparationDefaults");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "LiquidMaterialsConversionFactors");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "LiquidMaterialApplicationUsGallonsPerAcreRateConversions");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "LiquidFertilizerDensities");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "HarvestUnits");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "FertilizerUnits");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "FertilizerTypes");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "Fertilizers");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "FertilizerMethods");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "DryMatters");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "DensityUnits");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "DefaultSoilTests");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "CropYields");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "CropTypes");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "CropSoilTestPotassiumRegions");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "CropSoilTestPhosphorousRegions");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "Crops");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "ConversionFactors");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "Breed");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "BCSampleDateForNitrateCredit");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "AnimalSubType");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "StaticDataVersionId",
                table: "AmmoniaRetentions");

            migrationBuilder.RenameColumn(
                name: "DryMatterId",
                table: "Manures",
                newName: "DMId");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Yields",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Units",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "SubRegion",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "SolidMaterialsConversionFactors",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "SolidMaterialApplicationTonPerAcreRateConversions",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<int>(
                name: "UpperLimit",
                table: "SoilTestPotassiumRanges",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "SoilTestPotassiumKelownaRanges",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<int>(
                name: "UpperLimit",
                table: "SoilTestPhosphorusRanges",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "SoilTestPhosphorousKelownaRanges",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "SoilTestMethods",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "SeasonApplications",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "RptCompletedManureRequiredStdUnits",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "RptCompletedFertilizerRequiredStdUnits",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Regions",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "PreviousCropType",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "NitrogenRecommendations",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Messages",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Manures",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "NMineralizationId1",
                table: "Manures",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NMineralizationLocationId",
                table: "Manures",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ManureImportedDefaults",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "LiquidSolidSeparationDefaults",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "LiquidMaterialsConversionFactors",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "LiquidMaterialApplicationUsGallonsPerAcreRateConversions",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "LiquidFertilizerDensities",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "HarvestUnits",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "FertilizerUnits",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "FertilizerTypes",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Fertilizers",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "FertilizerMethods",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "DryMatters",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "DensityUnits",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "DefaultSoilTests",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "CropTypes",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Crops",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ConversionFactors",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Breed",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "AnimalSubType",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Animals",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Yields",
                table: "Yields",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Units",
                table: "Units",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubRegion",
                table: "SubRegion",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SolidMaterialsConversionFactors",
                table: "SolidMaterialsConversionFactors",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SolidMaterialApplicationTonPerAcreRateConversions",
                table: "SolidMaterialApplicationTonPerAcreRateConversions",
                column: "Id");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_SoilTestPotassiumRecommendation_PotassiumCropGroupRegionCod~",
                table: "SoilTestPotassiumRecommendation",
                columns: new[] { "PotassiumCropGroupRegionCode", "SoilTestPotassiumKelownaRangeId", "SoilTestPotassiumRegionCode" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SoilTestPotassiumRecommendation",
                table: "SoilTestPotassiumRecommendation",
                columns: new[] { "SoilTestPotassiumKelownaRangeId", "SoilTestPotassiumRegionCode", "PotassiumCropGroupRegionCode" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SoilTestPotassiumRanges",
                table: "SoilTestPotassiumRanges",
                column: "UpperLimit");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SoilTestPotassiumKelownaRanges",
                table: "SoilTestPotassiumKelownaRanges",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SoilTestPhosphorusRanges",
                table: "SoilTestPhosphorusRanges",
                column: "UpperLimit");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_SoilTestPhosphorousRecommendation_PhosphorousCropGroupRegio~",
                table: "SoilTestPhosphorousRecommendation",
                columns: new[] { "PhosphorousCropGroupRegionCode", "SoilTestPhosphorousKelownaRangeId", "SoilTestPhosphorousRegionCode" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SoilTestPhosphorousRecommendation",
                table: "SoilTestPhosphorousRecommendation",
                columns: new[] { "SoilTestPhosphorousKelownaRangeId", "SoilTestPhosphorousRegionCode", "PhosphorousCropGroupRegionCode" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SoilTestPhosphorousKelownaRanges",
                table: "SoilTestPhosphorousKelownaRanges",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SoilTestMethods",
                table: "SoilTestMethods",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SeasonApplications",
                table: "SeasonApplications",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RptCompletedManureRequiredStdUnits",
                table: "RptCompletedManureRequiredStdUnits",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RptCompletedFertilizerRequiredStdUnits",
                table: "RptCompletedFertilizerRequiredStdUnits",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Regions",
                table: "Regions",
                column: "Id");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_PrevYearManureApplicationNitrogenDefaults_FieldManureApplic~",
                table: "PrevYearManureApplicationNitrogenDefaults",
                column: "FieldManureApplicationHistory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PrevYearManureApplicationNitrogenDefaults",
                table: "PrevYearManureApplicationNitrogenDefaults",
                column: "Id");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_PrevManureApplicationYears_FieldManureApplicationHistory",
                table: "PrevManureApplicationYears",
                column: "FieldManureApplicationHistory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PrevManureApplicationYears",
                table: "PrevManureApplicationYears",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PreviousCropType",
                table: "PreviousCropType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PotassiumSoilTestRanges",
                table: "PotassiumSoilTestRanges",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PhosphorusSoilTestRanges",
                table: "PhosphorusSoilTestRanges",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NitrogenRecommendations",
                table: "NitrogenRecommendations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NitrogenMineralizations",
                table: "NitrogenMineralizations",
                columns: new[] { "Id", "LocationId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_NitrateCreditSampleDates",
                table: "NitrateCreditSampleDates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Messages",
                table: "Messages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Manures",
                table: "Manures",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ManureImportedDefaults",
                table: "ManureImportedDefaults",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LiquidSolidSeparationDefaults",
                table: "LiquidSolidSeparationDefaults",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LiquidMaterialsConversionFactors",
                table: "LiquidMaterialsConversionFactors",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LiquidMaterialApplicationUsGallonsPerAcreRateConversions",
                table: "LiquidMaterialApplicationUsGallonsPerAcreRateConversions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LiquidFertilizerDensities",
                table: "LiquidFertilizerDensities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HarvestUnits",
                table: "HarvestUnits",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FertilizerUnits",
                table: "FertilizerUnits",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FertilizerTypes",
                table: "FertilizerTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fertilizers",
                table: "Fertilizers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FertilizerMethods",
                table: "FertilizerMethods",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DryMatters",
                table: "DryMatters",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DensityUnits",
                table: "DensityUnits",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DefaultSoilTests",
                table: "DefaultSoilTests",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CropYields",
                table: "CropYields",
                columns: new[] { "CropId", "LocationId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CropTypes",
                table: "CropTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CropSoilTestPotassiumRegions",
                table: "CropSoilTestPotassiumRegions",
                columns: new[] { "CropId", "SoilTestPotassiumRegionCode" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CropSoilTestPhosphorousRegions",
                table: "CropSoilTestPhosphorousRegions",
                columns: new[] { "CropId", "SoilTestPhosphorousRegionCode" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Crops",
                table: "Crops",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConversionFactors",
                table: "ConversionFactors",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Breed",
                table: "Breed",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BCSampleDateForNitrateCredit",
                table: "BCSampleDateForNitrateCredit",
                column: "CoastalFromDateMonth");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnimalSubType",
                table: "AnimalSubType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Animals",
                table: "Animals",
                column: "Id");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AmmoniaRetentions_DryMatter_SeasonApplicationId",
                table: "AmmoniaRetentions",
                columns: new[] { "DryMatter", "SeasonApplicationId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AmmoniaRetentions",
                table: "AmmoniaRetentions",
                columns: new[] { "SeasonApplicationId", "DryMatter" });

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

            migrationBuilder.CreateIndex(
                name: "IX_SubRegion_RegionId",
                table: "SubRegion",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_PreviousCropType_CropId",
                table: "PreviousCropType",
                column: "CropId");

            migrationBuilder.CreateIndex(
                name: "IX_PreviousCropType_CropTypeId",
                table: "PreviousCropType",
                column: "CropTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Manures_DMId",
                table: "Manures",
                column: "DMId");

            migrationBuilder.CreateIndex(
                name: "IX_Manures_NMineralizationId1_NMineralizationLocationId",
                table: "Manures",
                columns: new[] { "NMineralizationId1", "NMineralizationLocationId" });

            migrationBuilder.CreateIndex(
                name: "IX_LiquidFertilizerDensities_DensityUnitId",
                table: "LiquidFertilizerDensities",
                column: "DensityUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_LiquidFertilizerDensities_FertilizerId",
                table: "LiquidFertilizerDensities",
                column: "FertilizerId");

            migrationBuilder.CreateIndex(
                name: "IX_Crops_CropTypeId",
                table: "Crops",
                column: "CropTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Crops_ManureApplicationHistory",
                table: "Crops",
                column: "ManureApplicationHistory");

            migrationBuilder.CreateIndex(
                name: "IX_Breed_AnimalId",
                table: "Breed",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalSubType_AnimalId",
                table: "AnimalSubType",
                column: "AnimalId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimalSubType_Animals_AnimalId",
                table: "AnimalSubType",
                column: "AnimalId",
                principalTable: "Animals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Breed_Animals_AnimalId",
                table: "Breed",
                column: "AnimalId",
                principalTable: "Animals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Crops_CropTypes_CropTypeId",
                table: "Crops",
                column: "CropTypeId",
                principalTable: "CropTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Crops_PrevYearManureApplicationNitrogenDefaults_ManureAppli~",
                table: "Crops",
                column: "ManureApplicationHistory",
                principalTable: "PrevYearManureApplicationNitrogenDefaults",
                principalColumn: "FieldManureApplicationHistory",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CropSoilTestPhosphorousRegions_Crops_CropId",
                table: "CropSoilTestPhosphorousRegions",
                column: "CropId",
                principalTable: "Crops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CropSoilTestPotassiumRegions_Crops_CropId",
                table: "CropSoilTestPotassiumRegions",
                column: "CropId",
                principalTable: "Crops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CropYields_Crops_CropId",
                table: "CropYields",
                column: "CropId",
                principalTable: "Crops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LiquidFertilizerDensities_DensityUnits_DensityUnitId",
                table: "LiquidFertilizerDensities",
                column: "DensityUnitId",
                principalTable: "DensityUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LiquidFertilizerDensities_Fertilizers_FertilizerId",
                table: "LiquidFertilizerDensities",
                column: "FertilizerId",
                principalTable: "Fertilizers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Manures_DryMatters_DMId",
                table: "Manures",
                column: "DMId",
                principalTable: "DryMatters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Manures_NitrogenMineralizations_NMineralizationId1_NMineral~",
                table: "Manures",
                columns: new[] { "NMineralizationId1", "NMineralizationLocationId" },
                principalTable: "NitrogenMineralizations",
                principalColumns: new[] { "Id", "LocationId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PreviousCropType_Crops_CropId",
                table: "PreviousCropType",
                column: "CropId",
                principalTable: "Crops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PreviousCropType_CropTypes_CropTypeId",
                table: "PreviousCropType",
                column: "CropTypeId",
                principalTable: "CropTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PrevYearManureApplicationNitrogenDefaults_PrevManureApplica~",
                table: "PrevYearManureApplicationNitrogenDefaults",
                column: "FieldManureApplicationHistory",
                principalTable: "PrevManureApplicationYears",
                principalColumn: "FieldManureApplicationHistory",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SoilTestPhosphorousRecommendation_SoilTestPhosphorousKelown~",
                table: "SoilTestPhosphorousRecommendation",
                column: "SoilTestPhosphorousKelownaRangeId",
                principalTable: "SoilTestPhosphorousKelownaRanges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SoilTestPotassiumRecommendation_SoilTestPotassiumKelownaRan~",
                table: "SoilTestPotassiumRecommendation",
                column: "SoilTestPotassiumKelownaRangeId",
                principalTable: "SoilTestPotassiumKelownaRanges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubRegion_Regions_RegionId",
                table: "SubRegion",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
