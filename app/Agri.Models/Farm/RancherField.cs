using System.Collections.Generic;

namespace Agri.Models.Farm
{
    public class RancherField
    {
        public RancherField()
        {
            Crops = new List<RancherFieldCrop>();
        }

        public int Id { get; set; }
        public string FieldName { get; set; }
        public decimal Area { get; set; }
        public string Comment { get; set; }
        public Nutrients Nutrients { get; set; }
        public List<RancherFieldCrop> Crops { get; set; }
        public SoilTest SoilTest { get; set; }
        public string PrevYearManureApplicationFrequency { get; set; }
        public int? PrevYearManureApplicationNitrogenCredit { get; set; }
        public decimal? SoilTestNitrateOverrideNitrogenCredit { get; set; }
        public bool IsSeasonalFeedingArea { get; set; }
        public string SeasonalFeedingArea { get; set; }
    }
}