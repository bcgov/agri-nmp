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
        public void LoadSeedDataFromJson()
        {
            var result = SeedDataLoader.GetStaticDataJson<List<UserPrompt>>("UserPrompts_Add");

            Assert.IsNotNull(result);
        }
    }
}
