using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class CropSoilTestPhosphorousRegion : ConfigurationBase
    {
        [Key]
        public int CropId { get; set; }
        [Key]
        public int SoilTestPhosphorousRegionCode { get; set; }
        public int? PhosphorousCropGroupRegionCode { get; set; }

        public Crop Crop { get; set; }
    }
}