using System.Collections.Generic;

namespace SERVERAPI.ViewModels
{
    public class ReportAnalysisViewModel
    {
        public bool nitratePresent { get; set; }
        public List<ReportAnalysisDetail> details { get; set; }
    }
}