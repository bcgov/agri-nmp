using System.Collections.Generic;
using Agri.Models.Calculate;
using SERVERAPI.Utility;

namespace SERVERAPI.ViewModels
{
    public class ReportFieldsField
    {
        public string fieldName { get; set; }
        public string fieldComment { get; set; }
        public string fieldCrops { get; set; }
        public string fieldArea { get; set; }
        public ReportFieldSoilTest soiltest { get; set; }
        public List<ReportFieldNutrient> nutrients { get; set; }
        public List<ReportFieldCrop> crops { get; set; }
        public List<ReportFieldOtherNutrient> otherNutrients { get; set; }
        public decimal reqN { get; set; }
        public decimal reqP { get; set; }
        public decimal reqK { get; set; }
        public decimal remN { get; set; }
        public decimal remP { get; set; }
        public decimal remK { get; set; }
        public List<BalanceMessages> alertMsgs { get; set; }
        public bool alertN { get; set; }
        public bool alertP { get; set; }
        public bool alertK { get; set; }
        public string iconAgriN { get; set; }
        public string iconAgriP { get; set; }
        public string iconAgriK { get; set; }
        public string iconCropN { get; set; }
        public string iconCropP { get; set; }
        public string iconCropK { get; set; }
        public List<ReportFieldFootnote> footnotes { get; set; }

        public bool showNitrogenCredit { get; set; }
        public int? nitrogenCredit { get; set; }  // prev manure application

        public bool showSoilTestNitrogenCredit { get; set; }
        public decimal soilTestNitrogenCredit { get; set; }

    }
}