using Agri.Models.Farm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVERAPI.ViewModels
{
    public class ManureStorageMaterialsRequireAssigningViewModel
    {
        public string Title { get; set; }
        public List<GeneratedManure> UnallocatedGeneratedManures { get; set; }
        public string Message { get; set; }
        public string Target { get; set; }
    }
}
