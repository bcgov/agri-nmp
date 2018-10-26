namespace Agri.Models
{
    public class FertilizerUnit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DryLiquid { get; set; }

        public decimal ConvToImpGalPerAc { get; set; }

        // conversion factor to the units displayed in the section, Manure and Fertilizer Required, of the Complete Report.
        // does not consider the area 
        public decimal FarmReqdNutrientsStdUnitsConversion { get; set; }
        public decimal FarmReqdNutrientsStdUnitsAreaConversion { get; set; }


    }
}