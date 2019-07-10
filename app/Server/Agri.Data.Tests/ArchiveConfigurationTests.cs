using Agri.Models.Security;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using Xunit;
using Xunit.Abstractions;

namespace Agri.Data.Tests
{
    public class ArchiveConfigurationTests : TestBase
    {
        public ArchiveConfigurationTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ArchiveConfigurationsSuccessfully()
        {
            var user = new ManageVersionUser
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName"
            };
            var repo = new AgriConfigurationRepository(agriConfigurationDb, Mapper);

            repo.ArchiveConfigurations(user);

            Assert.True(agriConfigurationDb.AmmoniaRetentions.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.AmmoniaRetentions.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.AmmoniaRetentions.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.AmmoniaRetentions.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.Animals.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.Animals.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.Animals.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.Animals.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.Animals.SelectMany(a => a.AnimalSubTypes).Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.Animals.SelectMany(a => a.AnimalSubTypes).Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.Animals.SelectMany(a => a.AnimalSubTypes).Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.Animals.SelectMany(a => a.AnimalSubTypes).Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.BCSampleDateForNitrateCredit.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.BCSampleDateForNitrateCredit.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.BCSampleDateForNitrateCredit.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.BCSampleDateForNitrateCredit.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.Breed.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.Breed.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.Breed.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.Breed.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.ConversionFactors.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.ConversionFactors.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.ConversionFactors.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.ConversionFactors.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.Crops.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.Crops.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.Crops.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.Crops.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.CropSoilTestPhosphorousRegions.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.CropSoilTestPhosphorousRegions.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.CropSoilTestPhosphorousRegions.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.CropSoilTestPhosphorousRegions.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.CropSoilTestPotassiumRegions.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.CropSoilTestPotassiumRegions.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.CropSoilTestPotassiumRegions.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.CropSoilTestPotassiumRegions.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.CropTypes.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.CropTypes.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.CropTypes.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.CropTypes.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.CropYields.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.CropYields.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.CropYields.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.CropYields.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.DefaultSoilTests.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.DefaultSoilTests.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.DefaultSoilTests.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.DefaultSoilTests.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.DensityUnits.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.DensityUnits.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.DensityUnits.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.DensityUnits.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.DryMatters.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.DryMatters.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.DryMatters.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.DryMatters.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.Fertilizers.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.Fertilizers.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.Fertilizers.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.Fertilizers.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.FertilizerMethods.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.FertilizerMethods.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.FertilizerMethods.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.FertilizerMethods.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.FertilizerTypes.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.FertilizerTypes.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.FertilizerTypes.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.FertilizerTypes.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.FertilizerUnits.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.FertilizerUnits.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.FertilizerUnits.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.FertilizerUnits.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.HarvestUnits.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.HarvestUnits.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.HarvestUnits.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.HarvestUnits.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.LiquidFertilizerDensities.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.LiquidFertilizerDensities.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.LiquidFertilizerDensities.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.LiquidFertilizerDensities.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.LiquidMaterialApplicationUsGallonsPerAcreRateConversions.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.LiquidMaterialApplicationUsGallonsPerAcreRateConversions.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.LiquidMaterialApplicationUsGallonsPerAcreRateConversions.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.LiquidMaterialApplicationUsGallonsPerAcreRateConversions.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.LiquidMaterialsConversionFactors.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.LiquidMaterialsConversionFactors.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.LiquidMaterialsConversionFactors.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.LiquidMaterialsConversionFactors.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.LiquidSolidSeparationDefaults.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.LiquidSolidSeparationDefaults.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.LiquidSolidSeparationDefaults.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.LiquidSolidSeparationDefaults.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.ManureImportedDefaults.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.ManureImportedDefaults.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.ManureImportedDefaults.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.ManureImportedDefaults.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.Manures.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.Manures.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.Manures.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.Manures.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.Messages.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.Messages.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.Messages.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.Messages.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.NitrateCreditSampleDates.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.NitrateCreditSampleDates.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.NitrateCreditSampleDates.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.NitrateCreditSampleDates.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.NitrogenMineralizations.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.NitrogenMineralizations.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.NitrogenMineralizations.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.NitrogenMineralizations.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.NitrogenRecommendations.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.NitrogenRecommendations.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.NitrogenRecommendations.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.NitrogenRecommendations.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.PhosphorusSoilTestRanges.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.PhosphorusSoilTestRanges.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.PhosphorusSoilTestRanges.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.PhosphorusSoilTestRanges.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.PotassiumSoilTestRanges.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.PotassiumSoilTestRanges.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.PotassiumSoilTestRanges.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.PotassiumSoilTestRanges.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.Crops.SelectMany(c => c.PreviousCropTypes).Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.Crops.SelectMany(c => c.PreviousCropTypes).Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.Crops.SelectMany(c => c.PreviousCropTypes).Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.Crops.SelectMany(c => c.PreviousCropTypes).Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.PrevManureApplicationYears.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.PrevManureApplicationYears.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.PrevManureApplicationYears.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.PrevManureApplicationYears.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.PrevYearManureApplicationNitrogenDefaults.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.PrevYearManureApplicationNitrogenDefaults.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.PrevYearManureApplicationNitrogenDefaults.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.PrevYearManureApplicationNitrogenDefaults.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.Regions.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.Regions.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.Regions.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.Regions.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.RptCompletedFertilizerRequiredStdUnits.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.RptCompletedFertilizerRequiredStdUnits.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.RptCompletedFertilizerRequiredStdUnits.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.RptCompletedFertilizerRequiredStdUnits.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.RptCompletedManureRequiredStdUnits.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.RptCompletedManureRequiredStdUnits.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.RptCompletedManureRequiredStdUnits.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.RptCompletedManureRequiredStdUnits.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.SeasonApplications.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.SeasonApplications.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.SeasonApplications.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.SeasonApplications.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.SoilTestMethods.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.SoilTestMethods.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.SoilTestMethods.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.SoilTestMethods.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.SoilTestPhosphorusRanges.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.SoilTestPhosphorusRanges.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.SoilTestPhosphorusRanges.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.SoilTestPhosphorusRanges.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.SoilTestPhosphorousKelownaRanges.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.SoilTestPhosphorousKelownaRanges.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.SoilTestPhosphorousKelownaRanges.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.SoilTestPhosphorousKelownaRanges.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.SoilTestPhosphorousKelownaRanges
                    .SelectMany(s => s.SoilTestPhosphorousRecommendations).Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.SoilTestPhosphorousKelownaRanges
                    .SelectMany(s => s.SoilTestPhosphorousRecommendations).Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.SoilTestPhosphorousKelownaRanges
                    .SelectMany(s => s.SoilTestPhosphorousRecommendations)
                    .Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.SoilTestPhosphorousKelownaRanges
                    .SelectMany(s => s.SoilTestPhosphorousRecommendations)
                    .Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.SoilTestPotassiumRanges.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.SoilTestPotassiumRanges.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.SoilTestPotassiumRanges.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.SoilTestPotassiumRanges.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.SoilTestPotassiumKelownaRanges.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.SoilTestPotassiumKelownaRanges.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.SoilTestPotassiumKelownaRanges.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.SoilTestPotassiumKelownaRanges.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.SoilTestPotassiumKelownaRanges.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.SoilTestPotassiumKelownaRanges.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.SoilTestPotassiumKelownaRanges
                    .SelectMany(s => s.SoilTestPotassiumRecommendations)
                    .Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.SoilTestPotassiumKelownaRanges
                    .SelectMany(s => s.SoilTestPotassiumRecommendations)
                    .Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.SolidMaterialApplicationTonPerAcreRateConversions.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.SolidMaterialApplicationTonPerAcreRateConversions.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.SolidMaterialApplicationTonPerAcreRateConversions.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.SolidMaterialApplicationTonPerAcreRateConversions.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.SolidMaterialsConversionFactors.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.SolidMaterialsConversionFactors.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.SolidMaterialsConversionFactors.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.SolidMaterialsConversionFactors.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.Units.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.Units.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.Units.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.Units.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.Regions.SelectMany(r => r.SubRegions).Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.Regions.SelectMany(r => r.SubRegions).Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.Regions.SelectMany(r => r.SubRegions).Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.Regions.SelectMany(r => r.SubRegions).Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.True(agriConfigurationDb.Yields.Any(a => a.StaticDataVersionId == 1));
            Assert.True(agriConfigurationDb.Yields.Any(a => a.StaticDataVersionId == 2));
            Assert.Equal(SerializeToString(agriConfigurationDb.Yields.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.Yields.Where(a => a.StaticDataVersionId == 2).ToList()));
        }

        private string SerializeToString<T>(T entity)
        {
            var result = string.Empty;
            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                serializer.WriteObject(stream, entity);

                stream.Position = 0;
                var sr = new StreamReader(stream);
                result = sr.ReadToEnd();
            }

            return result;
        }
    }
}