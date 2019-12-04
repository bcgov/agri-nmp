using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Agri.Models.Configuration;

namespace SERVERAPI.ViewModels
{
    public class FarmViewModel : FarmViewModelBase
    {
        [Display(Name = "Year")]
        [Required]
        public string year { get; set; }
        [Display(Name = "Farm Name")]
        public string farmName { get; set; }

        public string currYear { get; set; }
        public bool HasAnimals { get; set; }
        public bool ImportsManureCompost { get; set; }
        public bool UsesFertilizer { get; set; }
        public bool IsLegacyNMPReleaseVersion { get; set; }
        public string LegacyNMPMessage { get; set; }
        public bool HasAnimals1 { get; set; }
        public bool HasDairyCows { get; set; }
        public bool HasBeefCows { get; set; }
        public bool HasPoultry { get; set; }
        public bool HasMixedLiveStock { get; set; }
        // public bool showAnimals => HasAnimals1;
        public bool showAnimals { get; set; }
    }
}