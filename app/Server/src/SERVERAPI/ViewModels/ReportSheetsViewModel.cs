using System.Collections.Generic;

namespace SERVERAPI.ViewModels
{
    public class ReportSheetsViewModel
    {
        public string year { get; set; }
        public List<ReportSheetsField> fields { get; set; }
    }
}