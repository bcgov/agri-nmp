using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class SoilTestPhosphorousKelownaRange : Versionable
    {
        
        public SoilTestPhosphorousKelownaRange()
        {
            SoilTestPhosphorousRecommendations = new List<SoilTestPhosphorousRecommendation>();
        }
        [Key]
        public int Id { get; set; }
        public string Range { get; set; }
        public int RangeLow { get; set; }
        public int RangeHigh { get; set; }

        public List<SoilTestPhosphorousRecommendation> SoilTestPhosphorousRecommendations { get; set; }
    }
}