using System.Collections.Generic;

namespace SERVERAPI.ViewModels
{
    public class ReportManuresViewModel
    {
        public string year { get; set; }
        public List<ReportStoragesStorage> storages { get; set; }
    }
}