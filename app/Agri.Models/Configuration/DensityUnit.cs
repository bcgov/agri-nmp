using System.Collections.Generic;

namespace Agri.Models.Configuration
{
    public class DensityUnit
    {
        public DensityUnit()
        {
            LiquidFertilizerDensities = new List<LiquidFertilizerDensity>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal ConvFactor { get; set; }

        public List<LiquidFertilizerDensity> LiquidFertilizerDensities { get; set; }
    }
}