using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agri.Models.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Agri.Data.TestHarness
{
    [TestClass]
    public class SeedDataLoaderTests
    {
        [TestMethod]
        public void GetMigrationSeedData()
        {
            var result = SeedDataLoader.GetMigrationSeedData<List<UserPrompt>>("1_UserPrompts");
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.Count > 0);
            Assert.IsTrue(result.AppliedDateTime > default(DateTime));
        }

        [TestMethod]
        public void GetSeedJsonData_Should_Load_Json()
        {
            var result = SeedDataLoader.GetSeedJsonData();

            Assert.IsNotNull(result);
        }
    }
}