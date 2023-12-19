using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Agri.Models.Configuration;

namespace SERVERAPI.ViewModels
{
    public class LeafTestDetailsViewModel
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string btnText { get; set; }
        public string fieldName { get; set; }
        public string selectorAffected { get; set; }
        public string sampleDate { get; set; }
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^\d+\.?\d*$", ErrorMessage = "Positive numeric value required")]
        public string leafTissueP { get; set; }
        [RegularExpression(@"^\d+\.?\d*$", ErrorMessage = "Positive numeric value required")]
        [Required(ErrorMessage = "Required")]
        public string leafTissueK { get; set; }
        public List<LeafTestDetailsItem> LeafTestDetailsItems { get; set; }

        public string leafTestValuesMsg { get; set; }
        public string leafTestLeafTissuePMsg { get; set; }
        public string leafTestLeafTissueKMsg { get; set; }

    }

    public class LeafTestDetailsItem
    {
        public int Id { get; set; }
        public string cropName { get; set; }
        public string cropRequirementN { get; set; }
        public string cropRequirementP2O5 { get; set; }
        public string cropRequirementK2O { get; set; }
        public string cropRemovalP2O5 { get; set; }
        public string cropRemovalK2O5 { get; set; }
    }

}