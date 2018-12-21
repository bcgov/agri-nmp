using System.Collections.Generic;

namespace SERVERAPI.ViewModels
{
    public class ReportManureCompostViewModel
    {
        public string year { get; set; }
        public List<ReportStorage> storages { get; set; }

        public List<ReportManuress> unstoredManures { get; set; }
    }
}