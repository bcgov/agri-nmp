using System.ComponentModel.DataAnnotations;

namespace SERVERAPI.ViewModels
{
    public class OtherDeleteViewModel
    {
        public string act { get; set; }
        public string fldName { get; set; }
        public int id { get; set; }
        [Display(Name = "Nutrient Source")]
        public string source { get; set; }
    }
}