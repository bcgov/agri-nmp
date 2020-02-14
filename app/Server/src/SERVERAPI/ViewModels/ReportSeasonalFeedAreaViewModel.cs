﻿using Agri.Models.Farm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVERAPI.ViewModels
{
    public class ReportSeasonalFeedAreaViewModel
    {
        public List<Field> Fields { get; set; }

        public class Field
        {
            public int Id { get; set; }
            public string FieldName { get; set; }

            public decimal? FieldArea { get; set; }
            public string FieldComment { get; set; }

            public bool IsSeasonalFeedingArea { get; set; }
            public int? FeedingDaysSpentInFeedingArea { get; set; }
            public int? FeedingPercentageOutsideFeeingArea { get; set; }
            public int? MatureAnimalCount { get; set; }
            public int? GrowingAnimalCount { get; set; }
            public int? MatureAnimalAverageWeight { get; set; }
            public int? GrowingAnimalAverageWeight { get; set; }
            public int? MatureAnimalDailyFeedRequirementId { get; set; }
            public decimal? MatureAnimalDailyFeedRequirement { get; set; }
            public string MatureAnimalDailyFeedRequirementName { get; set; }

            public int CalculatedMatureFeedRequirement
            {
                get
                {
                    return Convert.ToInt32(MatureAnimalDailyFeedRequirement.GetValueOrDefault(0) *
                        MatureAnimalAverageWeight.GetValueOrDefault(0) *
                        FeedingDaysSpentInFeedingArea.GetValueOrDefault(0));
                }
            }

            public int? GrowingAnimalDailyFeedRequirementId { get; set; }
            public decimal? GrowingAnimalDailyFeedRequirement { get; set; }
            public string GrowingAnimalDailyFeedRequirementName { get; set; }

            public int CalculatedGrowingFeedRequirement
            {
                get
                {
                    return Convert.ToInt32(GrowingAnimalDailyFeedRequirement.GetValueOrDefault(0) *
                        GrowingAnimalAverageWeight.GetValueOrDefault(0) *
                        FeedingDaysSpentInFeedingArea.GetValueOrDefault(0));
                }
            }

            public List<FeedForageAnalysis> FeedForageAnalyses { get; set; } = new List<FeedForageAnalysis>();
            public decimal NAgroBalance { get; set; }
            public decimal P205AgroBalance { get; set; }
            public decimal K20AgroBalance { get; set; }
            public decimal NCropRemovalValue { get; set; }
            public decimal P205CropRemovalValue { get; set; }
            public decimal K20CropRemovalValue { get; set; }
        }

        public class FeedForageAnalysis
        {
            private decimal? crudeProteinPercent;
            private decimal? phosphorus;
            private decimal? potassium;
            private decimal? percentOfTotalFeedForageToAnimals;
            private decimal? percentOfFeedForageWastage;

            public int Id { get; set; }
            public int? FeedForageTypeId { get; set; }
            public int? FeedForageId { get; set; }
            public string FeedForageType { get; set; }
            public bool UseBookValues { get; set; } = true;

            public decimal? CrudeProteinPercent
            {
                get => crudeProteinPercent.HasValue ? Math.Round(crudeProteinPercent.Value, 2) : default(decimal?);
                set => crudeProteinPercent = value;
            }

            public decimal? Phosphorus
            {
                get => phosphorus.HasValue ? Math.Round(phosphorus.Value, 2) : default(decimal?);
                set => phosphorus = value;
            }

            public decimal? Potassium
            {
                get => potassium.HasValue ? Math.Round(potassium.Value, 2) : default(decimal?);
                set => potassium = value;
            }

            public decimal? PercentOfTotalFeedForageToAnimals
            {
                get => percentOfTotalFeedForageToAnimals.HasValue ? Math.Round(percentOfTotalFeedForageToAnimals.Value, 0) : default(decimal?);
                set => percentOfTotalFeedForageToAnimals = value;
            }

            public decimal? PercentOfFeedForageWastage
            {
                get => percentOfFeedForageWastage.HasValue ? Math.Round(percentOfFeedForageWastage.Value, 0) : default(decimal?);
                set => percentOfFeedForageWastage = value;
            }
        }
    }
}