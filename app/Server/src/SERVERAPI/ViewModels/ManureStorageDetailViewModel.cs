using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using SERVERAPI.Models;

namespace SERVERAPI.ViewModels
{
    public class ManureStorageDetailViewModel
    {
        public string Title { get; set; }
        public string Target { get; set; }
        [Required(ErrorMessage = "Select a Manure Material Type")]
        //[Range(1, 9999, ErrorMessage = "Select a Manure Material Type")]
        public StaticData.ManureMaterialType SelectedManureMaterialType { get; set; }

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
        public string SystemNamePlaceholder { get; set; }
        public int? SystemId { get; set; }
        public List<SelectListItem> GeneratedManures { get; set; }
        public bool GetsRunoffFromRoofsOrYards { get; set; }
        public int? RunoffAreaSquareFeet { get; set; }
        public string StorageStructureName { get; set; }
        public string StorageStructureNamePlaceholder { get; set; }
        public int UncoveredAreaOfStorageStructure { get; set; }
        public string ButtonText { get; set; }
        public string ButtonPressed { get; set; }
        public bool DisableMaterialTypeForEditMode { get; set; }
        public bool ShowRunOffQuestions => SelectedManureMaterialType == StaticData.ManureMaterialType.Liquid;
        public bool ShowRunoffAreaField => GetsRunoffFromRoofsOrYards;
    }

}
