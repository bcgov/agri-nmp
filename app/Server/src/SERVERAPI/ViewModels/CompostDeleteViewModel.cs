using System.ComponentModel.DataAnnotations;

namespace SERVERAPI.ViewModels
{
    public class CompostDeleteViewModel
    {
        public string act { get; set; }
        public int id { get; set; }
        public string target { get; set; }
        [Display(Name = "Compost/Manure")]
        public string manureName { get; set; }
        public string warning { get; set; }
    }
}