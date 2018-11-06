using System.Collections.Generic;

namespace SERVERAPI.ViewModels
{
    public class ReportApplicationViewModel
    {
        public string year { get; set; }
        public List<ReportApplicationField> fields { get; set; }
    }
}