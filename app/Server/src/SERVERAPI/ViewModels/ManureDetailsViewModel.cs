using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Agri.Models.Configuration;

namespace SERVERAPI.ViewModels
{
    public class ManureDetailsViewModel
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string btnText { get; set; }
        public string fieldName { get; set; }
        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public string selManOption { get; set; }
        public List<SelectListItem> manOptions { get; set; }
        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public string selApplOption { get; set; }
        public List<SelectListItem> applOptions { get; set; }
        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public string selRateOption { get; set; }
        public string selRateOptionText { get; set; }
        public List<SelectListItem> rateOptions { get; set; }
        [Required(ErrorMessage = "Required")]
        public string rate { get; set; }
        public string currUnit { get; set; }
        [Required(ErrorMessage = "Required")]
        public string nh4 { get; set; }
        [Required(ErrorMessage = "Required")]
        public string avail { get; set; }
        public string buttonPressed { get; set; }
        public string yrN { get; set; }
        public string yrP2o5 { get; set; }
        public string yrK2o { get; set; }
        public string ltN { get; set; }
        public string ltP2o5 { get; set; }
        public string ltK2o { get; set; }
        public bool stdN { get; set; }
        public bool stdAvail { get; set; }
        public string url { get; set; }
        public string urlText { get; set; }
        public string totNIcon { get; set; }
        public string totPIcon { get; set; }
        public string totKIcon { get; set; }
        public string totNIconText { get; set; }
        public string totPIconText { get; set; }
        public string totKIconText { get; set; }
        public string totN { get; set; }
        public string totP2o5 { get; set; }
        public string totK2o { get; set; }
    }
}