using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Agri.Models.Configuration;

namespace SERVERAPI.ViewModels
{
    public class FieldDetailViewModel
    {
        [Display(Name = "Field Name")]
        [Required]
        public string fieldName { get; set; }
        [Display(Name = "Area")]
        [Required]
        public string fieldArea { get; set; }
        [Display(Name = "Comments (optional)")]     
        public string fieldComment { get; set; }

        public List<PreviousManureApplicationYear> selPrevYrManureOptions { get; set; }
        [Display(Name = "Manure application in previous years")]
        [Required]
        public string selPrevYrManureOption { get; set; }

        public string act { get; set; }
        public string userDataField { get; set; }
        public string currFieldName { get; set; }
        public string target { get; set; }
        public string cntl { get; set; }
        public string actn { get; set; }
        public string currFld { get; set; }
        public int fieldId { get; set; }
        public string placehldr { get; set; }
    }
}