using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class ConversionFactor : ConfigurationBase
    {
        [Key]
        public int Id { get; set; }
        public decimal NitrogenProteinConversion { get; set; }
        public decimal UnitConversion { get; set; }
        public int DefaultSoilTestKelownaPhosphorous { get; set; }
        public int DefaultSoilTestKelownaPotassium { get; set; }
        public decimal KilogramPerHectareToPoundPerAcreConversion { get; set; }
        public decimal PotassiumAvailabilityFirstYear { get; set; }
        public decimal PotassiumAvailabilityLongTerm { get; set; }
        public decimal PotassiumKtoK2OConversion { get; set; }
        public decimal PhosphorousAvailabilityFirstYear { get; set; }
        public decimal PhosphorousAvailabilityLongTerm { get; set; }
        public decimal PhosphorousPtoP2O5Conversion { get; set; }
        public decimal PoundPerTonConversion { get; set; }
        public decimal PoundPer1000FtSquaredToPoundPerAcreConversion { get; set; }
        public string DefaultApplicationOfManureInPrevYears { get; set; }
        public decimal SoilTestPPMToPoundPerAcre { get; set; }
    }
}