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

            Assert.Equal(SerializeToString(agriConfigurationDb.AmmoniaRetentions.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.AmmoniaRetentions.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.Animals.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.Animals.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.Animals.SelectMany(a => a.AnimalSubTypes).Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.Animals.SelectMany(a => a.AnimalSubTypes).Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.BCSampleDateForNitrateCredit.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.BCSampleDateForNitrateCredit.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.Breed.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.Breed.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.ConversionFactors.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.ConversionFactors.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.Crops.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.Crops.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.CropSoilTestPhosphorousRegions.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.CropSoilTestPhosphorousRegions.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.CropSoilTestPotassiumRegions.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.CropSoilTestPotassiumRegions.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.CropTypes.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.CropTypes.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.CropYields.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.CropYields.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.DefaultSoilTests.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.DefaultSoilTests.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.DensityUnits.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.DensityUnits.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.DryMatters.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.DryMatters.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.Fertilizers.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.Fertilizers.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.FertilizerMethods.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.FertilizerMethods.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.FertilizerTypes.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.FertilizerTypes.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.FertilizerUnits.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.FertilizerUnits.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.HarvestUnits.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.HarvestUnits.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.LiquidFertilizerDensities.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.LiquidFertilizerDensities.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.LiquidMaterialApplicationUsGallonsPerAcreRateConversions.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.LiquidMaterialApplicationUsGallonsPerAcreRateConversions.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.LiquidMaterialsConversionFactors.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.LiquidMaterialsConversionFactors.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.LiquidSolidSeparationDefaults.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.LiquidSolidSeparationDefaults.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.ManureImportedDefaults.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.ManureImportedDefaults.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.Manures.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.Manures.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.Messages.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.Messages.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.NitrateCreditSampleDates.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.NitrateCreditSampleDates.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.NitrogenMineralizations.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.NitrogenMineralizations.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.NitrogenRecommendations.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.NitrogenRecommendations.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.PhosphorusSoilTestRanges.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.PhosphorusSoilTestRanges.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.PotassiumSoilTestRanges.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.PotassiumSoilTestRanges.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.Crops.SelectMany(c => c.PreviousCropTypes).Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.Crops.SelectMany(c => c.PreviousCropTypes).Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.PrevManureApplicationYears.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.PrevManureApplicationYears.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.PrevYearManureApplicationNitrogenDefaults.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.PrevYearManureApplicationNitrogenDefaults.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.Regions.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.Regions.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.RptCompletedFertilizerRequiredStdUnits.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.RptCompletedFertilizerRequiredStdUnits.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.RptCompletedManureRequiredStdUnits.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.RptCompletedManureRequiredStdUnits.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.SeasonApplications.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.SeasonApplications.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.SoilTestMethods.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.SoilTestMethods.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.SoilTestPhosphorusRanges.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.SoilTestPhosphorusRanges.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.SoilTestPhosphorousKelownaRanges.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.SoilTestPhosphorousKelownaRanges.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.SoilTestPhosphorousKelownaRanges
                    .SelectMany(s => s.SoilTestPhosphorousRecommendations)
                    .Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.SoilTestPhosphorousKelownaRanges
                    .SelectMany(s => s.SoilTestPhosphorousRecommendations)
                    .Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.SoilTestPotassiumRanges.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.SoilTestPotassiumRanges.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.SoilTestPotassiumKelownaRanges.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.SoilTestPotassiumKelownaRanges.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.SoilTestPotassiumKelownaRanges
                    .SelectMany(s => s.SoilTestPotassiumRecommendations)
                    .Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.SoilTestPotassiumKelownaRanges
                    .SelectMany(s => s.SoilTestPotassiumRecommendations)
                    .Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.SolidMaterialApplicationTonPerAcreRateConversions.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.SolidMaterialApplicationTonPerAcreRateConversions.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.SolidMaterialsConversionFactors.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.SolidMaterialsConversionFactors.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.Units.Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.Units.Where(a => a.StaticDataVersionId == 2).ToList()));

            Assert.Equal(SerializeToString(agriConfigurationDb.Regions.SelectMany(r => r.SubRegions).Where(a => a.StaticDataVersionId == 1).ToList()),
                SerializeToString(agriConfigurationDb.Regions.SelectMany(r => r.SubRegions).Where(a => a.StaticDataVersionId == 2).ToList()));

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