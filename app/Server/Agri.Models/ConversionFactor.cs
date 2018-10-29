namespace Agri.Models
{
    public class ConversionFactor
    {
        public int Id { get; set; }
        public decimal NProteinConversion { get; set; }
        public decimal UnitConversion { get; set; }
        public int DefaultSoilTestKelownaP { get; set; }
        public int DefaultSoilTestKelownaK { get; set; }
        public decimal KgPerHa_lbPerAc_Conversion { get; set; }
        public decimal PotassiumAvailabilityFirstYear { get; set; }
        public decimal PotassiumAvailabilityLongTerm { get; set; }
        public decimal PotassiumKtoK2Oconversion { get; set; }
        public decimal PhosphorousAvailabilityFirstYear { get; set; }
        public decimal PhosphorousAvailabilityLongTerm { get; set; }
        public decimal PhosphorousPtoP2O5KConversion { get; set; }
        public decimal lbPerTonConversion { get; set; }
        public decimal lbPer1000ftSquared_lbPerAc_Conversion { get; set; }
        public string DefaultApplicationOfManureInPrevYears { get; set; }
    }
}