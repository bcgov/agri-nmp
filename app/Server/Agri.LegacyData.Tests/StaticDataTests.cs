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
            var result = _staticDataExtRepo.GetCropStpRegionCds();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void GetSTKKelownaRangesSuccessfully()
        {
            var result = _staticDataExtRepo.GetSTKKelownaRanges();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void GetSTPKelownaRangesSuccessfully()
        {
            var result = _staticDataExtRepo.GetSTPKelownaRanges();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void GetSTKRecommendsSuccessfully()
        {
            var result = _staticDataExtRepo.GetSTKRecommendations();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void GetSTPRecommendsSuccessfully()
        {
            var result = _staticDataExtRepo.GetSTPRecommendations();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }
    }
}
