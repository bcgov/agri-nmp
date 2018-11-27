namespace SERVERAPI.ViewModels
{
    public class ReportStorageManure
    {
        public string subTypeName { get; set; }
        public string materialType { get; set; }
        public string averageAnimalNumber { get; set; }
        public string includeWashWater { get; set; }
        public string washWater { get; set; }
        public bool showWashWater { get; set; }
        public bool stdWashWater { get; set; }
        public string includeMilkProduction { get; set; }
        public string milkProduction { get; set; }
        public bool showMilkProduction { get; set; }
        public bool stdMilkProduction { get; set; }
    }
}