using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Agri.Models.Configuration;

namespace SERVERAPI.ViewModels
{
    public class RancherFieldDetailViewModel
    {
        [Display(Name = "Field Name")]
        [Required]
        public string FieldName { get; set; }

        [Display(Name = "Area")]
        [Required]
        public string FieldArea { get; set; }

        [Display(Name = "Comments (optional)")]
        public string FieldComment { get; set; }

        public List<PreviousManureApplicationYear> SelectPrevYrManureOptions { get; set; }

        [Display(Name = "Manure application in previous years")]
        public string SelectPrevYrManureOption { get; set; }

        public string Act { get; set; }
        public string UserDataField { get; set; }
        public string CurrFieldName { get; set; }
        public string Target { get; set; }
        public string Cntl { get; set; }
        public string Actn { get; set; }
        public string CurrFld { get; set; }
        public int FieldId { get; set; }
        public string Placehldr { get; set; }
        public bool IsSeasonalFeedingArea { get; set; }
        public string SeasonalFeedingArea { get; set; }
    }
}