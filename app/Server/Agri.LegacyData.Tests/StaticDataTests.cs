using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agri.LegacyData.Models.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Agri.LegacyData.Tests
{
    [TestClass]
    public class StaticDataTests
    {
        private StaticDataExtRepository _staticDataExtRepo;
        private StaticDataRepository _staticDataRepo;

        public StaticDataTests()
        {
            _staticDataRepo = new StaticDataRepository();
            _staticDataExtRepo = new StaticDataExtRepository();
        }

        [TestMethod]
        public void GetAnimalSubTypesSuccessfully()
        {
            var result = _staticDataRepo.GetAnimalSubTypes();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void GetLocationTypesSuccessfully()
        {
            var result = _staticDataExtRepo.GetLocations();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void GetCropYieldsSuccessfully()
        {
            var result = _staticDataExtRepo.GetCropYields();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void GetCropStkRegionCdsSuccessfully()
        {
            var result = _staticDataExtRepo.GetCropStkRegionCds();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void GetCropStpRegionCdsSuccessfully()
        {

        }

        [TestMethod]
        public void GetSoilTestMethodsSuccessfully()
        {
            var result = _staticDataRepo.GetSoilTestMethods();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void GetFertilizerMethodsSuccessfully()
        {
            var result = _staticDataRepo.GetFertilizerMethods();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void GetUserPromptsSuccessfully()
        {
            var result = _staticDataExtRepo.GetUserPromts();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void GetExternalLinksSuccessfully()
        {
            var result = _staticDataExtRepo.GetExternalLinks();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void GetUnitsSuccessfully()
        {
            var result = _staticDataExtRepo.GetUnits();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void GetSoilTestPhosphorousRangesSuccessfully()
        {
            var result = _staticDataExtRepo.GetSoilTestPhosphorusRanges();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void GetSoilTestPotassiumRangesSuccessfully()
        {
            var result = _staticDataExtRepo.GetSoilTestPotassiumRanges();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void GetMessagesSuccessfully()
        {
            var result = _staticDataExtRepo.GetMessages();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }
        [TestMethod]
        public void GetSeasonApplicationsSuccessfully()
        {
            var result = _staticDataExtRepo.GetSeasonApplications();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void GetYieldsSuccessfully()
        {
            var result = _staticDataExtRepo.GetYields();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void GetNitrogenRecommendationsSuccessfully()
        {
            var result = _staticDataExtRepo.GetNitrogenRecommendations();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }
    }
}
