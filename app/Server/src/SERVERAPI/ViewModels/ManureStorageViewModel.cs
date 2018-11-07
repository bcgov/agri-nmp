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
    public class ManureStorageViewModel
    {
        public string Title { get; set; }
        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public StaticData.ManureMaterialType SelectedManureMaterialType { get; set; }
        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public List<int> SelectedMaterialsToInclude { get; set; }
        [Required(ErrorMessage = "Required")]
        public string SystemName { get; set; }
        public MultiSelectList GeneratedManures { get; set; }
        public string Placeholder { get; set; }
        public string ButtonText { get; set; }
        public string ButtonPressed { get; set; }
    }

}
