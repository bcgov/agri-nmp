namespace Agri.Models
{
    public class ConversionFactor
    {
        public decimal n_protein_conversion { get; set; }
        public decimal unit_conversion { get; set; }
        public int defaultSoilTestKelownaP { get; set; }
        public int defaultSoilTestKelownaK { get; set; }
        public decimal kgperha_lbperac_conversion { get; set; }
        public decimal potassiumAvailabilityFirstYear { get; set; }
        public decimal potassiumAvailabilityLongTerm { get; set; }
        public decimal potassiumKtoK2Oconversion { get; set; }
        public decimal phosphorousAvailabilityFirstYear { get; set; }
        public decimal phosphorousAvailabilityLongTerm { get; set; }
        public decimal phosphorousPtoP2O5Kconversion { get; set; }
        public decimal lbPerTonConversion { get; set; }
        public decimal lbper1000ftsquared_lbperac_conversion { get; set; }
        public string defaultApplicationOfManureInPrevYears { get; set; }
    }
}