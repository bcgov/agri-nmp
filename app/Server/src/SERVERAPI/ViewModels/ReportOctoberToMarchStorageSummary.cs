using System.Collections.Generic;

namespace SERVERAPI.ViewModels
{
    public class ReportOctoberToMarchStorageSummaryViewModel
    {
        public string year { get; set; }
        public List<ReportStorages> storages { get; set; }
        public List<ReportFieldFootnote> footnotes { get; set; }
    }
}