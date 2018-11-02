using System.ComponentModel.DataAnnotations;

namespace Agri.Models.StaticData
{
    public class CropYield
    {
        [Key]
        public int CropId { get; set; }
        [Key]
        public int LocationId { get; set; }
        public decimal? Amount { get; set; }
        public Crop Crop { get; set; }
        public Location Location { get; set; }
    }
}