using System.Collections.Generic;
using Agri.Models;
namespace SERVERAPI.ViewModels
{
    public class ReportStorage
    {
        public string storageSystemName { get; set; }
        public ManureMaterialType ManureMaterialType { get; set; }
        public string units { get; set; }
        public string precipitation { get; set; }
        public string milkingCenterWashWater { get; set; }
        public List<ReportFieldFootnote> footnotes { get; set; }
        public string footnote { get; set; }
        public string annualAmountOfManurePerStorage { get; set; }
        public List<ReportManuress> reportManures { get; set; }

    }
}