using System.Collections.Generic;

namespace Agri.Models.Farm
{
    public class Field
    {
        public Field()
        {
            crops = new List<FieldCrop>();
        }

        public int Id { get; set; }
        public string fieldName { get; set; }
        public decimal area { get; set; }
        public string comment { get; set; }
        public Nutrients nutrients { get; set; }
        public List<FieldCrop> crops { get; set; }
        public SoilTest soilTest { get; set; }
        public string prevYearManureApplicationFrequency { get; set; }
        public int? prevYearManureApplicationNitrogenCredit { get; set; }
        public decimal? SoilTestNitrateOverrideNitrogenCredit { get; set; }
        public bool IsSeasonalFeedingArea { get; set; }
        public string SeasonalFeedingArea { get; set; }
        public int? FeedingValueDays { get; set; }
        public decimal? FeedingPercentage { get; set; }
        public int? MatureAnimalCount { get; set; }
        public int? GrowingAnimalCount { get; set; }
        public decimal? MatureAnimalAverage { get; set; }
        public decimal? GrowingAnimalAverage { get; set; }
    }
}