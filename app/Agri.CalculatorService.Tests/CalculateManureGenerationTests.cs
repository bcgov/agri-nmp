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
    }
}