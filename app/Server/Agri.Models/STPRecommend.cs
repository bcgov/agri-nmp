namespace Agri.Models
{
    public class STPRecommend
    {
        public int STPKelownaRangeId { get; set; }
        public int SoilTestPhosphorousRegionCd { get; set; }
        public int PhosphorousCropGroupRegionCd { get; set; }
        public int P2O5_Recommend_KgPerHa { get; set; }

        public STPKelownaRange StpKelownaRange { get; set; }
    }
}