using Agri.LegacyData.Models.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Agri.LegacyData.Tests
{
    [TestClass]
    public class StaticJsonLoaderTests
    {
        [TestMethod]
        public void RetrieveStaticJsonSuccessfully()
        {
            var rss = StaticDataLoader.GetStaticDataJson();

            Assert.IsNotNull(rss);
        }

        [TestMethod]
        public void RetrieveAmmoniaRetentionListSuccessfully()
        {
            var staticDataLists = new StaticDataExtRepository();
            var result = staticDataLists.GetAmmoniaRetentions();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }
    }
}
