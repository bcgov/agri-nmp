using System.ComponentModel.DataAnnotations;

namespace SERVERAPI.ViewModels
{
    public class FieldDeleteViewModel
    {
        [Display(Name = "Field Name")]
        public string fieldName { get; set; }
        public string act { get; set; }
        public string userDataField { get; set; }
        public string target { get; set; }
    }
}