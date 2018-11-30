using Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;
using Agri.Interfaces;
using Agri.LegacyData.Models.Impl;

namespace Agri.Data.TestHarness
{
    [TestClass]
    public class AgriConfigurationRepositoryTests
    {
        private ServiceProvider _serviceProvider;
        private IAgriConfigurationRepository _agriRepository;
        private IAgriConfigurationRepository _staticExtRepo;

        public AgriConfigurationRepositoryTests()
        {
            var connectionString = TestHelper.GetConnectionString(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            var services = new ServiceCollection();

            services.AddDbContext<AgriConfigurationContext>(options =>
            {
                options.UseNpgsql(connectionString, b => b.MigrationsAssembly("Agri.Data"));
            });

            services.AddTransient<AgriConfigurationRepository>();
            services.AddTransient<StaticDataExtRepository>();

            _serviceProvider = services.BuildServiceProvider();
            _agriRepository = _serviceProvider.GetRequiredService<AgriConfigurationRepository>();
            _staticExtRepo = _serviceProvider.GetRequiredService<StaticDataExtRepository>();
        }

        [TestMethod]
        public void CompareConvertYieldFromBushelToTonsPerAcre()
        {
            //Oats
            var actual = _agriRepository.ConvertYieldFromBushelToTonsPerAcre(68, 1);
            var expected = _staticExtRepo.ConvertYieldFromBushelToTonsPerAcre(68, 1);

            Assert.IsTrue(actual >= 0);
            Assert.AreEqual(actual, actual);
        }

        [TestMethod]
        public void CompareGetAllowableBrowsers()
        {
            var actual = _agriRepository.GetAllowableBrowsers();
            var expected = _staticExtRepo.GetAllowableBrowsers();

            Assert.IsTrue(actual.Count > 1);
            Assert.AreEqual(actual.Count, expected.Count);
        }

        [TestMethod]
        public void CompareGetAmmoniaRetention()
        {
            var actual = _agriRepository.GetAmmoniaRetention(1, 1);
            var expected = _staticExtRepo.GetAmmoniaRetention(1, 1);

            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.DryMatter, expected.DryMatter);
            Assert.AreEqual(actual.Value, expected.Value);
        }

        [TestMethod]
        public void CompareGetAmmoniaRetentionsn()
        {
            var actual = _agriRepository.GetAmmoniaRetentions();
            var expected = _staticExtRepo.GetAmmoniaRetentions();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(actual.Count, expected.Count);
        }

        [TestMethod]
        public void CompareGetAnimal()
        {
            var actual = _agriRepository.GetAnimal(1);
            var expected = _staticExtRepo.GetAnimal(1);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Name, actual.Name);
        }

        [TestMethod]
        public void CompareGetAnimals()
        {
            var actual = _agriRepository.GetAnimals();
            var expected = _staticExtRepo.GetAnimals();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(actual.Count, expected.Count);
        }

