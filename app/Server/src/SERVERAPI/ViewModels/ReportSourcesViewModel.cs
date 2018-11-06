using System.Collections.Generic;

namespace SERVERAPI.ViewModels
{
    public class ReportSourcesViewModel
    {
        public string year { get; set; }
        public List<ReportSourcesDetail> details { get; set; }
    }
}