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
        public List<int> SelectedMaterialsToInclude
        {
            get
            {
                if (GeneratedManures != null && GeneratedManures.Any())
                {
                    return GeneratedManures.Where(m => m.Selected).Select(gm => Convert.ToInt32(gm.Value)).ToList();
                }
                return new List<int>();
            }
        }

        [Required(ErrorMessage = "Required")]
        public string SystemName { get; set; }
        public int? SystemId { get; set; }
        public List<SelectListItem> GeneratedManures { get; set; }
        public bool GetsRunoffFromRoofsOrYards { get; set; }
        public int? RunoffAreaSquareFeet { get; set; }
        public int? StorageStructureId { get; set; }
        public string StorageStructureName { get; set; }
        [Required(ErrorMessage = "Required")]
        public string StorageStructureNamePlaceholder { get; set; }
        public int? UncoveredAreaOfStorageStructure { get; set; }
        public bool IsStructureCovered { get; set; }
        public string ButtonText { get; set; }
        public string ButtonPressed { get; set; }
        public bool DisableMaterialTypeForEditMode { get; set; }
        public bool ShowRunOffQuestions => SelectedManureMaterialType == ManureMaterialType.Liquid;
        public bool ShowRunoffAreaField => GetsRunoffFromRoofsOrYards;
        public bool DisableSystemFields { get; set; }
        public bool ShowStructureFields { get; set; }
    }

}
