using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class SoilTestMethod : Versionable
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal ConvertToKelownaPHLessThan72 { get; set; }
        public decimal ConvertToKelownaPHGreaterThanEqual72 { get; set; }
        public decimal ConvertToKelownaK { get; set; }
        public int SortNum { get; set; }
    }
}