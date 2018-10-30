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
    }
}
