using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SERVERAPI.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public List<Models.StaticData.SelectListItem> regOptions { get; set; }
        [Display(Name = "Region")]
        public int? selRegOption { get; set; }
        public string currYear { get; set; }

    }
    public class FieldPageViewModel
    {
        public bool? soilTests { get; set; }
        public bool? manure { get; set; }
    }
    public class IndexViewModel
    {
        public string userData { get; set; }
    }
    public class LaunchViewModel
    {
        public bool unsavedData { get; set; }
        public string userData { get; set; }
    }
    public class NewWarningViewModel
    {
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
        public string userDataField { get; set; }
        public string currFieldName { get; set; }
        public string target { get; set; }
        public string cntl { get; set; }
        public string actn { get; set; }
    }
    public class FieldDeleteViewModel
    {
        [Display(Name = "Field Name")]
        public string fieldName { get; set; }
        public string act { get; set; }
        public string userDataField { get; set; }
        public string target { get; set; }
    }
    public class FileLoadViewModel
    {
        [Display(Name = "File Name")]
        public string fileName { get; set; }
        public bool unsavedData { get; set; }
    }
    public class ManureDetailsViewModel
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string btnText { get; set; }
        public string fieldName { get; set; }
        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public string selManOption { get; set; }
        public List<Models.StaticData.SelectListItem> manOptions { get; set; }
        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public string selApplOption { get; set; }
        public List<Models.StaticData.SelectListItem> applOptions { get; set; }
        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public string selRateOption { get; set; }
        public string selRateOptionText { get; set; }
        public List<Models.StaticData.SelectListItem> rateOptions { get; set; }
        [Required(ErrorMessage = "Required")]
        public string rate { get; set; }
        public string currUnit { get; set; }
        [Required(ErrorMessage = "Required")]
        public string nh4 { get; set; }
        [Required(ErrorMessage = "Required")]
        public string avail { get; set; }
        public string buttonPressed { get; set; }
        public string yrN { get; set; }
        public string yrP2o5 { get; set; }
        public string yrK2o { get; set; }
        public string ltN { get; set; }
        public string ltP2o5 { get; set; }
        public string ltK2o { get; set; }
        public bool stdN { get; set; }
        public bool stdAvail { get; set; }
    }
    public class ManureDeleteViewModel
    {
        public string act { get; set; }
        public string fldName { get; set; }
        public int id { get; set; }
        [Display(Name = "Material Type")]
        public string matType { get; set; }
    }
    public class CropDetailsViewModel
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string btnText { get; set; }
        public string fieldName { get; set; }
        public bool showCrude { get; set; }
        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public string selTypOption { get; set; }
        public List<Models.StaticData.SelectListItem> typOptions { get; set; }
        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public string selCropOption { get; set; }
        public List<Models.StaticData.SelectListItem> cropOptions { get; set; }
        [Required(ErrorMessage = "Required")]
        public string yield { get; set; }
        public string yieldUnit { get; set; }
        public string crude { get; set; }
        public string buttonPressed { get; set; }
        public string reqN { get; set; }
        public string reqP2o5 { get; set; }
        public string reqK2o { get; set; }
        public string remN { get; set; }
        public string remP2o5 { get; set; }
        public string remK2o { get; set; }

    }
    public class CropDeleteViewModel
    {
        public string act { get; set; }
        public string fldName { get; set; }
        public int id { get; set; }
        [Display(Name = "Crop")]
        public string cropName { get; set; }
    }
    public class CalculateViewModel
    {
        public bool fldsFnd { get; set; }
        public string currFld { get; set; }
        public List<Field> fields { get; set; }
    }
    public class SoilTestViewModel
    {
        public bool fldsFnd { get; set; }
        public string buttonPressed { get; set; }
        public string selMthOption { get; set; }
        public List<Models.StaticData.SelectListItem> mthOptions { get; set; }
    }
    public class SoilTestDetailsViewModel
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string btnText { get; set; }
        public string fieldName { get; set; }
        public string buttonPressed { get; set; }
        public string sampleDate { get; set; }
        public string dispNO3H { get; set; }
        public string dispP { get; set; }
        public string dispK { get; set; }
        public string dispPH { get; set; }

    }
    public class SoilTestDeleteViewModel
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string btnText { get; set; }
        [Display(Name = "Field Name")]
        public string fieldName { get; set; }
    }

}
