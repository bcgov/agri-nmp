using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class CropSoilTestPotassiumRegion
    {
        [Key]
        public int CropId { get; set; }
        [Key]
        public int SoilTestPotassiumRegionCode { get; set; }
        public int? PotassiumCropGroupRegionCode { get; set; }
    }
}