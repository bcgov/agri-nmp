using System.Collections.Generic;

namespace SERVERAPI.ViewModels
{
    public class ReportApplicationField
    {
        public string fieldName { get; set; }
        public string fieldArea { get; set; }
        public string fieldComment { get; set; }
        public string fieldCrops { get; set; }
        public List<ReportFieldNutrient> nutrients { get; set; }
    }
}