using System.ComponentModel.DataAnnotations;

namespace Agri.Models.StaticData
{
    public class CropSTKRegionCd
    {
        [Key]
        public int CropId { get; set; }
        [Key]
        public int SoilTestPotassiumRegionCd { get; set; }
        public int? PotassiumCropGroupRegionCd { get; set; }
    }
}