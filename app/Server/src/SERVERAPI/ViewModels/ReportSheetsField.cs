using System.Collections.Generic;

namespace SERVERAPI.ViewModels
{
    public class ReportSheetsField
    {
        public string fieldName { get; set; }
        public string fieldCrops { get; set; }
        public string fieldArea { get; set; }
        public List<ReportFieldNutrient> nutrients { get; set; }
        public List<ReportFieldOtherNutrient> otherNutrients { get; set; }
    }
}