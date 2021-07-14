using Agri.Models.Configuration;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SERVERAPI.ViewModels
{
    public class FarmViewModelBase
    {
        public List<SelectListItem> RegOptions { get; set; } = new List<SelectListItem>();

        [Display(Name = "Region")]
        public int? SelRegOption { get; set; }

        public List<SelectListItem> SubRegionOptions { get; set; } = new List<SelectListItem>();
        public int? SelSubRegOption { get; set; }
        public bool ShowSubRegion { get; set; }
        public string ButtonPressed { get; set; }
        public bool TypeChangeDetected { get; set; } = false;
        public bool TypeChangeConfirmed { get; set; } = false;
        public bool OriginalHasAnimals { get; set; }
        public bool OriginalHasBeefCows { get; set; }
        public bool OriginalHasDairyCows { get; set; }
        public bool OriginalHasPoultry { get; set; }
        public bool OriginalHasMixedLiveStock { get; set; }
        public bool ShowAnimals { get; set; }
    }
}