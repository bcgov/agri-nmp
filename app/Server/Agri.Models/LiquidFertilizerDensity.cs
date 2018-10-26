namespace Agri.Models
{
    public class LiquidFertilizerDensity
    {
        public int id { get; set; }
        public int fertilizerId { get; set; }
        public int densityUnitId { get; set; }
        public decimal value { get; set; }
    }
}