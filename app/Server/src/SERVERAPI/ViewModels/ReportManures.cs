using System.Collections.Generic;
using Agri.Models;
using Agri.Models.Calculate;
using Agri.Models.Farm;
using SERVERAPI.Utility;

namespace SERVERAPI.ViewModels
{
    public class ReportManures
    {
        public string MaterialName { get; set; }
        public string MaterialSource { get; set; }
        public string AnnualAmount { get; set; }
        public string LandApplied { get; set; }
        public string AmountRemaining { get; set; }
        public string footnote { get; set; }
    }
}