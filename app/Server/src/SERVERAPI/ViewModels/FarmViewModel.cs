using System.ComponentModel.DataAnnotations;

namespace SERVERAPI.ViewModels
{
    public class FarmViewModel : FarmViewModelBase
    {
        [Display(Name = "Year")]
        [Required]
        public string Year { get; set; }

        [Display(Name = "Farm Name")]
        public string FarmName { get; set; }

        public string CurrentYear { get; set; }

        public bool ImportsManureCompost { get; set; }

        public bool UsesFertilizer { get; set; }
        public bool IsLegacyNMPReleaseVersion { get; set; }
        public string LegacyNMPMessage { get; set; }
        public bool HasSelectedFarmType { get; set; }
        public bool HasAnimals { get; set; }
        public bool HasTestResults { get; set; }
        public bool HasFields { get;  set; }
        public bool HasDairyCows { get; set; }
        public bool HasBeefCows { get; set; }
        public bool HasPoultry { get; set; }
        public bool HasMixedLiveStock { get; set; }
    }
}