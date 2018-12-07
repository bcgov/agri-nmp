using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Agri.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SERVERAPI.ViewModels
{
    public class CompostDetailViewModel
    {
        public string title { get; set; }
        public int? id { get; set; }
        public string target { get; set; }
        //[Required]
        public int selManOption { get; set; }
        public List<SelectListItem> sourceOfMaterialOptions { get; set; }
        //[Required(ErrorMessage = "Required")]
        public string selsourceOfMaterialOption { get; set; }
        public List<Agri.Models.Configuration.SelectListItem> manOptions { get; set; }
        public string act { get; set; }
        public string sourceOfMaterialName { get; set; }
        [Display(Name = "Material Type")]
        public string manureName { get; set; }
        public ManureMaterialType materialType { get; set; }
        [Display(Name = "Moisture (%)")]
        public string moisture { get; set; }
        [Display(Name = "N (%)")]
        public string nitrogen { get; set; }
        [Display(Name = "NH<sub>4</sub>-N (ppm)")]
        public string ammonia { get; set; }
        [Display(Name = "P (%)")]
        public string phosphorous { get; set; }
        [Display(Name = "K (%)")]
        public string potassium { get; set; }
        [Display(Name = "NO<sub>3</sub>-N (ppm)")]
        public string nitrate { get; set; }
        public bool bookValue { get; set; }
        public bool onlyCustom { get; set; }
        public bool compost { get; set; }
        public string buttonPressed { get; set; }
        public string url { get; set; }
        public string urlText { get; set; }
        public string moistureBook { get; set; }
        public string nitrogenBook { get; set; }
        public string ammoniaBook { get; set; }
        public string nitrateBook { get; set; }
        public string phosphorousBook { get; set; }
        public string potassiumBook { get; set; }
        public bool showNitrate { get; set; }
        public NutrientAnalysisTypes stored_imported { get; set; }

    }
}