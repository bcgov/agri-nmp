namespace SERVERAPI.ViewModels
{
    public class SoilTestDetailsViewModel
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string btnText { get; set; }
        public string fieldName { get; set; }
        public string buttonPressed { get; set; }
        public string sampleDate { get; set; }
        public string dispNO3H { get; set; }
        public string dispP { get; set; }
        public string dispK { get; set; }
        public string dispPH { get; set; }
        public string url { get; set; }
        public string urlText { get; set; }
        public string SoilTestValuesMsg { get; set; }
        public string SoilTestNitrogenNitrateMsg { get; set; }
        public string SoilTestPhosphorousMsg { get; set; }
        public string SoilTestPotassiumMsg { get; set; }
        public string SoilTestPHMsg { get; set; }
        public bool ShowBlueberries { get; set; }

        public string leafTissueP { get; set; }
        public string leafTissueK { get; set; }
        public string cropRequirementN { get; set; }
        public string cropRequirementP2O5 { get; set; }
        public string cropRequirementK2O5 { get; set; }
        public string cropRemovalP2O5 { get; set; }
        public string cropRemovalK2O5 { get; set; }
    }
}