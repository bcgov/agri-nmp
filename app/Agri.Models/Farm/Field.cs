using System.Collections.Generic;

namespace Agri.Models.Farm
{
    public class Field
    {
        public Field()
        {
            Crops = new List<FieldCrop>();
        }

        public int Id { get; set; }
        public string FieldName { get; set; }
        public decimal Area { get; set; }
        public string Comment { get; set; }
        public Nutrients Nutrients { get; set; }
        public bool HasNutrients => Nutrients != null;
        public List<FieldCrop> crops { get; set; } = new List<FieldCrop>();
        public List<FeedForageAnalysis> FeedForageAnalyses { get; set; }
        public SoilTest SoilTest { get; set; }
        public bool HasSoilTest => SoilTest != null;
        public string PreviousYearManureApplicationFrequency { get; set; }
        public int? PreviousYearManureApplicationNitrogenCredit { get; set; }
        public decimal? SoilTestNitrateOverrideNitrogenCredit { get; set; }
        public bool IsSeasonalFeedingArea { get; set; }
        public string SeasonalFeedingArea { get; set; }
        public decimal? FeedingValueDays { get; set; }
        public decimal? FeedingPercentage { get; set; }
        public decimal? MatureAnimalCount { get; set; }
        public decimal? GrowingAnimalCount { get; set; }
        public decimal? MatureAnimalAverage { get; set; }
        public decimal? GrowingAnimalAverage { get; set; }
        public string SelectMatureAnimalDailyFeed { get; set; }
        public string SelectGrowingAnimalDailyFeed { get; set; }
    }
}