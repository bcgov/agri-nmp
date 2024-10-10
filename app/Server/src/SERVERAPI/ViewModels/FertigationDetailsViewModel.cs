using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Agri.Models;
using Agri.Models.Configuration;
using SERVERAPI.Attributes;
namespace SERVERAPI.ViewModels
{
    public class FertigationDetailsViewModel
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string btnText { get; set; }
        public string fieldName { get; set; }
        public string cropName { get; set; }
        public string buttonPressed { get; set; }
        public string fieldArea { get; set; }

        // Fertilizer type
        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public string selTypOption { get; set; }
        [Required(ErrorMessage = "Required")]
        public int? selFertOption { get; set; }
        public string fertilizerType { get; set; }
        public string currUnit { get; set; }
        public List<SelectListItem> typOptions { get; set; }
        public List<SelectListItem> fertilizers { get; set; }

        // Fertilizer
        public List<SelectListItem> FertigationList { get; set; }
        public List<SelectListItem> fertOptions { get; set; }

        // Product rate
        //[Required(ErrorMessage = "Required")]
        //[Range(1, 9999, ErrorMessage = "Required")]
        public string selProductRateUnitOption { get; set; }
        public string selProductRateUnitOptionText { get; set; }
        public List<SelectListItem> productRateUnitOptions { get; set; }
        [RequiredIf("selTypOption", "3", "4", ErrorMessage = "Required")]
        //[Range(1, 9999.99, ErrorMessage = "Required")]
        public string productRate { get; set; }

        // Density
        //[Required(ErrorMessage = "Required")]
        //[Range(1, 9999, ErrorMessage = "Required")]
        public int? selDensityUnitOption { get; set; }
        public List<SelectListItem> densityUnitOptions { get; set; }
        //[Required(ErrorMessage = "Required")]
        //[Range(1, 9999.99, ErrorMessage = "Required")]
        [RequiredIf("selTypOption", "3", "4", ErrorMessage = "Required")]
        public string density { get; set; }
        public bool stdDensity { get; set; }

        // Injection rate
        [Range(0, 9999.99, ErrorMessage = "Number must be < 10,000")]
        public string injectionRate { get; set; }
        public string selInjectionRateUnitOption { get; set; }
        public string selInjectionRateUnitOptionText { get; set; }
        public List<SelectListItem> injectionRateUnitOptions { get; set; }

        // Number of fertigations per season
        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public int eventsPerSeason { get; set; }

        // Fertigation scheduling
        public string selFertSchedOption { get; set; } = "1";
        public int selApplPeriod { get; set; } = 1;
        public List<SelectListItem> applPeriod { get; set; }

        // Start date
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? applDate { get; set; }
        public bool manualEntry { get; set; }

        // Nutrient values
        //[Required(ErrorMessage = "Required")]
        //[Range(0, 9999, ErrorMessage = "Required")]
        public string valN { get; set; }
        //[Required(ErrorMessage = "Required")]
        //[Range(0, 9999, ErrorMessage = "Required")]
        public string valP2o5 { get; set; }
        //[Required(ErrorMessage = "Required")]
        //[Range(0, 9999, ErrorMessage = "Required")]
        public string valK2o { get; set; }

        // Calculated values
        public string calcN { get; set; }
        public string calcP2o5 { get; set; }
        public string calcK2o { get; set; }
        public string calcTotalN { get; set; }
        public string calcTotalK2o { get; set; }
        public string calcTotalP2o5 { get; set; }

        // Total nutrient icons and values
        public string totPIcon { get; set; }
        public string totKIcon { get; set; }
        public string totNIcon { get; set; }
        public string totN { get; set; }
        public string totP2o5 { get; set; }
        public string totK2o { get; set; }
        public string totNIconText { get; set; }
        public string totPIconText { get; set; }
        public string totKIconText { get; set; }

        // Fertigation calculations
        public decimal fertigationTime { get; set; }
        public decimal totProductVolPerFert { get; set; }
        public decimal totProductVolPerSeason { get; set; }

        // Dry fertigation additional fields
        // [Required(ErrorMessage = "Required")]
        // [Range(0, 9999, ErrorMessage = "Required")]
        [RequiredIf("selTypOption", "1", "2", ErrorMessage = "Required")]
        public string tankVolume { get; set; }
        public string tankVolumeUnits { get; set; }
        public List<SelectListItem> tankVolumeUnitOptions { get; set; }
        public int? selTankVolumeUnitOption { get; set; }
        // [Required(ErrorMessage = "Required")]
        // [Range(0, 9999, ErrorMessage = "Required")]
        [RequiredIf("selTypOption", "1", "2", ErrorMessage = "Required")]
        public string solInWater { get; set; }
        public string solInWaterUnits { get; set; }
        public List<SelectListItem> solubilityUnitOptions { get; set; }
        public int? selSolubilityUnitOption { get; set; }

        // [Required(ErrorMessage = "Required")]
        // [Range(0, 9999, ErrorMessage = "Required")]
        [RequiredIf("selTypOption", "1", "2", ErrorMessage = "Required")]
        public string amountToDissolve { get; set; }
        public string amountToDissolveUnits { get; set; }
        public List<SelectListItem> amountToDissolveUnitOptions { get; set; }
        public int? selDissolveUnitOption { get; set; }
        public string dryAction { get; set; }
        public string nutrientConcentrationN { get; set; }
        public string nutrientConcentrationP205 { get; set; }
        public string nutrientConcentrationK2O { get; set; }

        // Miscellaneous
        public bool isFertigation { get; set; }
        public string groupID { get; set; }
    }
}