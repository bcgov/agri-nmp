using System;
using Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
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

            var cropIds = _agriRepository.GetCrops().Select(c => c.Id).ToList();
            var expected = _staticExtRepo.GetCropSoilTestPotassiumRegions().Where(test => cropIds.Any(c => c == test.CropId)).ToList();

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
            Assert.AreEqual(expected.Message, actual.Message);
            Assert.AreEqual(expected.Icon, actual.Icon);

            actual = _agriRepository.GetMessageByChemicalBalance("AgrN", 40, false);
            expected = _staticExtRepo.GetMessageByChemicalBalance("AgrN", 40, false);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Message, actual.Message);
            Assert.AreEqual(expected.Icon, actual.Icon);
        }

        [TestMethod]
        public void CompareGetMessageByChemicalSoilTest()
        {
            var actual = _agriRepository.GetMessageByChemicalBalance("AgrN", 5, false, 0);
            var expected = _staticExtRepo.GetMessageByChemicalBalance("AgrN", 5, false, 0);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);

            actual = _agriRepository.GetMessageByChemicalBalance("AgrN", 5, true, 0);
            expected = _staticExtRepo.GetMessageByChemicalBalance("AgrN", 5, true, 0);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CompareGetMessageByChemicalBalance2()
        {
            var actual = _agriRepository.GetMessageByChemicalBalance("AgrP2O5CropP2O5", 40, 40, "CropP2O5");
            var expected = _staticExtRepo.GetMessageByChemicalBalance("AgrP2O5CropP2O5", 40, 40, "CropP2O5");

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Message, actual.Message);
            Assert.AreEqual(expected.Icon, actual.Icon);

            actual = _agriRepository.GetMessageByChemicalBalance("AgrP2O5CropP2O5", 40, 0, "CropP2O5");
            expected = _staticExtRepo.GetMessageByChemicalBalance("AgrP2O5CropP2O5", 40, 0, "CropP2O5");
        }
        
        [TestMethod]
        public void CompareGetMessages()
        {
            var actual = _agriRepository.GetMessages();
            var expected = _staticExtRepo.GetMessages();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetMilkProduction()
        {
            var actual = _agriRepository.GetMilkProduction(9);
            var expected = _staticExtRepo.GetMilkProduction(9);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CompareGetNitrogeMineralizations()
        {
            var actual = _agriRepository.GetNitrogeMineralizations();
            var expected = _staticExtRepo.GetNitrogeMineralizations();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetNitrogenRecommendations()
        {
            var actual = _agriRepository.GetNitrogenRecommendations();
            var expected = _staticExtRepo.GetNitrogenRecommendations();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetNMineralization()
        {
            var actual = _agriRepository.GetNMineralization(1,1);
            var expected = _staticExtRepo.GetNMineralization(1,1);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.FirstYearValue, actual.FirstYearValue);
        }

        [TestMethod]
        public void CompareGetNutrientIcon()
        {
            var actual = _agriRepository.GetNutrientIcon("good");
            var expected = _staticExtRepo.GetNutrientIcon("good");

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Definition, actual.Definition);
        }

        [TestMethod]
        public void CompareGetNutrientIcons()
        {
            var actual = _agriRepository.GetNutrientIcons();
            var expected = _staticExtRepo.GetNutrientIcons();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetPrevCropType()
        {
            var actual = _agriRepository.GetPrevCropType(1);
            var expected = _staticExtRepo.GetPrevCropType(1);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.PreviousCropCode, actual.PreviousCropCode);
        }

        [TestMethod]
        public void CompareGetPrevCropTypesDll()
        {
            var actual = _agriRepository.GetPrevCropTypesDll("1");
            var expected = _staticExtRepo.GetPrevCropTypesDll("1");

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetPreviousCropTypes()
        {
            var actual = _agriRepository.GetPreviousCropTypes();
            var expected = _staticExtRepo.GetPreviousCropTypes();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetPrevManureApplicationInPrevYears()
        {
            var actual = _agriRepository.GetPrevManureApplicationInPrevYears();
            var expected = _staticExtRepo.GetPrevManureApplicationInPrevYears();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetPrevYearManureNitrogenCreditDefaults()
        {
            var actual = _agriRepository.GetPrevYearManureNitrogenCreditDefaults();
            var expected = _staticExtRepo.GetPrevYearManureNitrogenCreditDefaults();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetRegion()
        {
            var actual = _agriRepository.GetRegion(1);
            var expected = _staticExtRepo.GetRegion(1);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.SoilTestPhosphorousRegionCd, actual.SoilTestPhosphorousRegionCd);
        }

        [TestMethod]
        public void CompareGetRegions()
        {
            var actual = _agriRepository.GetRegions();
            var expected = _staticExtRepo.GetRegions();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetRegionsDll()
        {
            var actual = _agriRepository.GetRegionsDll();
            var expected = _staticExtRepo.GetRegionsDll();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetRptCompletedFertilizerRequiredStdUnit()
        {
            var actual = _agriRepository.GetRptCompletedFertilizerRequiredStdUnit();
            var expected = _staticExtRepo.GetRptCompletedFertilizerRequiredStdUnit();

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.LiquidUnitId, actual.LiquidUnitId);
            Assert.AreEqual(expected.SolidUnitId, actual.SolidUnitId);
        }

        [TestMethod]
        public void CompareGetRptCompletedManureRequiredStdUnit()
        {
            var actual = _agriRepository.GetRptCompletedManureRequiredStdUnit();
            var expected = _staticExtRepo.GetRptCompletedManureRequiredStdUnit();

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.LiquidUnitId, actual.LiquidUnitId);
            Assert.AreEqual(expected.SolidUnitId, actual.SolidUnitId);
        }

        [TestMethod]
        public void CompareGetSeasonApplications()
        {
            var actual = _agriRepository.GetSeasonApplications();
            var expected = _staticExtRepo.GetSeasonApplications();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetSoilTestMethod()
        {
            var actual = _agriRepository.GetSoilTestMethod("1");
            var expected = _staticExtRepo.GetSoilTestMethod("1");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CompareGetSoilTestMethodById()
        {
            var actual = _agriRepository.GetSoilTestMethodById("1");
            var expected = _staticExtRepo.GetSoilTestMethodById("1");

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.ConvertToKelownaPHGreaterThanEqual72, actual.ConvertToKelownaPHGreaterThanEqual72);
        }

        [TestMethod]
        public void CompareGetSoilTestMethods()
        {
            var actual = _agriRepository.GetSoilTestMethods();
            var expected = _staticExtRepo.GetSoilTestMethods();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetSoilTestMethodsDll()
        {
            var actual = _agriRepository.GetSoilTestMethodsDll();
            var expected = _staticExtRepo.GetSoilTestMethodsDll();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetSoilTestNitratePPMToPoundPerAcreConversionFactor()
        {
            var actual = _agriRepository.GetSoilTestNitratePPMToPoundPerAcreConversionFactor();
            var expected = _staticExtRepo.GetSoilTestNitratePPMToPoundPerAcreConversionFactor();

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CompareGetSoilTestPhosphorousKelownaRanges()
        {
            var actual = _agriRepository.GetSoilTestPhosphorousKelownaRanges();
            var expected = _staticExtRepo.GetSoilTestPhosphorousKelownaRanges();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetSoilTestPhosphorousRecommendations()
        {
            var actual = _agriRepository.GetSoilTestPhosphorousRecommendations();
            var expected = _staticExtRepo.GetSoilTestPhosphorousRecommendations();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetSoilTestPhosphorusRanges()
        {
            var actual = _agriRepository.GetSoilTestPhosphorusRanges();
            var expected = _staticExtRepo.GetSoilTestPhosphorusRanges();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetSoilTestPotassiumRanges()
        {
            var actual = _agriRepository.GetSoilTestPotassiumRanges();
            var expected = _staticExtRepo.GetSoilTestPotassiumRanges();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetSoilTestPotassiumRecommendations()
        {
            var actual = _agriRepository.GetSoilTestPotassiumRecommendations();
            var expected = _staticExtRepo.GetSoilTestPotassiumRecommendations();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetSoilTestPotassiumKelownaRanges()
        {
            var actual = _agriRepository.GetSoilTestPotassiumKelownaRanges();
            var expected = _staticExtRepo.GetSoilTestPotassiumKelownaRanges();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetSoilTestWarning()
        {
            var actual = _agriRepository.GetSoilTestWarning();
            var expected = _staticExtRepo.GetSoilTestWarning();

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CompareGetStaticDataVersion()
        {
            var actual = _agriRepository.GetStaticDataVersion();
            var expected = _staticExtRepo.GetStaticDataVersion();

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CompareGetSTKKelownaRangeByPpm()
        {
            var actual = _agriRepository.GetSTKKelownaRangeByPpm(10);
            var expected = _staticExtRepo.GetSTKKelownaRangeByPpm(10);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Range, actual.Range);
            Assert.AreEqual(expected.RangeLow, actual.RangeLow);
            Assert.AreEqual(expected.RangeHigh, actual.RangeHigh);
        }

        [TestMethod]
        public void CompareGetSTKRecommend()
        {
            var actual = _agriRepository.GetSTKRecommend(1,1,1);
            var expected = _staticExtRepo.GetSTKRecommend(1, 1, 1);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.K2ORecommendationKilogramPerHectare, actual.K2ORecommendationKilogramPerHectare);
        }

        [TestMethod]
        public void CompareGetSTPKelownaRangeByPpm()
        {
            var actual = _agriRepository.GetSTPKelownaRangeByPpm(10);
            var expected = _staticExtRepo.GetSTPKelownaRangeByPpm(10);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Range, actual.Range);
            Assert.AreEqual(expected.RangeLow, actual.RangeLow);
            Assert.AreEqual(expected.RangeHigh, actual.RangeHigh);
        }

        [TestMethod]
        public void CompareGetSTPRecommend()
        {
            var actual = _agriRepository.GetSTPRecommend(1, 1, 6);
            var expected = _staticExtRepo.GetSTPRecommend(1, 1, 6);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.P2O5RecommendationKilogramPerHectare, actual.P2O5RecommendationKilogramPerHectare);
        }

        [TestMethod]
        public void CompareGetSubtypesDll()
        {
            var actual = _agriRepository.GetSubtypesDll(1);
            var expected = _staticExtRepo.GetSubtypesDll(1);

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetUnit()
        {
            var actual = _agriRepository.GetUnit("1");
            var expected = _staticExtRepo.GetUnit("1");

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.SolidLiquid, actual.SolidLiquid);
        }

        [TestMethod]
        public void CompareGetUnits()
        {
            var actual = _agriRepository.GetUnits();
            var expected = _staticExtRepo.GetUnits();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetUnitsDll()
        {
            var actual = _agriRepository.GetUnitsDll("solid");
            var expected = _staticExtRepo.GetUnitsDll("solid");

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);

            actual = _agriRepository.GetUnitsDll("liquid");
            expected = _staticExtRepo.GetUnitsDll("liquid");

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetUserPrompt()
        {
            var actual = _agriRepository.GetUserPrompt("welcome");
            var expected = _staticExtRepo.GetUserPrompt("welcome");

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CompareGetUserPrompts()
        {
            var actual = _agriRepository.GetUserPrompts();
            var expected = _staticExtRepo.GetUserPrompts();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetVersionData()
        {
            var actual = _agriRepository.GetVersionData();
            var expected = _staticExtRepo.GetVersionData();

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.StaticDataVersion, actual.StaticDataVersion);
        }

        [TestMethod]
        public void CompareGetYieldById()
        {
            var actual = _agriRepository.GetYieldById(1);
            var expected = _staticExtRepo.GetYieldById(1);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.YieldDesc, actual.YieldDesc);
        }

        [TestMethod]
        public void CompareGetYields()
        {
            var actual = _agriRepository.GetYields();
            var expected = _staticExtRepo.GetYields();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareIsCustomFertilizer()
        {
            var actual = _agriRepository.IsCustomFertilizer(1);
            var expected = _staticExtRepo.IsCustomFertilizer(1);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CompareIsFertilizerTypeDry()
        {
            var actual = _agriRepository.IsFertilizerTypeDry(1);
            var expected = _staticExtRepo.IsFertilizerTypeDry(1);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CompareIsFertilizerTypeLiquid()
        {
            var actual = _agriRepository.IsFertilizerTypeLiquid(1);
            var expected = _staticExtRepo.IsFertilizerTypeLiquid(1);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CompareIsNitrateCreditApplicable()
        {
            var actual = _agriRepository.IsNitrateCreditApplicable(1, DateTime.Today, 2018);
            var expected = _staticExtRepo.IsNitrateCreditApplicable(1, DateTime.Today, 2018);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);

            actual = _agriRepository.IsNitrateCreditApplicable(7, DateTime.Today, 2018);
            expected = _staticExtRepo.IsNitrateCreditApplicable(7, DateTime.Today, 2018);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);

            actual = _agriRepository.IsNitrateCreditApplicable(14, DateTime.Today, 2018);
            expected = _staticExtRepo.IsNitrateCreditApplicable(14, DateTime.Today, 2018);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CompareIsRegionInteriorBC()
        {
            var actual = _agriRepository.IsRegionInteriorBC(1);
            var expected = _staticExtRepo.IsRegionInteriorBC(1);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);

            actual = _agriRepository.IsRegionInteriorBC(7);
            expected = _staticExtRepo.IsRegionInteriorBC(7);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);

            actual = _agriRepository.IsRegionInteriorBC(14);
            expected = _staticExtRepo.IsRegionInteriorBC(14);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CompareGetPotassiumSoilTestRating()
        {
            var actual = _agriRepository.GetPotassiumSoilTestRating(1);
            var expected = _staticExtRepo.GetPotassiumSoilTestRating(1);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CompareGetPhosphorusSoilTestRating()
        {
            var actual = _agriRepository.GetPhosphorusSoilTestRating(1);
            var expected = _staticExtRepo.GetPhosphorusSoilTestRating(1);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CompareGetPotassiumSoilTestRanges()
        {
            var actual = _agriRepository.GetPotassiumSoilTestRanges();
            var expected = _staticExtRepo.GetPotassiumSoilTestRanges();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetPhosphorusSoilTestRanges()
        {
            var actual = _agriRepository.GetPhosphorusSoilTestRanges();
            var expected = _staticExtRepo.GetPhosphorusSoilTestRanges();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareWasManureAddedInPreviousYear()
        {
            var actual = _agriRepository.WasManureAddedInPreviousYear("1");
            var expected = _staticExtRepo.WasManureAddedInPreviousYear("1");

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CompareGetMainMenus()
        {
            var actual = _agriRepository.GetMainMenus();
            var expected = _staticExtRepo.GetMainMenus();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }
        
        [TestMethod]
        public void CompareGetMainMenusDll()
        {
            var actual = _agriRepository.GetMainMenusDll();
            var expected = _staticExtRepo.GetMainMenusDll();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetSubMenus()
        {
            var actual = _agriRepository.GetSubMenus();
            var expected = _staticExtRepo.GetSubMenus();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetManureImportedDefault()
        {
            var actual = _agriRepository.GetManureImportedDefault();
            var expected = _staticExtRepo.GetManureImportedDefault();

            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.Id, actual.Id);
            Assert.AreEqual(actual.DefaultSolidMoisture, actual.DefaultSolidMoisture);
        }

        [TestMethod]
        public void CompareGetSolidMaterialsConversionFactors()
        {
            var actual = _agriRepository.GetSolidMaterialsConversionFactors();
            var expected = _staticExtRepo.GetSolidMaterialsConversionFactors();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetLiquidMaterialsConversionFactors()
        {
            var actual = _agriRepository.GetLiquidMaterialsConversionFactors();
            var expected = _staticExtRepo.GetLiquidMaterialsConversionFactors();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetSolidMaterialApplicationTonPerAcreRateConversions()
        {
            var actual = _agriRepository.GetSolidMaterialApplicationTonPerAcreRateConversions();
            var expected = _staticExtRepo.GetSolidMaterialApplicationTonPerAcreRateConversions();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void CompareGetLiquidMaterialApplicationUSGallonsPerAcreRateConversion()
        {
            var actual = _agriRepository.GetLiquidMaterialApplicationUSGallonsPerAcreRateConversion();
            var expected = _staticExtRepo.GetLiquidMaterialApplicationUSGallonsPerAcreRateConversion();

            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [Ignore]
        [TestMethod]
        public void GetMessageByChemicalBalanceForSpecificValues()
        {
            var actual = _agriRepository.GetMessageByChemicalBalance("CropN", -5, false);
            var expected = _staticExtRepo.GetMessageByChemicalBalance("CropN", -5, false);

            Assert.IsNotNull(actual);
        }
    }
}
