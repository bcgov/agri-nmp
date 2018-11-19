using System.Collections.Generic;

namespace SERVERAPI.ViewModels
{
    public class SoilTestViewModel
    {
        public bool fldsFnd { get; set; }
        public string buttonPressed { get; set; }
        public string selTstOption { get; set; }
        public List<Models.StaticData.SelectListItem> tstOptions { get; set; }
        public bool testSelected { get; set; }
        public string warningMsg { get; set; }
    }
}