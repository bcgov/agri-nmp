namespace Agri.Models.StaticData
{
    public class SoilTestMethod
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal ConvertToKelownaPHLessThan72 { get; set; }
        public decimal ConvertToKelownaPHGreaterThanEqual72 { get; set; }
        public decimal ConvertToKelownaK { get; set; }
        public int SortNum { get; set; }
    }
}