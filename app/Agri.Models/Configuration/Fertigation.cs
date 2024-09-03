using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Agri.Models.Configuration
{
    public class Fertigation
    {
        public Fertigation()
        {
            
        }

        public List<Fertilizer> Fertilizers { get; set; }
        public List<LiquidFertilizerDensity> LiquidFertilizerDensities { get; set; }
        public List<LiquidMaterialApplicationUSGallonsPerAcreRateConversion> LiquidMaterialApplicationUsGallonsPerAcreRateConversions { get; set; }
        public List<LiquidMaterialsConversionFactor> LiquidMaterialsConversionFactors { get; set; }
        public List<FertilizerType> FertilizerTypes { get; set; }
        public List<ProductRateUnit> ProductRateUnits { get; set; }
        public List<DensityUnit> DensityUnits { get; set; }
        public List<InjectionRateUnit> InjectionRateUnits { get; set; }
        public List<FertilizerUnit> FertilizerUnit { get; set; }

        public LiquidFertilizerDensity GetLiquidFertilizerDensity( int id, int densityUnitId){
            return LiquidFertilizerDensities.Single(density => density.FertilizerId == id && density.DensityUnitId == densityUnitId);
        }

    }
}
