using System.ComponentModel.DataAnnotations;

namespace SERVERAPI.ViewModels
{
    public class FertilizerDeleteViewModel
    {
        public string act { get; set; }
        public string fldName { get; set; }
        public int id { get; set; }
        [Display(Name = "Fertilizer")]
        public string fertilizerName { get; set; }
        public int? fertilizerTypeId { get; set; }
    }
}