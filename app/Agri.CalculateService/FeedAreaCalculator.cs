using Agri.Data;
using Agri.Models.Configuration;
using Agri.Models.Farm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agri.CalculateService
{
    public interface IFeedAreaCalculator
    {
        decimal GetK20AgronomicBalance(Field field, Region region);

        decimal GetNitrogenAgronomicBalance(Field field, Region region);

        decimal GetP205AgronomicBalance(Field field, Region region);
    }

    public class FeedAreaCalculator : IFeedAreaCalculator
    {
        private readonly AgriConfigurationRepository _repo;
        private readonly AgriConfigurationContext _db;
        private readonly decimal _matureFeedEfficiency;
        private readonly decimal _growingFeedEfficiency;

        public FeedAreaCalculator(AgriConfigurationRepository repo, AgriConfigurationContext db)
        {
            _repo = repo;
            _db = db;
        }

        public decimal GetNitrogenAgronomicBalance(Field field, Region region)
        {
            if (!field.IsSeasonalFeedingArea)
            {
                return 0m;
            }

            //var dailyFeedRequirementForMatureAnimals = _db.DailyFeedRequirements.Single(d => d.Id == field.MatureAnimalDailyFeedRequirementId).Value;
            //var dailyFeedRequirementForGrowingAnimals = _db.DailyFeedRequirements.Single(d => d.Id == field.GrowingAnimalDailyFeedRequirementId).Value;
            var feedEfficiencies = _db.FeedEfficiencies.ToList();
            var matureFeedEfficiency = feedEfficiencies.Single(f => f.AnimalType.Contains("mature", StringComparison.OrdinalIgnoreCase)).Nitrogen;
            var growingFeedEfficiency = feedEfficiencies.Single(f => f.AnimalType.Contains("growing", StringComparison.OrdinalIgnoreCase)).Nitrogen;
            var NMineralizationFirstYearValue = _repo
                .GetNitrogeMineralizations()
                .Single(nm => nm.LocationId == region.LocationId &&
                                nm.Name.Contains("Solid - Other (<50%)", StringComparison.OrdinalIgnoreCase))
                .FirstYearValue;

            var matureAnimalFactor = GetMatureAnimalFactor(field);
            //if (field.MatureAnimalCount.HasValue && field.GrowingAnimalAverageWeight.HasValue &&
            //    field.FeedingDaysSpentInFeedingArea.HasValue && field.FeedingPercentageOutsideFeeingArea.HasValue)
            //{
            //    matureAnimalFactor =
            //        (field.MatureAnimalAverageWeight.Value * dailyFeedRequirementForMatureAnimals / 100) *
            //            field.MatureAnimalCount.Value * field.FeedingDaysSpentInFeedingArea.Value *
            //                ((100 - field.FeedingPercentageOutsideFeeingArea.Value) / 100);
            //}

            var growingAnimalFactor = GetGrowingAnimalFactor(field);
            //if (field.GrowingAnimalCount.HasValue && field.GrowingAnimalAverageWeight.HasValue &&
            //    field.FeedingDaysSpentInFeedingArea.HasValue && field.FeedingPercentageOutsideFeeingArea.HasValue)
            //{
            //    growingAnimalFactor =
            //        (field.GrowingAnimalAverageWeight.Value * dailyFeedRequirementForGrowingAnimals / 100) *
            //        field.GrowingAnimalCount.Value * field.FeedingDaysSpentInFeedingArea.Value *
            //            ((100 - field.FeedingPercentageOutsideFeeingArea.Value) / 100);
            //}
            var summation = 0M;

            foreach (var analytic in field.FeedForageAnalyses)
            {
                summation +=
                    ((matureAnimalFactor *
                        (analytic.CrudeProteinPercent / 6.25M / 100M * matureFeedEfficiency))
                    +

                    (growingAnimalFactor *
                        (analytic.CrudeProteinPercent / 6.25M / 100M * growingFeedEfficiency)))
                    *
                    (analytic.PercentOfTotalFeedForageToAnimals / 100) * ((100 + analytic.PercentOfFeedForageWastage) / 100)
                    *
                    ((100 - field.FeedingPercentageOutsideFeeingArea.GetValueOrDefault(0) / 100) * NMineralizationFirstYearValue);
            }

            var result = summation / (field.Area * 1M);

            return Math.Round(result, 0);
        }

        public decimal GetP205AgronomicBalance(Field field, Region region)
        {
            //(((
            //  (
            //      (mature animal average weight * daily feed requirement for mature animals/ 100) *
            //          number of mature animals* number of days spent in seas feeding area *
            //          ((100 - % of time spent outside seas feeding area for mature animals) / 100) *
            //          (% P for feed 1 / 100 * mature animal feed efficiency constant for P))
            //          +
            //          ((growing animal average weight* daily feed requirement for growing animals/ 100) *
            //          number of growing animals* number of days spend in seas feeding area *
            //          ((100 - % of time spent outside seas feeding area for mature animals)/ 100)
            //          *
            //          (% P for feed 1 / 100 * growing animal feed efficiency constant for P)
            //      )
            //  )
            //   *
            //  (% of total feedforage for feed 1 / 100)
            //   *
            //  ((100 - % of time spent outside seas feeding area)/ 100) * 0.7 * 2.29)
            //          +
            //          ((((mature animal average weight * daily feed requirement for mature animals/ 100) *
            //          number of mature animals* number of days spent in seas feeding area *
            //          ((100 - % of time spent outside seas feeding area for mature animals)/ 100) *
            //          (% P for feed 2 / 100 * mature animal feed efficiency constant for P))
            //          +
            //          ((growing animal average weight* daily feed requirement for growing animals/ 100) *
            //          number of growing animals* number of days spend in seas feeding area *
            //          ((100 - % of time spent outside seas feeding area for mature animals)/ 100) *
            //          (% P for feed 2 / 100 * growing animal feed efficiency constant for P))) *
            //          (% of total feedforage for feed 2 / 100) *
            //          ((100 - % of time spent outside seas feeding area) / 100) * 0.7 * 2.29)
            //           +
            //           ((((mature animal average weight* daily feed requirement for mature animals/ 100) *
            //          number of mature animals* number of days spent in seas feeding area *
            //          ((100 - % of time spent outside seas feeding area for mature animals)/ 100) *
            //          (% P for feed 3 / 100 * mature animal feed efficiency constant for P))
            //          +
            //          ((growing animal average weight* daily feed requirement for growing animals/ 100) *
            //          number of growing animals* number of days spend in seas feeding area *
            //          ((100 - % of time spent outside seas feeding area for mature animals)/ 100) *
            //          (% P for feed 3 / 100 * growing animal feed efficiency constant for P))) *
            //          (% of total feedforage for feed 3 / 100) * ((100 - % of time spent outside seas feeding area) / 100) * 0.7 * 2.29))
            // / field size

            var result = 0M;

            return result;
        }

        public decimal GetK20AgronomicBalance(Field field, Region region)
        {
            var result = 0M;

            return result;
        }

        private decimal GetMatureAnimalFactor(Field field)
        {
            var result = 0m;

            if (field.MatureAnimalAverageWeight.HasValue && field.MatureAnimalCount.HasValue)
            {
                var feedRequirement = _db.DailyFeedRequirements.Single(d => d.Id == field.MatureAnimalDailyFeedRequirementId).Value;
                result = CalculateAnimalFactor(field.MatureAnimalAverageWeight.Value, field.MatureAnimalCount.Value, feedRequirement, field);
            }

            return result;
        }

        private decimal GetGrowingAnimalFactor(Field field)
        {
            var result = 0m;

            if (field.GrowingAnimalAverageWeight.HasValue && field.GrowingAnimalCount.HasValue)
            {
                var feedRequirement = _db.DailyFeedRequirements.Single(d => d.Id == field.GrowingAnimalDailyFeedRequirementId).Value;
                result = CalculateAnimalFactor(field.GrowingAnimalAverageWeight.Value, field.GrowingAnimalCount.Value, feedRequirement, field);
            }

            return result;
        }

        private decimal CalculateAnimalFactor(decimal animalWieght, decimal animalCount, decimal feedRequirement, Field field)
        {
            var result = 0M;

            if (field.FeedingDaysSpentInFeedingArea.HasValue && field.FeedingPercentageOutsideFeeingArea.HasValue)
            {
                result =
                (animalWieght * feedRequirement / 100) *
                    animalCount * field.FeedingDaysSpentInFeedingArea.Value *
                        ((100 - field.FeedingPercentageOutsideFeeingArea.Value) / 100);
            }

            return result;
        }
    }
}