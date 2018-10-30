using System.ComponentModel.DataAnnotations;

namespace Agri.Models.StaticData
{
    public class STKRecommend
    {
        [Key]
        public int STKKelownaRangeId { get; set; }
        public int SoilTestPotassiumRegionCd { get; set; }
        public int PotassiumCropGroupRegionCd { get; set; }
        public int K2O_Recommend_kgPeHa { get; set; }

        public STKKelownaRange STKKelownaRange { get; set; }
    }
}