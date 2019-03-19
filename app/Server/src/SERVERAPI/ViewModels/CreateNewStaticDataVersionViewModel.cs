using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVERAPI.ViewModels
{
    public class CreateNewStaticDataVersionViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public int NewVersionId { get; set; }
        public bool ProcessingCompleted { get; set; }
        public bool ArchiveWasSuccessful { get; set; }
        public bool Authenticated { get; set; }
        public string ErrorMessage { get; set; }
    }
}
