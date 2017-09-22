using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SERVERAPI.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using SERVERAPI.Utility;

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
        public int fieldId { get; set; }
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
    public class OtherDeleteViewModel
    {
        public string act { get; set; }
        public string fldName { get; set; }
        public int id { get; set; }
        [Display(Name = "Nutrient Source")]
        public string source { get; set; }
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
        public string selPrevOption { get; set; }
        public List<Models.StaticData.SelectListItem> prevOptions { get; set; }
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
        public bool manEntry { get; set; }
        public string cropDesc { get; set; }
        public bool coverCrop { get; set; }
        public bool? coverCropHarvested { get; set; }
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
        public string selTstOption { get; set; }
        public List<Models.StaticData.SelectListItem> tstOptions { get; set; }
        public bool testSelected { get; set; }
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
    public class ReportViewModel
    {
        public bool fields { get; set; }
        public bool nutrientSources { get; set; }
        public bool nutrientApplicationSchedule { get; set; }
        public bool nutrientSourceAnalysis { get; set; }
        public bool soilTestSummary { get; set; }
        public bool recordKeepingSheets { get; set; }
    }
    public class ReportSourcesViewModel
    {
        public List<ReportSourcesDetail> details { get; set; }
    }
    public class ReportSourcesDetail
    {
        public string nutrientName { get; set; }
        public decimal nutrientAmount { get; set; }
        public string nutrientUnit { get; set; }
    }
    public class ReportApplicationViewModel
    {
        public List<ReportApplicationField> fields { get; set; }
    }
    public class ReportApplicationField
    {
        public string fieldName { get; set; }
        public string fieldComment { get; set; }
        public string fieldCrops { get; set; }
        public List<ReportFieldNutrient> nutrients { get; set; }
    }
    public class ReportFieldNutrient
    {
        public string nutrientName { get; set; }
        public decimal nutrientAmount { get; set; }
        public string nutrientUnit { get; set; }
        public string nutrientSeason { get; set; }
        public string nutrientApplication { get; set; }
        public decimal reqN { get; set; }
        public decimal reqP { get; set; }
        public decimal reqK { get; set; }
        public decimal remN { get; set; }
        public decimal remP { get; set; }
        public decimal remK { get; set; }
        public string footnote { get; set; }
    }
    public class ReportFieldsViewModel
    {
        public string year { get; set; }
        public List<ReportFieldsField> fields { get; set; }
    }
    public class ReportFieldsField
    {
        public string fieldName { get; set; }
        public string fieldComment { get; set; }
        public string fieldCrops { get; set; }
        public string fieldArea { get; set; }
        public ReportFieldSoilTest soiltest { get; set; }
        public List<ReportFieldNutrient> nutrients { get; set; }
        public List<ReportFieldCrop> crops { get; set; }
        public List<ReportFieldOtherNutrient> otherNutrients { get; set; }
        public decimal reqN { get; set; }
        public decimal reqP { get; set; }
        public decimal reqK { get; set; }
        public decimal remN { get; set; }
        public decimal remP { get; set; }
        public decimal remK { get; set; }
        public List<BalanceMessages> alertMsgs { get; set; }
        public bool alertN { get; set; }
        public bool alertP { get; set; }
        public bool alertK { get; set; }
        public List<ReportFieldFootnote> footnotes { get; set; }
    }
    public class ReportFieldSoilTest
    {
        public string methodName { get; set; }
        public string sampleDate { get; set; }
        public string dispNO3H { get; set; }
        public string dispP { get; set; }
        public string dispK { get; set; }
        public string dispPH { get; set; }
    }
    public class ReportFieldCrop
    {
        public string cropname { get; set; }
        public string previousCrop { get; set; }
        public decimal yield { get; set; }
        public decimal reqN { get; set; }
        public decimal reqP { get; set; }
        public decimal reqK { get; set; }
        public decimal remN { get; set; }
        public decimal remP { get; set; }
        public decimal remK { get; set; }
        public string footnote { get; set; }
    }
    public class ReportFieldOtherNutrient
    {
        public string otherName { get; set; }
        public decimal reqN { get; set; }
        public decimal reqP { get; set; }
        public decimal reqK { get; set; }
        public decimal remN { get; set; }
        public decimal remP { get; set; }
        public decimal remK { get; set; }
    }
    public class ReportFieldFootnote
    {
        public int id { get; set; }
        public string message { get; set; }
    }
    public class OtherDetailsViewModel
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string btnText { get; set; }
        public string fieldName { get; set; }
        [Display(Name = "Nutrient Source")]
        [Required(ErrorMessage = "Required")]
        public string source { get; set; }
        [Display(Name = "N")]
        [Required(ErrorMessage = "Required")]
        public string nitrogen { get; set; }
        [Display(Name = "P2O5")]
        [Required(ErrorMessage = "Required")]
        public string phospherous { get; set; }
        [Display(Name = "K2O")]
        [Required(ErrorMessage = "Required")]
        public string potassium { get; set; }
    }
}
