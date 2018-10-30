using System.Collections.Generic;

namespace Agri.Models.StaticData
{
    public class Unit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NutrientContentUnits { get; set; }
        public decimal Conversion_lbTon { get; set; }
        public string NutrientRateUnits { get; set; }
        public string CostUnits { get; set; }
        public decimal CostApplications { get; set; }
        public string DollarUnitArea { get; set; }
        public string ValueMaterialUnits { get; set; }
        public decimal Value_N { get; set; }
        public decimal Value_P2O5 { get; set; }

        public decimal Value_K2O { get; set; }

        // conversion factor to the units displayed in the section, Manure and Fertilizer Required, of the Complete Report.
        // does not consider the area 
        public decimal FarmReqdNutrientsStdUnitsConversion { get; set; }
        public decimal FarmReqdNutrientsStdUnitsAreaConversion { get; set; }
        public string SolidLiquid { get; set; }
    }
}