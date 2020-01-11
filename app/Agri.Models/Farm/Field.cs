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
        public string FieldName { get; set; }
        public decimal Area { get; set; }
        public string Comment { get; set; }
        public Nutrients Nutrients { get; set; }
        public List<FieldCrop> crops { get; set; }
        public SoilTest SoilTest { get; set; }
        public string PreviousYearManureApplicationFrequency { get; set; }
        public int? PreviousYearManureApplicationNitrogenCredit { get; set; }
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