namespace SERVERAPI.ViewModels
{
    public class LeafTestDetailsViewModel
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string btnText { get; set; }
        public string fieldName { get; set; }
        public string buttonPressed { get; set; }
        public string sampleDate { get; set; }
        
        public string leafTissueP { get; set; }
        public string leafTissueK { get; set; }
        public string cropRequirementN { get; set; }
        public string cropRequirementP2O5 { get; set; }
        public string cropRequirementK2O5 { get; set; }
        public string cropRemovalP2O5 { get; set; }
        public string cropRemovalK2O5 { get; set; }

        public string leafTestValuesMsg { get; set; }
        public string leafTestLeafTissuePMsg { get; set; }
        public string leafTestLeafTissueKMsg { get; set; }
        public string leafTestCropRequirementNMsg { get; set; }
        public string leafTestCropRequirementP2O5Msg { get; set; }
        public string leafTestCropRequirementK2O5Msg { get; set; }
        public string leafTestCropRemovalP2O5Msg { get; set; }
        public string leafTestCropRemovalK2O5Msg { get; set; }
    }
}