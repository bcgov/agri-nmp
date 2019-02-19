using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class DensityUnit : Versionable
    {
        public DensityUnit()
        {
            LiquidFertilizerDensities = new List<LiquidFertilizerDensity>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal ConvFactor { get; set; }

        public List<LiquidFertilizerDensity> LiquidFertilizerDensities { get; set; }
    }
}