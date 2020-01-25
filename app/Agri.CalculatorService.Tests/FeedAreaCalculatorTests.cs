using Agri.CalculateService;
using Agri.Data;
using Agri.Models.Farm;
using Agri.Tests.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Agri.CalculatorService.Tests
{
    public class FeedAreaCalculatorTests : TestBase
    {
        private readonly IFeedAreaCalculator _calculator;

        public FeedAreaCalculatorTests(ITestOutputHelper output) : base(output)
        {
            var repo = new AgriConfigurationRepository(agriConfigurationDb, Mapper);
            _calculator = new FeedAreaCalculator(repo);
            SeedDatabase();
        }

        [Fact]
        public void NAgronomicBalanceShouldBeCorrectValue()
        {
        }

        [Fact]
        public void P205AgronomicBalanceShouldBeCorrectValue()
        {
        }

        [Fact]
        public void K20AgronomicBalanceShouldBeCorrectValue()
        {
        }

        private Field GetTestField()
        {
            var field = new Field
            {
                Area = 12, //acres
                MatureAnimalCount = 60,
                MatureAnimalAverageWeight = 1200,
                MatureAnimalDailyFeedRequirementId = 2, //Should be .026 from db
                GrowingAnimalCount = 50,
                GrowingAnimalAverageWeight = 500,
                GrowingAnimalDailyFeedRequirementId = 3, //Should be .03 from db
                FeedingDaysSpentInFeedingArea = 135,
                FeedingPercentageOutsideFeeingArea = 15,
                FeedForageAnalyses = new List<FeedForageAnalysis>
                {
                    new FeedForageAnalysis
                    {
                        FeedForageId = 1,
                        CrudeProteinPercent = 12.9M,
                        Phosphorus = .21m,
                        Potassium = 1.87m,
                        PercentOfTotalFeedForageToAnimals = 60,
                        PercentOfFeedForageWastage = 10
                    },
                    new FeedForageAnalysis
                    {
                        FeedForageId = 6,
                        CrudeProteinPercent = 11.5M,
                        Phosphorus = .22m,
                        Potassium = 1.62m,
                        PercentOfTotalFeedForageToAnimals = 30,
                        PercentOfFeedForageWastage = 15
                    },
                    new FeedForageAnalysis
                    {
                        FeedForageId = 4,
                        CrudeProteinPercent = 10M,
                        Phosphorus = .29m,
                        Potassium = .37m,
                        PercentOfTotalFeedForageToAnimals = 30,
                        PercentOfFeedForageWastage = 15
                    }
                }
            };

            return field;
        }
    }
}