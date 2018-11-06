namespace SERVERAPI.ViewModels
{
    public class ReportFieldNutrient
    {
        public string nutrientName { get; set; }
        public string nutrientAmount { get; set; }
        public string nutrientUnit { get; set; }
        public string nutrientSeason { get; set; }
        public string nutrientApplication { get; set; }
        public decimal reqN { get; set; }
        public decimal reqP { get; set; }
        public decimal reqK { get; set; }
        public decimal remN { get; set; }
        public decimal remP { get; set; }
        public decimal remK { get; set; }
        public string footnote { get; set; }
    }
}