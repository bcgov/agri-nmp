namespace SERVERAPI.ViewModels
{
    public class ReportViewModel
    {
        public bool fields { get; set; }
        public bool nutrientSources { get; set; }
        public bool nutrientApplicationSchedule { get; set; }
        public bool nutrientSourceAnalysis { get; set; }
        public bool soilTestSummary { get; set; }
        public bool recordKeepingSheets { get; set; }
        public bool unsavedData { get; set; }
        public string url { get; set; }
        public string noCropsMsg { get; set; }
        public string downloadMsg { get; set; }
        public string loadMsg { get; set; }
    }
}