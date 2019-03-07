using System.Collections.Generic;
using Agri.Models;
using Agri.Models.Calculate;
using Agri.Models.Farm;
using SERVERAPI.Utility;

namespace SERVERAPI.ViewModels
{
    public class ReportManuress
    {
        public string animalManure { get; set; }
        public string annualAmount { get; set; }
        public string units { get; set; }
        public string milkingCenterWashWater { get; set; }
    }
}