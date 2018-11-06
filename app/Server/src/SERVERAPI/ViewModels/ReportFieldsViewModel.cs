using System.Collections.Generic;

namespace SERVERAPI.ViewModels
{
    public class ReportFieldsViewModel
    {
        public string year { get; set; }
        public string methodName { get; set; }
        public string prevHdg { get; set; }
        public List<ReportFieldsField> fields { get; set; }
    }
}