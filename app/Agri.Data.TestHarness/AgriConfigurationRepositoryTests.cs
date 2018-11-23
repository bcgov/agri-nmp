using Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Agri.Data.TestHarness
{
    [TestClass]
    public class AgriConfigurationRepositoryTests
    {
        public AgriConfigurationRepositoryTests()
        {
            //var connectionString = TestHelper.GetConnectionString();
            var services = new ServiceCollection();

            //services.AddDbContext<AgriConfigurationContext>(options =>
            //{
            //    options.UseNpgsql(agriConnectionString, b => b.MigrationsAssembly("Agri.Data"));
            //});
        }

        [TestMethod]
        public void GetConvertYieldFromBushelToTonsPerAcre()
        {
        }
    }
}
