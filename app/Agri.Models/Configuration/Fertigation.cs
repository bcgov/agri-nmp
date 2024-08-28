using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class Fertigation
    {
        public Fertigation()
        {
            // LiquidFertilizerDensities = new List<LiquidFertilizerDensity>();
        }

        public List<Fertilizer> Fertilizers { get; set; }
        public List<LiquidFertilizerDensity> LiquidFertilizerDensities { get; set; }
        public List<LiquidMaterialApplicationUSGallonsPerAcreRateConversion> LiquidMaterialApplicationUsGallonsPerAcreRateConversions { get; set; }
        public List<LiquidMaterialsConversionFactor> LiquidMaterialsConversionFactors { get; set; }
        public List<FertilizerType> FertilizerTypes { get; set; }
        public List<ProductRateUnit> ProductRateUnits { get; set; }
        public List<DensityUnit> DensityUnits { get; set; }
        public List<InjectionRateUnit> InjectionRateUnits { get; set; }

    }
}
