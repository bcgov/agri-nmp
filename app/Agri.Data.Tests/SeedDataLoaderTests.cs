using System;
using System.Collections.Generic;
using System.Linq;
using Agri.Models.Configuration;
using Agri.Tests.Shared;
using Shouldly;
using Xunit;

using Xunit.Abstractions;

namespace Agri.Data.Tests
{
    public class SeedDataLoaderTests : TestBase
    {
        public SeedDataLoaderTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void GetSeedJsonData_Should_Load_BrowserData_Json()
        {
            var result = SeedDataLoader.GetSeedJsonData<List<Browser>>(Constants.SeedDataFiles.Browsers);

            result.ShouldNotBeNull();
            result.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public void GetSeedJsonData_Should_Load_ExternalLinks_Json()
        {
            var result = SeedDataLoader.GetSeedJsonData<List<ExternalLink>>(Constants.SeedDataFiles.ExternalLinks);

            result.ShouldNotBeNull();
            result.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public void GetSeedJsonData_Should_Load_Locations_Json()
        {
            var result = SeedDataLoader.GetSeedJsonData<List<Location>>(Constants.SeedDataFiles.Location);

            result.ShouldNotBeNull();
            result.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public void GetSeedJsonData_Should_Load_Journies_Json()
        {
            var result = SeedDataLoader.GetSeedJsonData<List<Journey>>(Constants.SeedDataFiles.Journey);

            result.ShouldNotBeNull();
            result.Count.ShouldBe(6);
            result.SelectMany(j => j.MainMenus).Count().ShouldBe(26);
            result.SelectMany(j => j.MainMenus).SelectMany(m => m.SubMenus).Count().ShouldBe(28);
        }

        [Fact]
        public void GetSeedJsonData_Should_Load_NutrientIcons_Json()
        {
            var result = SeedDataLoader.GetSeedJsonData<List<NutrientIcon>>(Constants.SeedDataFiles.NutrientIcons);

            result.ShouldNotBeNull();
            result.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public void GetSeedJsonData_Should_Load_UserPrompts_Json()
        {
            var result = SeedDataLoader.GetSeedJsonData<List<UserPrompt>>(Constants.SeedDataFiles.UserPrompts);

            result.ShouldNotBeNull();
            result.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public void SeedData_Should_Load_Entire_Database()
        {
            var repo = new AgriConfigurationRepository(agriConfigurationDb, Mapper);
            var seeder = new AgriSeeder(agriConfigurationDb, repo);
            seeder.Seed();

            agriConfigurationDb.Browsers.Any().ShouldBeTrue();
            agriConfigurationDb.ExternalLinks.Any().ShouldBeTrue();
            agriConfigurationDb.Locations.Any().ShouldBeTrue();
            agriConfigurationDb.MainMenus.Any().ShouldBeTrue();
            agriConfigurationDb.MainMenus.Any(m => m.SubMenus.Count > 0).ShouldBeTrue();
            agriConfigurationDb.NutrientIcons.Any().ShouldBeTrue();
            agriConfigurationDb.UserPrompts.Any().ShouldBeTrue();

            agriConfigurationDb.AmmoniaRetentions.Any().ShouldBeTrue();
            agriConfigurationDb.Animals.Any().ShouldBeTrue();
            agriConfigurationDb.Animals.SelectMany(a => a.AnimalSubTypes).Any().ShouldBeTrue();
            agriConfigurationDb.BCSampleDateForNitrateCredit.Any().ShouldBeTrue();
            agriConfigurationDb.Breed.Any().ShouldBeTrue();
            agriConfigurationDb.ConversionFactors.Any().ShouldBeTrue();
            agriConfigurationDb.Crops.Any().ShouldBeTrue();
            agriConfigurationDb.CropSoilTestPhosphorousRegions.Any().ShouldBeTrue();
            agriConfigurationDb.CropSoilTestPotassiumRegions.Any().ShouldBeTrue();
            agriConfigurationDb.CropTypes.Any().ShouldBeTrue();
            agriConfigurationDb.CropYields.Any().ShouldBeTrue();
            agriConfigurationDb.DefaultSoilTests.Any().ShouldBeTrue();
            agriConfigurationDb.DensityUnits.Any().ShouldBeTrue();
            agriConfigurationDb.DryMatters.Any().ShouldBeTrue();
            agriConfigurationDb.Fertilizers.Any().ShouldBeTrue();
            agriConfigurationDb.FertilizerMethods.Any().ShouldBeTrue();
            agriConfigurationDb.FertilizerTypes.Any().ShouldBeTrue();
            agriConfigurationDb.FertilizerUnits.Any().ShouldBeTrue();
            agriConfigurationDb.HarvestUnits.Any().ShouldBeTrue();
            agriConfigurationDb.LiquidFertilizerDensities.Any().ShouldBeTrue();
            agriConfigurationDb.LiquidMaterialApplicationUsGallonsPerAcreRateConversions.Any().ShouldBeTrue();
            agriConfigurationDb.LiquidMaterialsConversionFactors.Any().ShouldBeTrue();
            agriConfigurationDb.LiquidSolidSeparationDefaults.Any().ShouldBeTrue();
            agriConfigurationDb.ManureImportedDefaults.Any().ShouldBeTrue();
            agriConfigurationDb.Manures.Any().ShouldBeTrue();
            agriConfigurationDb.Messages.Any().ShouldBeTrue();
            agriConfigurationDb.NitrateCreditSampleDates.Any().ShouldBeTrue();
            agriConfigurationDb.NitrogenMineralizations.Any().ShouldBeTrue();
            agriConfigurationDb.NitrogenRecommendations.Any().ShouldBeTrue();
            agriConfigurationDb.PhosphorusSoilTestRanges.Any().ShouldBeTrue();
            agriConfigurationDb.PotassiumSoilTestRanges.Any().ShouldBeTrue();
            agriConfigurationDb.Crops.SelectMany(c => c.PreviousCropTypes).Any().ShouldBeTrue();
            agriConfigurationDb.PrevManureApplicationYears.Any().ShouldBeTrue();
            agriConfigurationDb.PrevYearManureApplicationNitrogenDefaults.Any().ShouldBeTrue();
            agriConfigurationDb.Regions.Any().ShouldBeTrue();
            agriConfigurationDb.RptCompletedFertilizerRequiredStdUnits.Any().ShouldBeTrue();
            agriConfigurationDb.RptCompletedManureRequiredStdUnits.Any().ShouldBeTrue();
            agriConfigurationDb.SeasonApplications.Any().ShouldBeTrue();
            agriConfigurationDb.SoilTestMethods.Any().ShouldBeTrue();
            agriConfigurationDb.SoilTestPhosphorusRanges.Any().ShouldBeTrue();
            agriConfigurationDb.SoilTestPhosphorousKelownaRanges.Any().ShouldBeTrue();
            agriConfigurationDb.SoilTestPhosphorousKelownaRanges
                    .SelectMany(s => s.SoilTestPhosphorousRecommendations).Any().ShouldBeTrue();
            agriConfigurationDb.SoilTestPotassiumRanges.Any().ShouldBeTrue();
            agriConfigurationDb.SoilTestPotassiumKelownaRanges.Any().ShouldBeTrue();
            agriConfigurationDb.SoilTestPotassiumKelownaRanges.Any().ShouldBeTrue();
            agriConfigurationDb.SolidMaterialApplicationTonPerAcreRateConversions.Any().ShouldBeTrue();
            agriConfigurationDb.SolidMaterialsConversionFactors.Any().ShouldBeTrue();
            agriConfigurationDb.Units.Any().ShouldBeTrue();
            agriConfigurationDb.Regions.SelectMany(r => r.SubRegions).Any().ShouldBeTrue();
            agriConfigurationDb.Yields.Any().ShouldBeTrue();
        }
    }
}