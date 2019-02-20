using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class SoilTestPotassiumKelownaRange : Versionable
    {
        public SoilTestPotassiumKelownaRange()
        {
            SoilTestPotassiumRecommendations = new List<SoilTestPotassiumRecommendation>();
        }
        [Key]
        public int Id { get; set; }
        public string Range { get; set; }
        public int RangeLow { get; set; }
        public int RangeHigh { get; set; }

        public List<SoilTestPotassiumRecommendation> SoilTestPotassiumRecommendations { get; set; }
    }
}