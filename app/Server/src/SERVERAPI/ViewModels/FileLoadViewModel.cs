using System.ComponentModel.DataAnnotations;

namespace SERVERAPI.ViewModels
{
    public class FileLoadViewModel
    {
        [Display(Name = "File Name")]
        public string fileName { get; set; }
        public bool unsavedData { get; set; }
        public bool badFile { get; set; }
        public string warningMsg { get; set; }
    }
}