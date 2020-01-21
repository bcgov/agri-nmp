using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Agri.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SERVERAPI.ViewModels
{
    public class CompostDetailViewModel
    {
        public string Title { get; set; }
        public int? Id { get; set; }
        public string Target { get; set; }

        //[Required]
        public int SelectedManureOption { get; set; }

        public List<SelectListItem> SourceOfMaterialOptions { get; set; }

        //[Required(ErrorMessage = "Required")]
        public string SelectedSourceOfMaterialOption { get; set; }

        public List<Agri.Models.Configuration.SelectListItem> ManureOptions { get; set; }
        public string Action { get; set; }
        public string SourceOfMaterialName { get; set; }

        [Display(Name = "Material Type")]
        public string ManureName { get; set; }

        public ManureMaterialType MaterialType { get; set; }

        [Display(Name = "Moisture (%)")]
        public string Moisture { get; set; }

        [Display(Name = "N (%)")]
        public string Nitrogen { get; set; }

        [Display(Name = "NH<sub>4</sub>-N (ppm)")]
        public string Ammonia { get; set; }

        [Display(Name = "P (%)")]
        public string Phosphorous { get; set; }

        [Display(Name = "K (%)")]
        public string Potassium { get; set; }

        [Display(Name = "NO<sub>3</sub>-N (ppm)")]
        public string Nitrate { get; set; }

        public bool BookValue { get; set; }
        public bool OnlyCustom { get; set; }
        public bool Compost { get; set; }
        public string ButtonPressed { get; set; }
        public string Url { get; set; }
        public string UrlText { get; set; }
        public string MoistureBook { get; set; }
        public string NitrogenBook { get; set; }
        public string AmmoniaBook { get; set; }
        public string NitrateBook { get; set; }
        public string PhosphorousBook { get; set; }
        public string PotassiumBook { get; set; }
        public bool ShowNitrate { get; set; }
        public NutrientAnalysisTypes StoredImported { get; set; }
        public bool IsAssignedToStorage { get; set; }
        public string ExplainNutrientAnalysisMoisture { get; set; }
        public string ExplainNutrientAnalysisNitrogen { get; set; }
        public string ExplainNutrientAnlalysisAmmonia { get; set; }
        public string ExplainNutrientAnlalysisPhosphorous { get; set; }
        public string ExplainNutrientAnlalysisPotassium { get; set; }
        public bool IsLegacyNMPReleaseVersion { get; set; }
        public int? LegacyNMPReleaseVersionManureId { get; set; }
    }
}