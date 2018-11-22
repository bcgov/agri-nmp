using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Agri.Models.Configuration;

namespace SERVERAPI.ViewModels
{
    public class FertilizerDetailsViewModel
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string btnText { get; set; }
        public string fieldName { get; set; }
        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public string selTypOption { get; set; }
        public List<SelectListItem> typOptions { get; set; }
        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public int selFertOption { get; set; }
        public List<SelectListItem> fertOptions { get; set; }
        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public string selRateOption { get; set; }
        public string selRateOptionText { get; set; }
        public List<SelectListItem> rateOptions { get; set; }
        public int selDenOption { get; set; }
        public List<SelectListItem> denOptions { get; set; }
        public int selMethOption { get; set; }
        public List<SelectListItem> methOptions { get; set; }
        [Required(ErrorMessage = "Required")]
        public string applRate { get; set; }
        public string currUnit { get; set; }
        public string applDate { get; set; }
        public string buttonPressed { get; set; }
        public string valN { get; set; }
        public string valP2o5 { get; set; }
        public string valK2o { get; set; }
        public string calcN { get; set; }
        public string calcP2o5 { get; set; }
        public string calcK2o { get; set; }
        public bool manEntry { get; set; }
        public string fertilizerType { get; set; }
        public string density { get; set; }
        public bool stdDensity { get; set; }
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