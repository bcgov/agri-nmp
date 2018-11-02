namespace Agri.Models.StaticData
{
    public class FertilizerUnit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DryLiquid { get; set; }

        public decimal ConversionToImperialGallonsPerAcre { get; set; }

        // conversion factor to the units displayed in the section, Manure and Fertilizer Required, of the Complete Report.
        // does not consider the area 
        public decimal FarmRequiredNutrientsStdUnitsConversion { get; set; }
        public decimal FarmRequiredNutrientsStdUnitsAreaConversion { get; set; }

    }
}