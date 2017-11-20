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
        public List<Models.StaticData.SelectListItem> regOptions { get; set; }
        [Display(Name = "Region")]
        public int? selRegOption { get; set; }
        public string currYear { get; set; }

    }
    public class FieldPageViewModel
    {
    }
    public class IndexViewModel
    {
        public bool unsavedData { get; set; }
        public string userData { get; set; }
        public string welcomeMsg { get; set; }
        public string disclaimerMsg { get; set; }
        public string staticDataVersionMsg { get; set; }
    }
    public class LaunchViewModel
    {
        public bool unsavedData { get; set; }
        public string userData { get; set; }
    }
    public class NewWarningViewModel
    {
        public string msg { get; set; }
    }
    public class FinishWarningViewModel
    {
        public string msg { get; set; }
    }
    public class FieldDetailViewModel
    {
        [Display(Name = "Field Name")]
        [Required]
        public string fieldName { get; set; }
        [Display(Name = "Area")]
        [Required]
        public string fieldArea { get; set; }
        [Display(Name = "Comments (optional)")]
        public string fieldComment { get; set; }
        public string act { get; set; }
        public string userDataField { get; set; }
        public string currFieldName { get; set; }
        public string target { get; set; }
        public string cntl { get; set; }
        public string actn { get; set; }
        public int fieldId { get; set; }
        public string placehldr { get; set; }
    }
    public class FieldDeleteViewModel
    {
        [Display(Name = "Field Name")]
        public string fieldName { get; set; }
        public string act { get; set; }
        public string userDataField { get; set; }
        public string target { get; set; }
    }
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
    }
    public class FileLoadViewModel
    {
        [Display(Name = "File Name")]
        public string fileName { get; set; }
        public bool unsavedData { get; set; }
        public bool badFile { get; set; }
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
        public string url { get; set; }
    }
    public class ManureDeleteViewModel
    {
        public string act { get; set; }
        public string fldName { get; set; }
        public int id { get; set; }
        [Display(Name = "Material Type")]
        public string matType { get; set; }
    }
    public class FertilizerDeleteViewModel
    {
        public string act { get; set; }
        public string fldName { get; set; }
        public int id { get; set; }
        [Display(Name = "Fertilizer")]
        public string fertilizerName { get; set; }
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
        public bool stdCrude { get; set; }
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
        public string nCredit { get; set; }
        public string nCreditLabel { get; set; }
        public bool coverCrop { get; set; }
        public bool? coverCropHarvested { get; set; }
        public bool modNitrogen { get; set; }
        public bool stdN { get; set; }
        public string stdNAmt { get; set; }
    }
    public class CropDeleteViewModel
    {
        public string act { get; set; }
        public string fldName { get; set; }
        public int id { get; set; }
        [Display(Name = "Crop")]
        public string cropName { get; set; }
    }
    public class CompostDeleteViewModel
    {
        public string act { get; set; }
        public int id { get; set; }
        public string target { get; set; }
        [Display(Name = "Compost/Manure")]
        public string manureName { get; set; }
        public string warning { get; set; }
    }
    public class FertilizerDetailsViewModel
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string btnText { get; set; }
        public string fieldName { get; set; }
        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public string selTypOption { get; set; }
        public List<Models.StaticData.SelectListItem> typOptions { get; set; }
        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public int selFertOption { get; set; }
        public List<Models.StaticData.SelectListItem> fertOptions { get; set; }
        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public string selRateOption { get; set; }
        public string selRateOptionText { get; set; }
        public List<Models.StaticData.SelectListItem> rateOptions { get; set; }
        public int selDenOption { get; set; }
        public List<Models.StaticData.SelectListItem> denOptions { get; set; }
        public int selMethOption { get; set; }
        public List<Models.StaticData.SelectListItem> methOptions { get; set; }
        [Required(ErrorMessage = "Required")]
        public string applRate { get; set; }
        public string currUnit { get; set; }
        public string applDate { get; set; }
        public string buttonPressed { get; set; }
        public string valN { get; set; }
        public string valP2o5 { get; set; }
        public string valK2o { get; set; }
        public string calcN { get; set; }
        public string calcP2o5 { get; set; }
        public string calcK2o { get; set; }
        public bool manEntry { get; set; }
        public string fertilizerType { get; set; }
        public string density { get; set; }
        public bool stdDensity { get; set; }
    }
    public class CalculateViewModel
    {
        public bool fldsFnd { get; set; }
        public string currFld { get; set; }
        public bool itemsPresent { get; set; }
        public string noData { get; set; }
        public List<Field> fields { get; set; }
    }
    public class SoilTestViewModel
    {
        public bool fldsFnd { get; set; }
        public string buttonPressed { get; set; }
        public string selTstOption { get; set; }
        public List<Models.StaticData.SelectListItem> tstOptions { get; set; }
        public bool testSelected { get; set; }
        public string url { get; set; }
        public string warningMsg { get; set; }
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
        public string url { get; set; }
    }
    public class SoilTestDeleteViewModel
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string btnText { get; set; }
        [Display(Name = "Field Name")]
        public string fieldName { get; set; }
    }
    public class MissingTestsViewModel
    {
        public string target { get; set; }
        public string msg { get; set; }
    }
    public class SaveWarningViewModel
    {
        public string target { get; set; }
        public string msg { get; set; }
    }
    public class ReportViewModel
    {
        public bool fields { get; set; }
        public bool nutrientSources { get; set; }
        public bool nutrientApplicationSchedule { get; set; }
        public bool nutrientSourceAnalysis { get; set; }
        public bool soilTestSummary { get; set; }
        public bool recordKeepingSheets { get; set; }
        public bool unsavedData { get; set; }
        public string url { get; set; }
    }
    public class ReportSourcesViewModel
    {
        public string year { get; set; }
        public List<ReportSourcesDetail> details { get; set; }
    }
    public class ReportSourcesDetail
    {
        public string nutrientName { get; set; }
        public string nutrientAmount { get; set; }
        public string nutrientUnit { get; set; }
    }
    public class ReportAnalysisViewModel
    {
        public bool nitratePresent { get; set; }
        public List<ReportAnalysisDetail> details { get; set; }
    }
    public class ReportAnalysisDetail
    {
        public string manureName { get; set; }
        public string sampleDate { get; set; }
        public string moisture { get; set; }
        public string ammonia { get; set; }
        public string nitrogen { get; set; }
        public string phosphorous { get; set; }
        public string potassium { get; set; }
        public string nitrate { get; set; }
    }
    public class ReportSummaryViewModel
    {
        public string testMethod { get; set; }
        public string year { get; set; }
        public List<ReportSummaryTest> tests { get; set; }
    }
    public class ReportSummaryTest
    {
        public string fieldName { get; set; }
        public string sampleDate { get; set; }
        public string fieldCrops { get; set; }
        public string pH { get; set; }
        public string nitrogen { get; set; }
        public string phosphorous { get; set; }
        public string potassium { get; set; }
        public string phosphorousRange { get; set; }
        public string potassiumRange { get; set; }
    }
    public class ReportApplicationViewModel
    {
        public string year { get; set; }
        public List<ReportApplicationField> fields { get; set; }
    }
    public class ReportApplicationField
    {
        public string fieldName { get; set; }
        public string fieldArea { get; set; }
        public string fieldComment { get; set; }
        public string fieldCrops { get; set; }
        public List<ReportFieldNutrient> nutrients { get; set; }
    }
    public class ReportFieldNutrient
    {
        public string nutrientName { get; set; }
        public string nutrientAmount { get; set; }
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
        public string methodName { get; set; }
        public string prevHdg { get; set; }
        public List<ReportFieldsField> fields { get; set; }
    }
    public class ReportSheetsViewModel
    {
        public string year { get; set; }
        public List<ReportSheetsField> fields { get; set; }
    }
    public class ReportFontsViewModel
    {
        public string year { get; set; }
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
        public string iconAgriN { get; set; }
        public string iconAgriP { get; set; }
        public string iconAgriK { get; set; }
        public string iconCropN { get; set; }
        public string iconCropP { get; set; }
        public string iconCropK { get; set; }
        public List<ReportFieldFootnote> footnotes { get; set; }
    }
    public class ReportSheetsField
    {
        public string fieldName { get; set; }
        public string fieldCrops { get; set; }
        public string fieldArea { get; set; }
        public List<ReportFieldNutrient> nutrients { get; set; }
        public List<ReportFieldOtherNutrient> otherNutrients { get; set; }
    }
    public class ReportFieldSoilTest
    {
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
        public string yrN { get; set; }
        [Display(Name = "P2O5")]
        [Required(ErrorMessage = "Required")]
        public string yrP { get; set; }
        [Display(Name = "K2O")]
        [Required(ErrorMessage = "Required")]
        public string yrK { get; set; }
        [Display(Name = "N")]
        [Required(ErrorMessage = "Required")]
        public string ltN { get; set; }
        [Display(Name = "P2O5")]
        [Required(ErrorMessage = "Required")]
        public string ltP { get; set; }
        [Display(Name = "K2O")]
        [Required(ErrorMessage = "Required")]
        public string ltK { get; set; }
        public string url { get; set; }
    }
    public class InfoAgriViewModel
    {
        public string title { get; set; }
        public string text { get; set; }
    }

    public class ValidateStaticDataViewModel
    {
        public string staticDataErrors { get; set; }
    }
}
