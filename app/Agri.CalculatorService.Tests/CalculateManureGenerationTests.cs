using Agri.Data;
using Agri.Tests.Shared;
using Shouldly;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Agri.CalculateService.Tests
{
    public class CalculateManureGenerationTests : TestBase
    {
        private readonly ICalculateManureGeneration calculator;

        public CalculateManureGenerationTests(ITestOutputHelper output) : base(output)
        {
            SeedDatabase();
            var repo = new AgriConfigurationRepository(agriConfigurationDb, Mapper);
            calculator = new CalculateManureGeneration(repo);
        }

        [Fact]
        public void GetSolidTonsGeneratedForAnimalSubType_Should_Calculate()
        {
            var calfToWeening = agriConfigurationDb.AnimalSubType
                .Single(a => a.Name.Equals("Calves (0 to 3 months old)", StringComparison.OrdinalIgnoreCase));

            var result = calculator.GetSolidTonsGeneratedForAnimalSubType(calfToWeening.Id, 100, 12);

            result.ShouldBe(9);
        }

        [Fact]
        public void GetTonsGeneratedForPoultrySubType_Should_Calcluate()
        {
            var broiler = agriConfigurationDb.AnimalSubType.Single(a => a.Name.Equals("Broiler Chicken (6.5 cycles)", StringComparison.OrdinalIgnoreCase));
            var roaster = agriConfigurationDb.AnimalSubType.Single(a => a.Name.Equals("Roaster Chicken", StringComparison.OrdinalIgnoreCase));
            var pullets = agriConfigurationDb.AnimalSubType.Single(a => a.Name.Equals("Broiler Breeder Pullets", StringComparison.OrdinalIgnoreCase));

            calculator.GetTonsGeneratedForPoultrySubType(broiler.Id, 20000, 5.5m, 42).ShouldBe(153);
            calculator.GetTonsGeneratedForPoultrySubType(roaster.Id, 10000, 4, 60).ShouldBe(110);
            calculator.GetTonsGeneratedForPoultrySubType(pullets.Id, 11000, 1, 330).ShouldBe(129);
        }
    }
}