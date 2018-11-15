using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SERVERAPI.Models;

namespace SERVERAPI.ViewModels
{
    public class ManureGeneratedObtainedDetailViewModel
    {
        public string title { get; set; }
        public string btnText { get; set; }
        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public string selAnimalTypeOption { get; set; }
        public List<Models.StaticData.SelectListItem> animalTypeOptions { get; set; }
        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public string selSubTypeOption { get; set; }
        public string subTypeName { get; set; }
        public List<Models.StaticData.SelectListItem> subTypeOptions { get; set; }
        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public StaticData.ManureMaterialType selManureMaterialTypeOption { get; set; }
        //public List<Models.StaticData.SelectListItem> manureMaterialTypeOptions { get; set; }
        [Required(ErrorMessage = "Required")]
        public string averageAnimalNumber { get; set; }
        public string includeWashWater { get; set; }
        public string washWater { get; set; }
        public bool showWashWater { get; set; }
        public bool stdWashWater { get; set; }
        public string includeMilkProduction { get; set; }
        public string milkProduction { get; set; }
        public bool showMilkProduction { get; set; }
        public bool stdMilkProduction { get; set; }
        public string buttonPressed { get; set; }
        public string placehldr { get; set; }
        public string target { get; set; }
        public int? id { get; set; }
    }
}