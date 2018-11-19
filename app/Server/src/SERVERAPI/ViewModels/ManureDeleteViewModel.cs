using System.ComponentModel.DataAnnotations;

namespace SERVERAPI.ViewModels
{
    public class ManureDeleteViewModel
    {
        public string act { get; set; }
        public string fldName { get; set; }
        public int id { get; set; }
        [Display(Name = "Material Type")]
        public string matType { get; set; }
    }
}