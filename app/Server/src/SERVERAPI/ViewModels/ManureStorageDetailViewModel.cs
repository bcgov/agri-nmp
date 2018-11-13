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
        public IList<int> SelectedMaterialsToInclude { get; set; }
        public MultiSelectList GeneratedManures { get; set; }
        [Required(ErrorMessage = "Required")]
        public string SystemName { get; set; }
        public int? SystemId { get; set; }
        public bool GetsRunoffFromRoofsOrYards { get; set; }
        public int RunoffAreaSquareFeet { get; set; }
        public string Placeholder { get; set; }
        public string ButtonText { get; set; }
        public string ButtonPressed { get; set; }
        public bool DisableForEditMode { get; set; }
    }

}
