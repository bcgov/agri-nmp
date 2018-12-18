using System.Collections.Generic;

namespace SERVERAPI.ViewModels
{
    public class ReportManureSummaryViewModel
    {
        public string year { get; set; }
        public List<ReportManures> manures { get; set; }
    }
}