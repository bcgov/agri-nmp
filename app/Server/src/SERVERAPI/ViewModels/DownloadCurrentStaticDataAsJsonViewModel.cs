using System.ComponentModel.DataAnnotations;

namespace SERVERAPI.ViewModels
{
    public class DownloadCurrentStaticDataAsJsonViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public int NewVersionId { get; set; }
        public bool ProcessingCompleted { get; set; }
        public bool Authenticated { get; set; }
        public string ErrorMessage { get; set; }
    }
}