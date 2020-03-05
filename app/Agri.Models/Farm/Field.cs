﻿using System.Collections.Generic;

namespace Agri.Models.Farm
{
    public class Field
    {
        public Field()
        {
            Crops = new List<FieldCrop>();
            FeedForageAnalyses = new List<FeedForageAnalysis>();
        }

        public int Id { get; set; }
        public string FieldName { get; set; }
        public decimal Area { get; set; }
        public string Comment { get; set; }
        public Nutrients Nutrients { get; set; }
        public bool HasNutrients => Nutrients != null;
        public List<FieldCrop> Crops { get; set; } = new List<FieldCrop>();
        public List<FeedForageAnalysis> FeedForageAnalyses { get; set; } = new List<FeedForageAnalysis>();
        public SoilTest SoilTest { get; set; }
        public bool HasSoilTest => SoilTest != null;
        public string PreviousYearManureApplicationFrequency { get; set; }
        public int? PreviousYearManureApplicationNitrogenCredit { get; set; }
        public decimal? SoilTestNitrateOverrideNitrogenCredit { get; set; }
        public bool IsSeasonalFeedingArea { get; set; }
        public string SeasonalFeedingArea { get; set; }
        public decimal? FeedingDaysSpentInFeedingArea { get; set; }
        public decimal? FeedingPercentageOutsideFeeingArea { get; set; }
        public decimal? MatureAnimalCount { get; set; }
        public decimal? GrowingAnimalCount { get; set; }
        public decimal? MatureAnimalAverageWeight { get; set; }
        public decimal? GrowingAnimalAverageWeight { get; set; }
        public int? MatureAnimalDailyFeedRequirementId { get; set; }
        public int? GrowingAnimalDailyFeedRequirementId { get; set; }
    }
}