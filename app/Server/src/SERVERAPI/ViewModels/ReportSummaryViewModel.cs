using System.Collections.Generic;

namespace SERVERAPI.ViewModels
{
    public class ReportSummaryViewModel
    {
        public string testMethod { get; set; }
        public string year { get; set; }
        public List<ReportSummaryTest> tests { get; set; }
    }
}