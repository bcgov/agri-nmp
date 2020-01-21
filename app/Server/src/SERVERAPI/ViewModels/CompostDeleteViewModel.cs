using System.ComponentModel.DataAnnotations;

namespace SERVERAPI.ViewModels
{
    public class CompostDeleteViewModel
    {
        public string Action { get; set; }
        public int Id { get; set; }
        public string Target { get; set; }

        [Display(Name = "Compost/Manure")]
        public string ManureName { get; set; }

        public string Warning { get; set; }
    }
}