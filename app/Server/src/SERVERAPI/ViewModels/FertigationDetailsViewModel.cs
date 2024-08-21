using System;
using Agri.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Agri.Models.Configuration;

namespace SERVERAPI.ViewModels
{
	public class FertigationDetailsViewModel
	{
        public int? id { get; set; }
        public string title { get; set; }
        public string btnText { get; set; }
        public string fieldName { get; set; }
        public string cropName { get; set; }
        //fertilizer
        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public int selFertOption { get; set; }
        public List<SelectListItem> fertOptions { get; set; }
        //product rate
        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public string selProductRateOption { get; set; }
        public string selProductRateOptionText { get; set; }
        public List<SelectListItem> productRateOptions { get; set; }
        //density
        //density unit
        public int selDenOption { get; set; }
        public List<SelectListItem> denOptions { get; set; }
        public string density { get; set; }
        public bool stdDensity { get; set; }
        //injection rate
        //injection unit
        public string selInjectionRateOption { get; set; }
        public string selInjectionRateOptionText { get; set; }
        public List<SelectListItem> injectionRateOptions { get; set; }
        //#of fertigation per season
        public int eventsPerSeason { get; set; }
        //fertigation scheduling

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

    }
}
