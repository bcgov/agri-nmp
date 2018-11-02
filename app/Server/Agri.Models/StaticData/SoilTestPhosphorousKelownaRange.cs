using System.Collections.Generic;

namespace Agri.Models.StaticData
{
    public class SoilTestPhosphorousKelownaRange
    {
        public SoilTestPhosphorousKelownaRange()
        {
            SoilTestPhosphorousRecommendations = new List<SoilTestPhosphorousRecommendation>();
        }
        public int Id { get; set; }
        public string Range { get; set; }
        public int RangeLow { get; set; }
        public int RangeHigh { get; set; }

        public List<SoilTestPhosphorousRecommendation> SoilTestPhosphorousRecommendations { get; set; }
    }
}