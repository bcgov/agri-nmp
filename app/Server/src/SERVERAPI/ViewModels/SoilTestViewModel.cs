using System.Collections.Generic;
using Agri.Models.Configuration;


namespace SERVERAPI.ViewModels
{
    public class SoilTestViewModel
    {
        public bool fldsFnd { get; set; }
        public string buttonPressed { get; set; }
        public string selTstOption { get; set; }
        public List<SelectListItem> tstOptions { get; set; }
        public bool testSelected { get; set; }
        public string warningMsg { get; set; }

        public bool showLeafTests { get; set; }
        public string selLeafTstOption { get; set; }
        public List<SelectListItem> leafTstOptions { get; set; }
        public bool leafTestSelected { get; set; }
        
    }
}