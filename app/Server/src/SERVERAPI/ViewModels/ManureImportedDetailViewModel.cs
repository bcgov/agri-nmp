using Agri.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MvcRendering = Microsoft.AspNetCore.Mvc.Rendering;

namespace SERVERAPI.ViewModels
{
    public class ManureImportedDetailViewModel
    {
        public string Title { get; set; }
        public string Target { get; set; }
        public int? ManureImportId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string MaterialName { get; set; }
        public ManureMaterialType SelectedManureType { get; set; }
        public string ManureTypeName { get; set; }
        [Required(ErrorMessage = "Required")]
        [Range(0,99999990, ErrorMessage = "Enter a numeric value")]
        public decimal? AnnualAmount { get; set; }
        public AnnualAmountUnits SelectedAnnualAmountUnit { get; set; }
        public decimal? Moisture { get; set; }
        public decimal StandardSolidMoisture { get; set; }
        public bool IsStdMoisture => SelectedManureType == ManureMaterialType.Solid && Moisture.HasValue &&
                                     Moisture.Value == StandardSolidMoisture;
        public bool IsLandAppliedBeforeStorage { get; set; }
        public string LandAppliedLabelText { get; set; }
        public string ButtonText { get; set; }
        public string ButtonPressed { get; set; }

        public List<MvcRendering.SelectListItem> GetAnnualAmountUnits()
        {
            var selectListItems = new List<MvcRendering.SelectListItem>();

            if (SelectedManureType == ManureMaterialType.Solid)
            {

                selectListItems.Add(new MvcRendering.SelectListItem { Value = AnnualAmountUnits.Yards.ToString(), Text = EnumHelper<AnnualAmountUnits>.GetDisplayValue(AnnualAmountUnits.Yards) });
                selectListItems.Add(new MvcRendering.SelectListItem { Value = AnnualAmountUnits.tons.ToString(), Text = EnumHelper<AnnualAmountUnits>.GetDisplayValue(AnnualAmountUnits.tons) });
                selectListItems.Add(new MvcRendering.SelectListItem { Value = AnnualAmountUnits.CubicMeters.ToString(), Text = EnumHelper<AnnualAmountUnits>.GetDisplayValue(AnnualAmountUnits.CubicMeters) });
                selectListItems.Add(new MvcRendering.SelectListItem { Value = AnnualAmountUnits.tonnes.ToString(), Text = EnumHelper<AnnualAmountUnits>.GetDisplayValue(AnnualAmountUnits.tonnes) });
            }
            else
            {
                selectListItems.Add(new MvcRendering.SelectListItem { Value = AnnualAmountUnits.USGallons.ToString(), Text = EnumHelper<AnnualAmountUnits>.GetDisplayValue(AnnualAmountUnits.USGallons) });
                selectListItems.Add(new MvcRendering.SelectListItem { Value = AnnualAmountUnits.ImperialGallons.ToString(), Text = EnumHelper<AnnualAmountUnits>.GetDisplayValue(AnnualAmountUnits.ImperialGallons) });
                selectListItems.Add(new MvcRendering.SelectListItem { Value = AnnualAmountUnits.CubicMeters.ToString(), Text = EnumHelper<AnnualAmountUnits>.GetDisplayValue(AnnualAmountUnits.CubicMeters) });
            }

            return selectListItems;
        }
    }
}
