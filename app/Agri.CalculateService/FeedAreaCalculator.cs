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
        private readonly IAgriConfigurationRepository _repo;
        private readonly AgriConfigurationContext _db;

        public FeedAreaCalculator(IAgriConfigurationRepository repo, AgriConfigurationContext db)
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

            var feedEfficiencies = _db.FeedEfficiencies.ToList();
            var matureFeedEfficiency = feedEfficiencies.Single(f => f.AnimalType.Contains("mature", StringComparison.OrdinalIgnoreCase)).Nitrogen;
            var growingFeedEfficiency = feedEfficiencies.Single(f => f.AnimalType.Contains("growing", StringComparison.OrdinalIgnoreCase)).Nitrogen;
            var NMineralizationFirstYearValue = _repo
                .GetNitrogeMineralizations()
                .Single(nm => nm.LocationId == region.LocationId &&
                                nm.Name.Contains("Solid - Other (<50%)", StringComparison.OrdinalIgnoreCase))
                .FirstYearValue;

            var matureAnimalFactor = GetMatureAnimalFactor(field);

            var growingAnimalFactor = GetGrowingAnimalFactor(field);

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
            if (!field.IsSeasonalFeedingArea)
            {
                return 0m;
            }

            var feedEfficiencies = _db.FeedEfficiencies.ToList();
            var matureFeedEfficiency = feedEfficiencies.Single(f => f.AnimalType.Contains("mature", StringComparison.OrdinalIgnoreCase)).Phosphorous;
            var growingFeedEfficiency = feedEfficiencies.Single(f => f.AnimalType.Contains("growing", StringComparison.OrdinalIgnoreCase)).Phosphorous;

            var matureAnimalFactor = GetMatureAnimalFactor(field);

            var growingAnimalFactor = GetGrowingAnimalFactor(field);

            var summation = 0M;

            foreach (var analytic in field.FeedForageAnalyses)
            {
                summation +=
                    ((matureAnimalFactor *
                        (analytic.Phosphorus / 100M * matureFeedEfficiency))
                    +

                    (growingAnimalFactor *
                        (analytic.Phosphorus / 100M * growingFeedEfficiency)))
                    *
                    (analytic.PercentOfTotalFeedForageToAnimals / 100) * ((100 + analytic.PercentOfFeedForageWastage) / 100)
                    *
                    ((100 - field.FeedingPercentageOutsideFeeingArea.GetValueOrDefault(0) / 100) * 0.7M * 2.29M);
            }

            var result = summation / (field.Area * 1M);

            return Math.Round(result, 0);
        }

        public decimal GetK20AgronomicBalance(Field field, Region region)
        {
            if (!field.IsSeasonalFeedingArea)
            {
                return 0m;
            }

            var feedEfficiencies = _db.FeedEfficiencies.ToList();
            var matureFeedEfficiency = feedEfficiencies.Single(f => f.AnimalType.Contains("mature", StringComparison.OrdinalIgnoreCase)).Potassium;
            var growingFeedEfficiency = feedEfficiencies.Single(f => f.AnimalType.Contains("growing", StringComparison.OrdinalIgnoreCase)).Potassium;

            var matureAnimalFactor = GetMatureAnimalFactor(field);

            var growingAnimalFactor = GetGrowingAnimalFactor(field);

            var summation = 0M;

            foreach (var analytic in field.FeedForageAnalyses)
            {
                summation +=
                    ((matureAnimalFactor *
                        (analytic.Potassium / 100M * matureFeedEfficiency))
                    +

                    (growingAnimalFactor *
                        (analytic.Potassium / 100M * growingFeedEfficiency)))
                    *
                    (analytic.PercentOfTotalFeedForageToAnimals / 100) * ((100 + analytic.PercentOfFeedForageWastage) / 100)
                    *
                    ((100 - field.FeedingPercentageOutsideFeeingArea.GetValueOrDefault(0) / 100) * 1.21M);
            }

            var result = summation / (field.Area * 1M);

            return Math.Round(result, 0);
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