using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVERAPI.ViewModels
{
    public class CreateNewStaticDataVersionViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int NewVersionId { get; set; }
        public bool ArchiveWasSuccessful { get; set; }
        public string ErrorMessage { get; set; }
    }
}
