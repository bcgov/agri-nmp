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
    }
}
