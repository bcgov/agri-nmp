using System.ComponentModel.DataAnnotations;

namespace SERVERAPI.ViewModels
{
    public class RancherFieldDeleteViewModel
    {
        [Display(Name = "Field Name")]
        public string FieldName { get; set; }

        public string Act { get; set; }
        public string UserDataField { get; set; }
        public string Target { get; set; }
    }
}