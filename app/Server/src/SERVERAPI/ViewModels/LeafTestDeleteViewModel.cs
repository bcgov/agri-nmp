using System.ComponentModel.DataAnnotations;

namespace SERVERAPI.ViewModels
{
    public class LeafTestDeleteViewModel
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string btnText { get; set; }
        [Display(Name = "Field Name")]
        public string fieldName { get; set; }
    }
}