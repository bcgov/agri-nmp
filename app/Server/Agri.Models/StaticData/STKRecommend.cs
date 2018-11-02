using System.ComponentModel.DataAnnotations;

namespace Agri.Models.StaticData
{
    public class STKRecommend
    {
        [Key]
        public int STKKelownaRangeId { get; set; }
        [Key]
        public int SoilTestPotassiumRegionCode { get; set; }
        [Key]
        public int PotassiumCropGroupRegionCode { get; set; }
        public int K2O_Recommend_kgPeHa { get; set; }

        public STKKelownaRange STKKelownaRange { get; set; }
    }
}