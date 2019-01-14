using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Agri.Models;
using Agri.Models.Configuration;
using MvcRendering = Microsoft.AspNetCore.Mvc.Rendering;

namespace SERVERAPI.ViewModels
{
    public class ManureGeneratedObtainedDetailViewModel
    {
        public string title { get; set; }
        public string btnText { get; set; }
        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public string selAnimalTypeOption { get; set; }
        public List<SelectListItem> animalTypeOptions { get; set; }
        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public string selSubTypeOption { get; set; }
        public string subTypeName { get; set; }
        public List<SelectListItem> subTypeOptions { get; set; }
        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public ManureMaterialType selManureMaterialTypeOption { get; set; }
        [Required(ErrorMessage = "Required")]
        public string averageAnimalNumber { get; set; }
        public string includeWashWater { get; set; }
        public string washWater { get; set; }
        public bool showWashWater { get; set; }
        public bool stdWashWater { get; set; }
        public bool stdManureMaterialType { get; set; }
        public bool hasLiquidManureType { get; set; }
        public bool hasSolidManureType { get; set; }
        public string includeMilkProduction { get; set; }
        public string milkProduction { get; set; }
        public bool showMilkProduction { get; set; }
        public bool stdMilkProduction { get; set; }
        public string buttonPressed { get; set; }
        public string placehldr { get; set; }
        public string target { get; set; }
        public int? id { get; set; }
        public string liquidPerGalPerAnimalPerDay { get; set; }
        public string solidPerPoundPerAnimalPerDay { get; set; }
        [Required(ErrorMessage = "Select a Type")]
        public WashWaterUnits SelWashWaterUnit { get; set; }
        public List<MvcRendering.SelectListItem> GetWashWaterUnits()
        {
            var selectListItems = new List<MvcRendering.SelectListItem>();

            selectListItems.Add(new MvcRendering.SelectListItem { Value = WashWaterUnits.USGallonsPerDayPerAnimal.ToString(), Text = EnumHelper<WashWaterUnits>.GetDisplayValue(WashWaterUnits.USGallonsPerDayPerAnimal) });
            selectListItems.Add(new MvcRendering.SelectListItem { Value = WashWaterUnits.USGallonsPerDay.ToString(), Text = EnumHelper<WashWaterUnits>.GetDisplayValue(WashWaterUnits.USGallonsPerDay) });

            return selectListItems;
        }

        public string ExplainWashWaterVolumesDaily;

    }
}