        [TestMethod]
        public void CompareGetAnimalSubType()
        {
            //Beef
            var actual = _agriRepository.GetAnimalSubType(1);
            var expected = _staticExtRepo.GetAnimalSubType(1);

            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Animal);
            Assert.AreEqual(expected.AnimalId, actual.AnimalId);
            Assert.AreEqual(expected.Name, actual.Name);
        }

        [TestMethod]
        public void CompareGetAnimalSubTypesWithId()
        {
            //Swine
            var actual = _agriRepository.GetAnimalSubTypes(9);
            var expected = _staticExtRepo.GetAnimalSubTypes(9);

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetAnimalSubTypes()
        {
            var actual = _agriRepository.GetAnimalSubTypes();
            var expected = _staticExtRepo.GetAnimalSubTypes();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetAnimalSubTypesDll()
        {
            var actual = _agriRepository.GetAnimalTypesDll();
            var expected = _staticExtRepo.GetAnimalTypesDll();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetApplications()
        {
            var actual = _agriRepository.GetApplications();
            var expected = _staticExtRepo.GetApplications();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetApplicationsDll()
        {
            var actual = _agriRepository.GetApplicationsDll("Liquid");
            var expected = _staticExtRepo.GetApplicationsDll("Liquid");

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetBCSampleDateForNitrateCredit()
        {
            var actual = _agriRepository.GetBCSampleDateForNitrateCredit();
            var expected = _staticExtRepo.GetBCSampleDateForNitrateCredit();

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.CoastalFromDateMonth, actual.CoastalFromDateMonth);
            Assert.AreEqual(expected.CoastalToDateMonth, actual.CoastalToDateMonth);
        }

        [TestMethod]
        public void CompareGetConversionFactor()
        {
            var actual = _agriRepository.GetConversionFactor();
            var expected = _staticExtRepo.GetConversionFactor();

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.DefaultSoilTestKelownaPhosphorous, actual.DefaultSoilTestKelownaPhosphorous);
            Assert.AreEqual(expected.DefaultSoilTestKelownaPotassium, actual.DefaultSoilTestKelownaPotassium);
            Assert.AreEqual(expected.SoilTestPPMToPoundPerAcre, actual.SoilTestPPMToPoundPerAcre);
            Assert.AreEqual(expected.DefaultApplicationOfManureInPrevYears, actual.DefaultApplicationOfManureInPrevYears);
            Assert.AreEqual(expected.KilogramPerHectareToPoundPerAcreConversion, actual.KilogramPerHectareToPoundPerAcreConversion);
        }

        [TestMethod]
        public void CompareGetCrop()
        {
            //Alfalfa or legume dominant stand (1 cut)
            var actual = _agriRepository.GetCrop(1);
            var expected = _staticExtRepo.GetCrop(1);

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.CropYields.Count > 0);
            Assert.AreEqual(expected.CropName, actual.CropName);
            Assert.AreEqual(expected.YieldCd, actual.YieldCd);
        }

        [TestMethod]
        public void CompareCropHarvestUnitsDll()
        {
            var actual = _agriRepository.GetCropHarvestUnitsDll();
            var expected = _staticExtRepo.GetCropHarvestUnitsDll();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetCropPrevYearManureApplVolCatCd()
        {
            //Alfalfa or legume dominant stand (1 cut)
            var actual = _agriRepository.GetCropPrevYearManureApplVolCatCd(1);
            var expected = _staticExtRepo.GetCropPrevYearManureApplVolCatCd(1);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CompareGetCrops()
        {
            var actual = _agriRepository.GetCrops();
            var expected = _staticExtRepo.GetCrops();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetCropsDll()
        {
            //Forage
            var actual = _agriRepository.GetCropsDll(1);
            var expected = _staticExtRepo.GetCropsDll(1);

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetCropSoilTestPhosphorousRegions()
        {
            var actual = _agriRepository.GetCropSoilTestPhosphorousRegions();
            var expected = _staticExtRepo.GetCropSoilTestPhosphorousRegions();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetCropSoilTestPotassiumRegions()
        {
            var actual = _agriRepository.GetCropSoilTestPotassiumRegions();
            var expected = _staticExtRepo.GetCropSoilTestPotassiumRegions();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetCropSTKRegionCd()
        {
            //Alfalfa or legume dominant stand (1 cut)
            var actual = _agriRepository.GetCropSTKRegionCd(1, 1);
            var expected = _staticExtRepo.GetCropSTKRegionCd(1, 1);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.PotassiumCropGroupRegionCode, actual.PotassiumCropGroupRegionCode);
            Assert.AreEqual(expected.SoilTestPotassiumRegionCode, actual.SoilTestPotassiumRegionCode);
        }

        [TestMethod]
        public void CompareGetCropSTPRegionCd()
        {
            //Alfalfa or legume dominant stand (1 cut)
            var actual = _agriRepository.GetCropSTPRegionCd(1, 1);
            var expected = _staticExtRepo.GetCropSTPRegionCd(1, 1);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.PhosphorousCropGroupRegionCode, actual.PhosphorousCropGroupRegionCode);
            Assert.AreEqual(expected.SoilTestPhosphorousRegionCode, actual.SoilTestPhosphorousRegionCode);
        }

        [TestMethod]
        public void CompareGetCropType()
        {
            //Alfalfa or legume dominant stand (1 cut)
            var actual = _agriRepository.GetCropType(1);
            var expected = _staticExtRepo.GetCropType(1);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.CoverCrop, actual.CoverCrop);
            Assert.AreEqual(expected.CrudeProteinRequired, actual.CrudeProteinRequired);
        }

        [TestMethod]
        public void CompareGetCropTypes()
        {
            var actual = _agriRepository.GetCropTypes();
            var expected = _staticExtRepo.GetCropTypes();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetCropTypesDll()
        {
            var actual = _agriRepository.GetCropTypesDll();
            var expected = _staticExtRepo.GetCropTypesDll();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetCropYield()
        {
            //Alfalfa or legume dominant stand (1 cut)
            var actual = _agriRepository.GetCropYield(1, 1);
            var expected = _staticExtRepo.GetCropYield(1, 1);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Amount, actual.Amount);
        }

        [TestMethod]
        public void CompareGetCropYields()
        {
            var actual = _agriRepository.GetCropYields();
            var expected = _staticExtRepo.GetCropYields();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetDefaultSoilTest()
        {
            var actual = _agriRepository.GetDefaultSoilTest();
            var expected = _staticExtRepo.GetDefaultSoilTest();

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.ConvertedKelownaK, actual.ConvertedKelownaK);
            Assert.AreEqual(expected.ConvertedKelownaP, actual.ConvertedKelownaP);
        }

        [TestMethod]
        public void CompareGetDensityUnit()
        {
            var actual = _agriRepository.GetDensityUnit(1);
            var expected = _staticExtRepo.GetDensityUnit(1);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Name, actual.Name);
        }

        [TestMethod]
        public void CompareGetDensityUnits()
        {
            var actual = _agriRepository.GetDensityUnits();
            var expected = _staticExtRepo.GetDensityUnits();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetDensityUnitsDll()
        {
            var actual = _agriRepository.GetDensityUnitsDll();
            var expected = _staticExtRepo.GetDensityUnitsDll();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetAnimalsUsingWashWater()
        {
            var actual = _agriRepository.GetAnimalsUsingWashWater();
            var expected = _staticExtRepo.GetAnimalsUsingWashWater();

            Assert.IsTrue(actual.Animals.Count > 0);
            Assert.AreEqual(expected.Animals.Count, actual.Animals.Count);
        }

        [TestMethod]
        public void CompareDoesAnimalUseWashWater()
        {
            var actual = _agriRepository.DoesAnimalUseWashWater(9);
            var expected = _staticExtRepo.DoesAnimalUseWashWater(9);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CompareGetDryMatter()
        {
            //Lagoon <1% DM
            var actual = _agriRepository.GetDryMatter(1);
            var expected = _staticExtRepo.GetDryMatter(1);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Name, actual.Name);
        }

        [TestMethod]
        public void CompareGetDryMatters()
        {
            var actual = _agriRepository.GetDryMatters();
            var expected = _staticExtRepo.GetDryMatters();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetExternalLinks()
        {
            var actual = _agriRepository.GetExternalLinks();
            var expected = _staticExtRepo.GetExternalLinks();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetFertilizer()
        {
            var actual = _agriRepository.GetFertilizer("1");
            var expected = _staticExtRepo.GetFertilizer("1");

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.DryLiquid, actual.DryLiquid);
        }

        [TestMethod]
        public void CompareGetFertilizerMethod()
        {
            var actual = _agriRepository.GetFertilizerMethod("1");
            var expected = _staticExtRepo.GetFertilizerMethod("1");

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Name, actual.Name);
        }

        [TestMethod]
        public void CompareGetFertilizerMethods()
        {
            var actual = _agriRepository.GetFertilizerMethods();
            var expected = _staticExtRepo.GetFertilizerMethods();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetFertilizerMethodsDll()
        {
            var actual = _agriRepository.GetFertilizerMethodsDll();
            var expected = _staticExtRepo.GetFertilizerMethodsDll();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetFertilizerRptStdUnit()
        {
            var actual = _agriRepository.GetFertilizerRptStdUnit("dry");
            var expected = _staticExtRepo.GetFertilizerRptStdUnit("dry");

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);

            actual = _agriRepository.GetFertilizerRptStdUnit("liquid");
            expected = _staticExtRepo.GetFertilizerRptStdUnit("liquid");

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CompareGetFertilizers()
        {
            var actual = _agriRepository.GetFertilizers();
            var expected = _staticExtRepo.GetFertilizers();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetFertilizersDll()
        {
            var actual = _agriRepository.GetFertilizersDll("dry");
            var expected = _staticExtRepo.GetFertilizersDll("dry");

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);

            actual = _agriRepository.GetFertilizersDll("liquid");
            expected = _staticExtRepo.GetFertilizersDll("liquid");

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetFertilizerUnit()
        {
            var actual = _agriRepository.GetFertilizerUnit(1);
            var expected = _staticExtRepo.GetFertilizerUnit(1);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.DryLiquid, actual.DryLiquid);
        }

        [TestMethod]
        public void CompareGetFertilizerUnits()
        {
            var actual = _agriRepository.GetFertilizerUnits();
            var expected = _staticExtRepo.GetFertilizerUnits();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetFertilizerUnitsDll()
        {
            var actual = _agriRepository.GetFertilizerUnitsDll("dry");
            var expected = _staticExtRepo.GetFertilizerUnitsDll("dry");

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);

            actual = _agriRepository.GetFertilizerUnitsDll("liquid");
            expected = _staticExtRepo.GetFertilizerUnitsDll("liquid");

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetHarvestUnits()
        {
            var actual = _agriRepository.GetHarvestUnits();
            var expected = _staticExtRepo.GetHarvestUnits();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetHarvestYieldDefaultUnitName()
        {
            var actual = _agriRepository.GetHarvestYieldDefaultUnitName();
            var expected = _staticExtRepo.GetHarvestYieldDefaultUnitName();

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CompareGetHarvestYieldUnitName()
        {
            var actual = _agriRepository.GetHarvestYieldUnitName("1");
            var expected = _staticExtRepo.GetHarvestYieldUnitName("1");

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CompareGetIncludeWashWater()
        {
            //Milking Cow
            var actual = _agriRepository.GetIncludeWashWater(9);
            var expected = _staticExtRepo.GetIncludeWashWater(9);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CompareGetInteriorId()
        {
            var actual = _agriRepository.GetInteriorId();
            var expected = _staticExtRepo.GetInteriorId();

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CompareGetLiquidFertilizerDensities()
        {
            var actual = _agriRepository.GetLiquidFertilizerDensities();
            var expected = _staticExtRepo.GetLiquidFertilizerDensities();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetLiquidFertilizerDensity()
        {
            var actual = _agriRepository.GetLiquidFertilizerDensity(22, 3);
            var expected = _staticExtRepo.GetLiquidFertilizerDensity(22, 3);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Value, actual.Value);
        }

        [TestMethod]
        public void CompareGetLocations()
        {
            var actual = _agriRepository.GetLocations();
            var expected = _staticExtRepo.GetLocations();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetManure()
        {
            var actual = _agriRepository.GetManure("1");
            var expected = _staticExtRepo.GetManure("1");

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Name, actual.Name);
        }

        [TestMethod]
        public void CompareGetManureByName()
        {
            var actual = _agriRepository.GetManureByName("Beef-feedlot, solid (dry)");
            var expected = _staticExtRepo.GetManureByName("Beef-feedlot, solid (dry)");

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Name, actual.Name);
        }

        [TestMethod]
        public void CompareGetManureRptStdUnit()
        {
            var actual = _agriRepository.GetManureRptStdUnit("dry");
            var expected = _staticExtRepo.GetManureRptStdUnit("dry");

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CompareGetManures()
        {
            var actual = _agriRepository.GetManures();
            var expected = _staticExtRepo.GetManures();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetManuresDll()
        {
            var actual = _agriRepository.GetManuresDll();
            var expected = _staticExtRepo.GetManuresDll();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetMessageByChemical()
        {
            var actual = _agriRepository.GetMessageByChemicalBalance("AgrN", 5, false);
            var expected = _staticExtRepo.GetMessageByChemicalBalance("AgrN", 5, false);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }
    }
}
