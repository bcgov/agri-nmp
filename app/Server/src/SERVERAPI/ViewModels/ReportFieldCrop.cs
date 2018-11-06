namespace SERVERAPI.ViewModels
{
    public class ReportFieldCrop
    {
        public string cropname { get; set; }
        public string previousCrop { get; set; }
        public decimal yield { get; set; }
        public string yieldInUnit { get; set; }
        public decimal reqN { get; set; }
        public decimal reqP { get; set; }
        public decimal reqK { get; set; }
        public decimal remN { get; set; }
        public decimal remP { get; set; }
        public decimal remK { get; set; }
        public string footnote { get; set; }
    }
}