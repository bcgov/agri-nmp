using System.ComponentModel.DataAnnotations;

namespace SERVERAPI.ViewModels
{
    public class CropDeleteViewModel
    {
        public string act { get; set; }
        public string fldName { get; set; }
        public int id { get; set; }
        [Display(Name = "Crop")]
        public string cropName { get; set; }
    }
}