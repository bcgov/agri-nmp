using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Agri.Models.Configuration;

namespace SERVERAPI.ViewModels
{
    public class FarmViewModel
    {
        [Display(Name = "Year")]
        [Required]
        public string year { get; set; }
        [Display(Name = "Farm Name")]
        public string farmName { get; set; }
        public List<SelectListItem> regOptions { get; set; }
        [Display(Name = "Region")]
        public int? selRegOption { get; set; }
        public List<SelectListItem> subRegionOptions { get; set; }
        public int? selSubRegOption { get; set; }
        public string currYear { get; set; }
        public bool showSubRegion { get; set; }
        public bool multipleSubRegion { get; set; }
        public string buttonPressed { get; set; }
        public bool HasAnimals { get; set; }
        public bool ImportsManureCompost { get; set; }
        public bool UsesFertilizer { get; set; }
        public bool IsRelease1Data { get; set; }
        public string LegacyNMPMessage { get; set; }
    }
}