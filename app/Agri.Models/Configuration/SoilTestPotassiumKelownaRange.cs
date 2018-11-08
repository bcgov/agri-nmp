using System.Collections.Generic;

namespace Agri.Models.Configuration
{
    public class SoilTestPotassiumKelownaRange
    {
        public SoilTestPotassiumKelownaRange()
        {
            SoilTestPotassiumRecommendations = new List<SoilTestPotassiumRecommendation>();
        }
        public int Id { get; set; }
        public string Range { get; set; }
        public int RangeLow { get; set; }
        public int RangeHigh { get; set; }

        public List<SoilTestPotassiumRecommendation> SoilTestPotassiumRecommendations { get; set; }
    }
}