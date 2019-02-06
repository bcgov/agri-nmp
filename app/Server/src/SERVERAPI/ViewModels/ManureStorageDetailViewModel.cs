using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Agri.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SERVERAPI.ViewModels
{
    public class ManureStorageDetailViewModel
    {
        public string Title { get; set; }
        public string Target { get; set; }
        [Required(ErrorMessage = "Select a Manure Material Type")]
        //[Range(1, 9999, ErrorMessage = "Select a Manure Material Type")]
        public ManureMaterialType SelectedManureMaterialType { get; set; }

        [Required(ErrorMessage = "Required")]
        public List<string> SelectedMaterialsToInclude
        {
            get
            {
                if (ManagedManures != null && ManagedManures.Any())
                {
                    return ManagedManures.Where(m => m.Selected).Select(gm => gm.Value).ToList();
                }
                return new List<string>();
            }
        }

        [Required(ErrorMessage = "Required")]
        public string SystemName { get; set; }
        public int? SystemId { get; set; }
        public List<SelectListItem> ManagedManures { get; set; }
        public bool AnyManagedManures => ManagedManures == null || (ManagedManures.Any() && SelectedManureMaterialType > 0);
        public bool GetsRunoffFromRoofsOrYards { get; set; }
        public int? RunoffAreaSquareFeet { get; set; }
        public int? StorageStructureId { get; set; }
        public string StorageStructureName { get; set; }
        [Required(ErrorMessage = "Required")]
        public string StorageStructureNamePlaceholder { get; set; }
        public int? UncoveredAreaOfStorageStructure { get; set; }
        public bool IsStructureCovered { get; set; }
        public bool IsThereSolidLiquidSeparation { get; set; }
        public int PercentageOfLiquidVolumeSeparated { get; set; }
        public decimal SeparatedLiquidsUSGallons { get; set; }
        public string SeparatedLiquidsUSGallonsText => SeparatedLiquidsUSGallons.ToString("0");
        public decimal SeparatedSolidsTons { get; set; }
        public string SeparatedSolidsTonsText => SeparatedSolidsTons.ToString("0");
        public string ButtonText { get; set; }
        public string ButtonPressed { get; set; }
        public bool DisableMaterialTypeForEditMode { get; set; }
        public bool ShowRunOffQuestions => SelectedManureMaterialType == ManureMaterialType.Liquid;
        public bool ShowRunoffAreaField => GetsRunoffFromRoofsOrYards;
        public bool ShowSolidLiquidSeparation => SelectedManureMaterialType == ManureMaterialType.Liquid;
        public bool ShowSeparatedValueFields { get; set; }
        public bool DisableSystemFields { get; set; }
        public bool ShowStructureFields { get; set; }
        public string ZeroManagedManuresMessage { get; set; }
        public int? AnnualPrecipitation { get; set; }
        [Required(ErrorMessage = "Select a Storage Shape")]
        public StorageShapes SelectedStorageShape { get; set; }
        public decimal? RectangularLength { get; set; }
        public decimal? RectangularWidth { get; set; }
        public decimal? RectangularHeight { get; set; }
        public decimal? CircularDiameter { get; set; }
        public decimal? CircularHeight { get; set; }
        public decimal? SlopedWallTopLength { get; set; }
        public decimal? SlopedWallTopWidth { get; set; }
        public decimal? SlopedWallHeight { get; set; }
        public decimal? SlopedWallSlopeOfWall { get; set; }
        public int? surfaceArea { get; set; }
        public int? volumeUSGallons { get; set; }
        public string volumeOfStorageStructure { get; set; }
        public int? volumeOfStorageSystem { get; set; }
        public string volumeUSGallonsOfStorageSystem { get; set; }
    }

}
