using System;
using Agri.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Agri.Models.Configuration;

namespace SERVERAPI.ViewModels
{
	public class FertigationDetailsViewModel
	{
        public int? id { get; set; } //ID for an existing fertigation configuration
        public string title { get; set; }
        public string btnText { get; set; }
        public string fieldName { get; set; }
        public string cropName { get; set; }
        public string buttonPressed { get; set; }
        public string fieldArea { get; set; }

        //fetilizer type
        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public string selTypOption { get; set; } // the selected fertilizer type
        [Required(ErrorMessage = "Required")]
        public int? selFertOption { get; set; } // the selected 
        public string fertilizerType { get; set; }
        public string currUnit { get; set; }
        public List<SelectListItem> typOptions { get; set; }
        public List<SelectListItem> fertilizers { get; set; }

        //fertilizer
        public List<SelectListItem> FertigationList { get; set; }

        public List<SelectListItem> fertOptions { get; set; }

        //product rate
        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public string selProductRateUnitOption { get; set; }
        public string selProductRateUnitOptionText { get; set; }
        public List<SelectListItem> productRateUnitOptions { get; set; }
        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public string productRate { get; set; }

        //density
        //density unit
        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public int? selDensityUnitOption { get; set; }
        public List<SelectListItem> densityUnitOptions { get; set; }
        [Required(ErrorMessage = "Required")]
        [Range(1, 9999.99, ErrorMessage = "Required")]
        public string density { get; set; }
        public bool stdDensity { get; set; }

        //injection rate
        //injection unit
        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public string injectionRate { get; set; }
        [Required(ErrorMessage = "Required")]
        public string selInjectionRateUnitOption { get; set; }
        
        public string selInjectionRateUnitOptionText { get; set; }
        public List<SelectListItem> injectionRateUnitOptions { get; set; }

        //#of fertigations per season
        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public int eventsPerSeason { get; set; }

        //fertigation scheduling
        public string selFertSchedOption { get; set; }
        public int selApplPeriod { get; set; }
        public List<SelectListItem> applPeriod { get; set; }

        //start date
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:m/d/yyyy}", ApplyFormatInEditMode = true)]
        public string applDate { get; set; }
        public bool manualEntry { get; set; }
        //calculated
        //total product volume per fertigation
        //product volume per fertigation
        //product vol per growing season
        //fertigation time
        //applied nutrients per fertigation
          //n
          //p2Os
          //k2o
        //total applied nutrients
          //n
          //p2Os
          //k2o
        //still required this year
          //n
          //p2Os
          //k2o
        public string valN { get; set; }
        public string valP2o5 { get; set; }
        public string valK2o { get; set; }
        public string calcN { get; set; }
        public string calcP2o5 { get; set; }
        public string calcK2o { get; set; }
        public string calcTotalN { get; set; }
        public string calcTotalK2o { get; set; }
        public string calcTotalP2o5 { get; set; }

        public string totPIcon { get; set; }
        public string totKIcon { get; set; }
        public string totNIcon { get; set; }
        public string totN { get; set; }
        public string totP2o5 { get; set; }
        public string totK2o { get; set; }
        public string totNIconText { get; set; }
        public string totPIconText { get; set; }
        public string totKIconText { get; set; }

    }
}