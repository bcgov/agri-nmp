namespace Agri.Models
{
    public class CropYield
    {
        public int CropId { get; set; }
        public int LocationId { get; set; }
        public decimal? Amt { get; set; }
        public Crop Crop { get; set; }
        public Location Location { get; set; }
    }
}