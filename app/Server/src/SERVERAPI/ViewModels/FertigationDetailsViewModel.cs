using System;
using Agri.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Agri.Models.Configuration;
using FertigationData.Json;

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
        public int selFertOption { get; set; } // the selected 
        public string fertilizerType { get; set; }
        public string currUnit { get; set; }
        public List<SelectListItem> typOptions { get; set; }
        public List<SelectListItem> fertilizers { get; set; }
        //fertilizer
        public List<SelectListItem> FertigationList { get; set; }
        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public List<SelectListItem> fertOptions { get; set; }
        //product rate
        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public string selProductRateUnitOption { get; set; }
        public string selProductRateUnitOptionText { get; set; }
        public List<SelectListItem> productRateUnitOptions { get; set; }
        public string productRate { get; set; }
        //density
        //density unit
        public int selDensityUnitOption { get; set; }
        public List<SelectListItem> densityUnitOptions { get; set; }
        public string density { get; set; }
        public bool stdDensity { get; set; }
        //injection rate
        //injection unit
        public string injectionRate { get; set; }
        public string selInjectionRateUnitOption { get; set; }
        public string selInjectionRateUnitOptionText { get; set; }
        public List<SelectListItem> injectionRateUnitOptions { get; set; }
        //#of fertigations per season
        public int eventsPerSeason { get; set; }
        //fertigation scheduling
        public List<SelectListItem> applPeriod { get; set; }
        //start date
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

namespace FertigationData.Json
{
    public class FertigationData
    {
      public int Id { get; set; }
      public string Name { get; set; }
      public string DryLiquid { get; set; }
      public string Nitrogen { get; set; }
      public string Phosphorous { get; set; }
      public string Potassium { get; set; }
      public int SortNum { get; set; }
      public string LiquidFertilizerDensities { get; set; }
      public int StaticDataVersionId { get; set; }
    }

    public class FertigationList
    {
      public List<Fertilizer> Fertilizers { get; set; }
    }
}
