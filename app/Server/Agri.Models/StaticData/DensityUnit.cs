using System.Collections.Generic;

namespace Agri.Models.StaticData
{
    public class DensityUnit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal ConvFactor { get; set; }

        public List<LiquidFertilizerDensity> LiquidFertilizerDensities { get; set; }
    }
}