using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class DensityUnit : SelectOption
    {
        public DensityUnit()
        {
            LiquidFertilizerDensities = new List<LiquidFertilizerDensity>();
        }

        public decimal ConvFactor { get; set; }

        public List<LiquidFertilizerDensity> LiquidFertilizerDensities { get; set; }
    }
}