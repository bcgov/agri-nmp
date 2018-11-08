﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SERVERAPI.ViewModels
{
    public class CompostDetailViewModel
    {
        public string title { get; set; }
        public int? id { get; set; }
        public string target { get; set; }
        public int selManOption { get; set; }
        public List<Models.StaticData.SelectListItem> manOptions { get; set; }
        public string act { get; set; }
        [Display(Name = "Material Type")]
        public string manureName { get; set; }
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
    }
}