using System.ComponentModel.DataAnnotations;

namespace SERVERAPI.ViewModels
{
    public class OtherDetailsViewModel
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string btnText { get; set; }
        public string fieldName { get; set; }
        [Display(Name = "Nutrient Source")]
        [Required(ErrorMessage = "Required")]
        public string source { get; set; }
        [Display(Name = "N")]
        public string yrN { get; set; }
        [Display(Name = "P2O5")]
        public string yrP { get; set; }
        [Display(Name = "K2O")]
        public string yrK { get; set; }
        [Display(Name = "N")]
        public string ltN { get; set; }
        [Display(Name = "P2O5")]
        public string ltP { get; set; }
        [Display(Name = "K2O")]
        public string ltK { get; set; }
        public string url { get; set; }
        public string urlText { get; set; }
        public string placehldr { get; set; }
        public string ExplainCalculateOtherNutrientSource { get; set; }
        public string ExplainCalculateOtherNutrientAvailableThisYear { get; set; }
        public string ExplainCalculateOtherNutrientAvailbleLongTerm { get; set; }
    }
}