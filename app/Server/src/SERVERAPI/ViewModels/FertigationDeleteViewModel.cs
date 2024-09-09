using System.ComponentModel.DataAnnotations;

namespace SERVERAPI.ViewModels
{
    public class FertigationDeleteViewModel
    {
        public string act { get; set; }
        public string fldName { get; set; }
        public int id { get; set; }
        [Display(Name = "Fertilizer")]
        public string fertilizerName { get; set; }
    }
}
