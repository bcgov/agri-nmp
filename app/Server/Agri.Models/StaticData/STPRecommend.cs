using System.ComponentModel.DataAnnotations;

namespace Agri.Models.StaticData
{
    public class STPRecommend
    {
        [Key]
        public int STPKelownaRangeId { get; set; }
        public int SoilTestPhosphorousRegionCd { get; set; }
        public int PhosphorousCropGroupRegionCd { get; set; }
        public int P2O5_Recommend_KgPerHa { get; set; }

        public STPKelownaRange StpKelownaRange { get; set; }
    }
}