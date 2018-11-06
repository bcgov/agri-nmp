using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SERVERAPI.Models;

namespace SERVERAPI.ViewModels
{
    public class ManureStorageViewModel
    {
        public string Title { get; set; }
        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public int SelectedManureMaterialType { get; set; }
        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public List<int> SelectedMaterialsToInclude { get; set; }
        [Required(ErrorMessage = "Required")]
        public string SystemName { get; set; }

        public List<GeneratedManure> GeneratedManures { get; set; }

        //public List<Models.StaticData.SelectListItem> ManureMaterialTypeOptions { get; set; }
        public List<Models.StaticData.ManureMaterialType> ManureMaterialTypeOptions { get; set; }
        public string Placeholder { get; set; }
        public string ButtonText { get; set; }
        public string ButtonPressed { get; set; }
    }

}
