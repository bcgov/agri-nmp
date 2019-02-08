using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class SoilTestPhosphorousRecommendation : ConfigurationBase
    {
        [Key]
        public int SoilTestPhosphorousKelownaRangeId { get; set; }
        [Key]
        public int SoilTestPhosphorousRegionCode { get; set; }
        [Key]
        public int PhosphorousCropGroupRegionCode { get; set; }
        public int P2O5RecommendationKilogramPerHectare { get; set; }

        public SoilTestPhosphorousKelownaRange SoilTestPhosphorousKelownaRange { get; set; }
    }
}