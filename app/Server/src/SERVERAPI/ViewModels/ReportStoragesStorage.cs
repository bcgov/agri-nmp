using System.Collections.Generic;
using Agri.Models.Calculate;
using Agri.Models.Farm;
using SERVERAPI.Utility;

namespace SERVERAPI.ViewModels
{
    public class ReportStoragesStorage
    {
        public string storageSystemName { get; set; }
        public string animalManure { get; set; }
        public decimal annualAmount { get; set; }
        public string units { get; set; }
        public decimal precipitation { get; set; }
        public decimal milkingCenterWashWater { get; set; }
        public List<GeneratedManure> manures { get; set; }
        public List<ReportFieldFootnote> footnotes { get; set; }

        public string footnote { get; set; }

    }
}