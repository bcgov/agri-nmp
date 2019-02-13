using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class DefaultSoilTest : Versionable
    {
        [Key]
        public int Id { get; set; }
        public decimal Nitrogen { get; set; }
        public decimal Phosphorous { get; set; }
        public decimal Potassium { get; set; }
        public decimal pH { get; set; }
        public int ConvertedKelownaP { get; set; }
        public int ConvertedKelownaK { get; set; }
        public string DefaultSoilTestMethodId { get; set; }
    }
}