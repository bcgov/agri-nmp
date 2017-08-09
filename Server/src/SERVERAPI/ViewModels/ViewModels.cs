using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SERVERAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace SERVERAPI.ViewModels
{
    public class ViewModels
    {
    }
    public class FarmViewModel
    {
        [Display(Name = "Year")]
        [Required]
        public string year { get; set; }
        [Display(Name = "Farm Name")]
        public string farmName { get; set; }
        [Display(Name = "Do you have soil tests for for fields?")]
        public bool? soilTests { get; set; }
        [Display(Name = "Do you use manure or compost?")]
        public bool? manure { get; set; }
        public bool sendNMP { get; set; }
        public string userData { get; set; }
        public List<Models.StaticData.SelectListItem> regOptions { get; set; }
        [Display(Name = "Region")]
        public int? selRegOption { get; set; }
        public string currYear { get; set; }

    }
    public class IndexViewModel
    {
        public string userData { get; set; }
    }
    public class LaunchViewModel
    {
        public bool canContinue { get; set; }
        public string userData { get; set; }
    }
    public class FieldDetailViewModel
    {
        [Display(Name = "Field Name")]
        [Required]
        public string fieldName { get; set; }
        [Display(Name = "Area")]
        [Required]
        public string fieldArea { get; set; }
        [Display(Name = "Comments")]
        public string fieldComment { get; set; }
        public string act { get; set; }
        public bool sendNMP { get; set; }
        public string userDataField { get; set; }
        public string currFieldName { get; set; }
    }
    public class FieldDeleteViewModel
    {
        [Display(Name = "Field Name")]
        public string fieldName { get; set; }
        public string act { get; set; }
        public string userDataField { get; set; }
    }
}
