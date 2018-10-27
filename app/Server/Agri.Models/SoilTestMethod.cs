namespace Agri.Models
{
    public class SoilTestMethod
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal ConvertToKelownaPlt72 { get; set; }
        public decimal ConvertToKelownaPge72 { get; set; }
        public decimal ConvertToKelownaK { get; set; }
        public int SortNum { get; set; }
    }
}