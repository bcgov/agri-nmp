namespace Agri.Models
{
    public class LiquidFertilizerDensity
    {
        public int Id { get; set; }
        public int FertilizerId { get; set; }
        public int DensityUnitId { get; set; }
        public decimal Value { get; set; }

        public Fertilizer Fertilizer { get; set; }
        public DensityUnit DensityUnit { get; set; }
    }
}