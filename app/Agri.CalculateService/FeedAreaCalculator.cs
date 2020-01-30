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

            //((
            //  (
            //      (

            //          (mature animal average weight * daily feed requirement for mature animals/ 100) *
            //              number of mature animals * number of days spent in seas feeding area *
            //              ((100 - % of time spent outside seas feeding area for mature animals) / 100) *
            //              (Crude protein for feed 1 / 6.25 / 100 * mature animal feed efficiency constant for N)
            //      )
            //       +
            //      (
            //          (growing animal average weight * daily feed requirement for growing animals/ 100) *
            //              number of growing animals * number of days spend in seas feeding area *
            //              ((100 - % of time spent outside seas feeding area for mature animals)/ 100) *
            //              (crude protein for feed 1 / 6.25 / 100 * growing animal feed efficiency constant for N)
            //      )
            //  )
            //       *
            //      (% of total feedforage for feed 1 / 100)
            //       *
            //      ((100 - % of time spent outside seas feeding area)/ 100) * N mineralization constant for short term)
            //       +
            //      (
            //          (
            //              (
            //                  (mature animal average weight * daily feed requirement for mature animals/ 100) *
            //                      number of mature animals * number of days spent in seas feeding area *
            //                      ((100 - % of time spent outside seas feeding area for mature animals) / 100) *
            //                      (Crude protein for feed 2 / 6.25 / 100 * mature animal feed efficiency constant for N)
            //              )
            //              +
            //              (
            //                  (growing animal average weight * daily feed requirement for growing animals/ 100) *
            //                      number of growing animals * number of days spend in seas feeding area *
            //                      ((100 - % of time spent outside seas feeding area for mature animals) / 100) *
            //                      (crude protein for feed 2 / 6.25 / 100 * growing animal feed efficiency constant for N)
            //              )
            //          )
            //           *
            //          (% of total feedforage for feed 2 / 100)
            //           *
            //          ((100 - % of time spent outside seas feeding area) / 100) * N mineralization constant for short term)
            //           +
            //          (
            //              (
            //                  (
            //                      (mature animal average weight * daily feed requirement for mature animals/ 100) *
            //                          number of mature animals * number of days spent in seas feeding area *
            //                          ((100 - % of time spent outside seas feeding area for mature animals)/ 100) *
            //                          (Crude protein for feed 3 / 6.25 / 100 * mature animal feed efficiency constant for N)
            //                  )
            //                   +
            //                  (
            //                      (growing animal average weight * daily feed requirement for growing animals/ 100) *
            //                          number of growing animals * number of days spend in seas feeding area *
            //                          ((100 - % of time spent outside seas feeding area for mature animals)/ 100) *
            //                          (crude protein for feed 3 / 6.25 / 100 * growing animal feed efficiency constant for N)
            //                  )
            //              )
            //               *
            //              (% of total feedforage for feed 3 / 100)
            //               *
            //              ((100 - % of time spent outside seas feeding area)/ 100) * N mineralization constant for short term)
            //      )
            //      / field size

            var dailyFeedRequirementForMatureAnimals = _db.DailyFeedRequirements.Single(d => d.Id == field.MatureAnimalDailyFeedRequirementId).Value;
            var feedEfficiencies = _db.FeedEfficiencies.ToList();
            var matureFeedEfficiency = feedEfficiencies.Single(f => f.AnimalType.Contains("mature", StringComparison.OrdinalIgnoreCase));
            var growingFeedEfficiency = feedEfficiencies.Single(f => f.AnimalType.Contains("growing", StringComparison.OrdinalIgnoreCase));
            var NMineralization = _repo
                .GetNitrogeMineralizations()
                .Single(nm => nm.LocationId == region.LocationId &&
                                nm.Name.Contains("Solid - Other (<50%)", StringComparison.OrdinalIgnoreCase));

            var result = 0M;
            //(
            //    (field.MatureAnimalAverageWeight.Value * dailyFeedRequirementForMatureAnimals / 100) *
            //        field.MatureAnimalCount * field.FeedingDaysSpentInFeedingArea *
            //            ((100 - field.FeedingPercentageOutsideFeeingArea) / 100) *
            //                ()

            //)
            /// (field.Area * 1M);

            return result;
        }

        public decimal GetP205AgronomicBalance(Field field, Region region)
        {
            var result = 0M;

            return result;
        }

        public decimal GetK20AgronomicBalance(Field field, Region region)
        {
            var result = 0M;

            return result;
        }
    }
}